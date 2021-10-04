using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestiRecord.API.Models
{
    public class PrayerRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RequestedBy { get; set; }
        public string Sickness { get; set; }
        public bool IsHealed { get; set; }
        public DateTime DateRequested { get; set; }
    }
}
