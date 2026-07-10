using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication5.Models
{
    public class FinalSchedule
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key to Teacher_Lecture table
        public int Lec_id { get; set; }
        [ForeignKey("Lec_id")]
        public Teacher_Lecture Lec { get; set; }

        // Foreign Key to ScheduleHour table
        public int Hour_id { get; set; }
        [ForeignKey("Hour_id")]
        public ScheduleHour Hours { get; set; }

        // Foreign Key to Dayes table
        public int Day_id { get; set; }
        [ForeignKey("Day_id")]
        public Dayes Day { get; set; }

        // Foreign Key to Rooms table
        public int Room_id { get; set; }
        [ForeignKey("Room_id")]
        public Rooms Room { get; set; }

        // Foreign Key to Stage table
        public int Stage_id { get; set; }
        [ForeignKey("Stage_id")]
        public Stages Stage { get; set; }
    }
}
