﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/watch-lists")]
    public class WatchListController : Controller
    {
        private readonly IWatchListService _watchListService;


        public WatchListController(
            IWatchListService watchListService)
        {
            _watchListService = watchListService;
        }
        
        [HttpDelete("custom/{watchListId}")]
        [SwaggerOperation("WatchListCustomRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteCustom(string watchListId, [FromQuery] string userId)
        {
            await _watchListService.RemoveCustomAsync(userId, watchListId);

            return NoContent();
        }

        [HttpDelete("predefined/{watchListId}")]
        [SwaggerOperation("WatchListPredefinedRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeletePredefined(string watchListId)
        {
            await _watchListService.RemovePredefinedAsync(watchListId);

            return NoContent();
        }

        [HttpGet("all")]
        [SwaggerOperation("WatchListGetAll")]
        [ProducesResponseType(typeof(IEnumerable<WatchList>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] string userId)
        {
            var watchLists = (await _watchListService.GetAllAsync(userId))
                .Select(Mapper.Map<WatchList>);

            return Ok(watchLists);
        }

        [HttpGet("custom")]
        [SwaggerOperation("WatchListGetAllCustom")]
        [ProducesResponseType(typeof(IEnumerable<WatchList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustom([FromQuery] string userId)
        {
            var watchLists = (await _watchListService.GetAllCustomAsync(userId))
                .Select(Mapper.Map<WatchList>);

            return Ok(watchLists);
        }

        [HttpGet("custom/{watchListId}")]
        [SwaggerOperation("WatchListGetCustom")]
        [ProducesResponseType(typeof(WatchList), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCustom(string watchListId, [FromQuery] string userId)
        {
            var watchList = await _watchListService.GetCustomAsync(watchListId, userId);

            if (watchList != null)
            {
                return Ok(Mapper.Map<WatchList>(watchList));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("predefined")]
        [SwaggerOperation("WatchListGetAllPredefined")]
        [ProducesResponseType(typeof(IEnumerable<WatchList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPredefined()
        {
            var watchLists = (await _watchListService.GetAllPredefinedAsync())
                .Select(Mapper.Map<WatchList>);

            return Ok(watchLists);
        }

        [HttpGet("predefined/{watchListId}")]
        [SwaggerOperation("WatchListGetPredefined")]
        [ProducesResponseType(typeof(WatchList), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPredefined(string watchListId)
        {
            var watchList = await _watchListService.GetPredefinedAsync(watchListId);

            if (watchList != null)
            {
                return Ok(Mapper.Map<WatchList>(watchList));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("custom")]
        [SwaggerOperation("WatchListAddCustom")]
        [ProducesResponseType(typeof(WatchList), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> PostCustom([FromBody] WatchList watchList, [FromQuery] string userId)
        {
            watchList = Mapper.Map<WatchList>(await _watchListService.AddCustomAsync(userId, watchList));

            return Created
            (
                uri:   $"api/v2/watch-lists/custom/{watchList.Id}?userId={userId}",
                value: watchList
            );
        }

        [HttpPost("predefined")]
        [SwaggerOperation("WatchListAddPredefined")]
        [ProducesResponseType(typeof(WatchList), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> PostPredefined([FromBody] WatchList watchList)
        {
            watchList = Mapper.Map<WatchList>(await _watchListService.AddPredefinedAsync(watchList));

            return Created
            (
                uri:   $"api/v2/watch-lists/predefined/{watchList.Id}",
                value: watchList
            );
        }

        [HttpPut("custom")]
        [SwaggerOperation("WatchListUpdateCustom")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> PutCustom([FromBody] WatchList watchList, [FromQuery] string userId)
        {
            await _watchListService.UpdateCustomAsync(userId, watchList);

            return NoContent();
        }

        [HttpPut("predefined")]
        [SwaggerOperation("WatchListUpdatePredefined")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> PutPredefined([FromBody] WatchList watchList)
        {
            await _watchListService.UpdatePredefinedAsync(watchList);

            return NoContent();
        }
    }
}