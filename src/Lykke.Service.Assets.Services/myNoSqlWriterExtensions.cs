﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.Services
{
    public class MyNoSqlWriterWrapper<TEntity> : IDisposable where TEntity : IMyNoSqlDbEntity, new()
    {
        private readonly IMyNoSqlServerDataWriter<TEntity> _writer;
        private readonly Func<IList<TEntity>> _readAllRecordsCallback;
        private readonly ILog _log;
        private Timer _timer;
        private bool _isStarted = false;
        private readonly object _sync = new object();

        private readonly TimeSpan _reloadTimerPeriod;


        public MyNoSqlWriterWrapper(
            IMyNoSqlServerDataWriter<TEntity> writer,
            Func<IList<TEntity>> readAllRecordsCallback,
            ILog log,
            TimeSpan? reloadTimerPeriod = null)
        {
            _writer = writer;
            _readAllRecordsCallback = readAllRecordsCallback 
                ?? throw new ArgumentException($"readAllRecordsCallback cannot be null. TEntity: {typeof(TEntity).Name}", nameof(readAllRecordsCallback));
            _log = log;

            _reloadTimerPeriod = reloadTimerPeriod ?? TimeSpan.FromMinutes(10);
        }

        public async Task<bool> TryInsertOrReplaceAsync(TEntity entity)
        {
            try
            {
                await _writer.InsertOrReplaceAsync(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, $"Cannot execute InsertOrReplaceAsync to MyNoSql with entity {typeof(TEntity).Name}", $"data: {entity.ToJson()}");
                return false;
            }
        }

        public async Task<bool> TryDeleteAsync(string partitionKey, string rowKey)
        {
            try
            {
                await _writer.DeleteAsync(partitionKey, rowKey);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, $"Cannot execute DeleteAsync to MyNoSql with entity {typeof(TEntity).Name}", $"partitionKey: {partitionKey}; rowKey: {rowKey}");
                return false;
            }
        }

        public void Start()
        {
            lock (_sync)
            {
                _timer = new Timer(DoTimer);
                _isStarted = true;
            }
            
            DoTimer(null);

            _log.Info($"Started wrapper MyNoSql table for entity {typeof(TEntity).Name}");
        }

        private void DoTimer(object state)
        {
            lock (_sync)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            if (!_isStarted)
                return;

            try
            {
                var data = _readAllRecordsCallback.Invoke();

                foreach (var group in data.GroupBy(e => e.PartitionKey))
                {
                    _writer.CleanAndBulkInsertAsync(group.Key, group).GetAwaiter().GetResult();
                }

                _log.Info($"Reload MyNoSql table for entity {typeof(TEntity).Name}. Count Record: {data.Count}");
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot refresh data in MyNoSQL table. Entity: {typeof(TEntity).Name}");
            }

            lock (_sync)
            {
                if (_isStarted)
                {
                    _timer?.Change(_reloadTimerPeriod, _reloadTimerPeriod);
                }
            }
        }

        public void Dispose()
        {
            lock (_sync)
            {
                _isStarted = false;
                _timer?.Dispose();
            }
        }
    }
}
