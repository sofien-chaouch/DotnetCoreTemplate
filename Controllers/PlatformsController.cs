using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.Services.Platform;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly ILogger<PlatformsController> _logger;
        private readonly IDistributedCache _cache;

        public PlatformsController(
            IPlatformService platformService, 
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient,
            ILogger<PlatformsController> logger,
            IDistributedCache cache)
        {
            _platformService = platformService;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
            _logger = logger;
            _cache = cache;
        }
        
        public string? CachedTimeUTC { get; set; }
        public string? ASP_Environment { get; set; }


    
        [HttpGet("OnGetAsyncCache")]
        public async Task<IActionResult>  OnGetAsync()
        {
            CachedTimeUTC = "Cached Time Expired";
            var encodedCachedTimeUTC = await _cache.GetAsync("cachedTimeUTC");

            if (encodedCachedTimeUTC != null)
            {
                CachedTimeUTC = Encoding.UTF8.GetString(encodedCachedTimeUTC);
            }

            ASP_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (String.IsNullOrEmpty(ASP_Environment))
            {
                ASP_Environment = "Null, so Production";
            }
            
            return null;

        }
        
    
        [HttpPost("OnPostAsyncCache")]
        public async Task<IActionResult> OnPostResetCachedTime()
        {
            var currentTimeUTC = DateTime.UtcNow.ToString();
            byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));
            await _cache.SetAsync("cachedTimeUTC", encodedCurrentTimeUTC, options);

            return null;
        }
        
        [HttpPost("Notif")]
        public async Task<IActionResult> Notif([FromBody] JObject data)
        {
            try
            {
                return Ok(new Response<object>(true, "successfully", false, 200));
            }
            catch (Exception e)
            {
                _logger.LogInformation(new CustomLog(e.Message, e.GetType().ToString()).GetLog());
                return Ok(new Response<Object>(null, "exception : " + e.Message, true, 500));
            }
        }
        
        [HttpGet("GetPlatforms")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");

            var platformItem = _platformService.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _platformService.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _platformService.AddPlatform(platformModel);

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            // Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id}, platformReadDto);
        }
    }
}