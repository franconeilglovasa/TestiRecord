using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestiRecord.API.Models;
using TestiRecord.API.Services;

namespace TestiRecord.API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly TestiService _testiService;

        public DataController(TestiService testiService)
        {
            _testiService = testiService;
        }

        [HttpGet("GetAllPrayerRequests")]
        public async Task<List<PrayerRequest>> GetAllPrayerRequests()
        {

            var data =  await _testiService.GetAllPrayerRequest();
            return data;
        }

        [HttpGet("GetPrayerRequest")]
        public async Task<PrayerRequest> GetPrayerRequest(int id)
        {

            return await _testiService.GetPrayerRequest(id);
        }

        [HttpGet("GetHealedPrayerRequest")]
        public async Task<List<PrayerRequest>> GetHealedPrayerRequest()
        {

            return await _testiService.GetHealedPrayerRequest();
        }

        [HttpGet("GetStillProcessPrayerRequest")]
        public async Task<List<PrayerRequest>> GetStillProcessPrayerRequest()
        {

            return await _testiService.GetStillProcessPrayerRequest();
        }

        [HttpPut("AddOrEditPrayerRequest")]
        public async Task<bool> AddOrEditPrayerRequest(PrayerRequest prayerRequest)
        {
            if (prayerRequest.Id > 0)
            {
                return await _testiService.UpdatePrayerRequest(prayerRequest);
            } else
            {
                return await _testiService.AddPrayerRequest(prayerRequest);
            }
            
        }

        [HttpPut("UpdatePrayerRequest")]
        public async Task<bool> UpdatePrayerRequest(PrayerRequest prayerRequest)
        {

            return await _testiService.UpdatePrayerRequest(prayerRequest);
        }

        [HttpDelete("DeletePrayerRequest")]
        public async Task<bool> DeletePrayerRequest(int id)
        {

            return await _testiService.DeletePrayerRequest(id);
        }





    }
}
