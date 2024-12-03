using System;
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
    public interface IMyNoSqlWriterWrapper<TEntity> where TEntity : IMyNoSqlDbEntity, new()
    {
        Task<bool> TryInsertOrReplaceAsync(TEntity entity);
        Task<bool> TryDeleteAsync(string partitionKey, string rowKey);
        void Start(Func<IList<TEntity>> readAllRecordsCallback, TimeSpan? reloadTimerPeriod = null);
        void StartWithClearing(int countInCache, TimeSpan? reloadTimerPeriod = null);
        Task CleanAndBulkInsertAsync(string partitionKey, IEnumerable<TEntity> list);
        Task Clear();
    }

    public class MyNoSqlWriterWrapper<TEntity> : IDisposable, IMyNoSqlWriterWrapper<TEntity> where TEntity : IMyNoSqlDbEntity, new()
    {
        private readonly IMyNoSqlServerDataWriter<TEntity> _writer;
        private Func<IList<TEntity>> _readAllRecordsCallback;
        private int? _maxInCache;

        private readonly ILog _log;
        private Timer _timer;
        private bool _isStarted = false;
        private readonly object _sync = new object();

        private TimeSpan _reloadTimerPeriod;


        public MyNoSqlWriterWrapper(
            IMyNoSqlServerDataWriter<TEntity> writer,
            ILogFactory logFactory)
        {
            _writer = writer;
            _log = logFactory.CreateLog(this);
            
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

        public void Start(Func<IList<TEntity>> readAllRecordsCallback, TimeSpan? reloadTimerPeriod = null)
        {
            if (readAllRecordsCallback == null)
                throw new ArgumentException($"readAllRecordsCallback cannot be null. TEntity: {typeof(TEntity).Name}", nameof(readAllRecordsCallback));

            lock (_sync)
            {
                _reloadTimerPeriod = reloadTimerPeriod ?? TimeSpan.FromMinutes(10);
                _timer = new Timer(DoTimer);
                _isStarted = true;
            }

            _readAllRecordsCallback = readAllRecordsCallback;
            _maxInCache = null;


            DoTimer(null);

            _log.Info($"Started wrapper MyNoSql table for entity {typeof(TEntity).Name}");
        }

        public void StartWithClearing(int countInCache, TimeSpan? reloadTimerPeriod = null)
        {
            lock (_sync)
            {
                _reloadTimerPeriod = reloadTimerPeriod ?? TimeSpan.FromMinutes(10);
                _timer = new Timer(DoTimer);
                _isStarted = true;
            }

            _readAllRecordsCallback = null;
            _maxInCache = countInCache;


            DoTimer(null);

            _log.Info($"Started wrapper MyNoSql table for entity {typeof(TEntity).Name}");
        }

        public async Task Clear()
        {
            await _writer.CleanAndKeepMaxPartitions(0);
        }

        public async Task CleanAndBulkInsertAsync(string partitionKey, IEnumerable<TEntity> list)
        {
            await _writer.CleanAndBulkInsertAsync(partitionKey, list);
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
                if (_readAllRecordsCallback != null)
                {
                    var data = _readAllRecordsCallback.Invoke();

                    foreach (var group in data.GroupBy(e => e.PartitionKey))
                    {
                        _writer.CleanAndBulkInsertAsync(group.Key, group).GetAwaiter().GetResult();
                    }

                    _log.Info($"Reload MyNoSql table for entity {typeof(TEntity).Name}. Count Record: {data.Count}");
                }

                if (_maxInCache.HasValue)
                {
                    _writer.CleanAndKeepMaxPartitions(_maxInCache.Value).GetAwaiter().GetResult();
                    _log.Info($"Clear MyNoSql table for entity {typeof(TEntity).Name}. Max Record: {_maxInCache}");
                }
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
                if (_isStarted)
                {
                    _isStarted = false;
                    _timer?.Dispose();
                }
            }
        }
    }
}
