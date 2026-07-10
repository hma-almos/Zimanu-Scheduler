using WebApplication5.Models;
namespace WebApplication5.dto
{
    public class SaveBulkDataRequest
    {
        public List<Teacher> Teachers { get; set; }
        public List<Lecture> Subjects { get; set; }
        public List<Stages> Stages { get; set; }
        public List <ScheduleHour> Time { get; set; }
        public List<Rooms> Rooms { get; set; }
        
    }
}
