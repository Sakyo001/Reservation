using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAB_FINAL_ACT.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public int BidetNumber { get; set; }
        public DateTime DateAdded { get; set; }
        public string EventName { get; set; }
        public string SetupLocation { get; set; }
        public TimeSpan OpeningHour { get; set; }
        public TimeSpan ClosingHour { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Add NotMapped attribute to exclude these from database mapping
        [NotMapped]
        public string DateAddedString { get; set; }
        [NotMapped]
        public string OpeningHourString { get; set; }
        [NotMapped]
        public string ClosingHourString { get; set; }
    }
}