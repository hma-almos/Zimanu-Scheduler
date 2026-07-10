using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class ScheduleHour
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Hours { get; set; }
        public ScheduleHour(int Hours)
        {
            this.Hours = Hours;
        }
    }
}