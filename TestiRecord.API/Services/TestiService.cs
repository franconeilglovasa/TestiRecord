using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestiRecord.API.Data;
using TestiRecord.API.Models;

namespace TestiRecord.API.Services
{
    public class TestiService
    {
        private readonly DataContext _context;

        public TestiService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<PrayerRequest>> GetAllPrayerRequest()
        {
            try
            {
                return await _context.PrayerRequests.ToListAsync();
            } catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<List<PrayerRequest>> GetHealedPrayerRequest()
        {
            return await _context.PrayerRequests.Where(p => p.IsHealed == true).ToListAsync();
        }

        public async Task<List<PrayerRequest>> GetStillProcessPrayerRequest()
        {
            return await _context.PrayerRequests.Where(p => p.IsHealed == false).ToListAsync();
        }

        public async Task<PrayerRequest> GetPrayerRequest(int id)
        {
            return await _context.PrayerRequests.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> AddPrayerRequest (PrayerRequest prayerRequest)
        {
            try
            {
                prayerRequest.DateRequested = DateTime.Now;
                await _context.PrayerRequests.AddAsync(prayerRequest);
                await _context.SaveChangesAsync();

                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePrayerRequest (PrayerRequest prayerRequest)
        {
            try
            {
                _context.PrayerRequests.Update(prayerRequest);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeletePrayerRequest (int id)
        {
            try
            {
                var data = await _context.PrayerRequests.Where(p => p.Id == id).FirstOrDefaultAsync();
                _context.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
