using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class Dayes
    {
        [Key]
        public int Id { get; set; }
        public string DayName { get; set; }
    }
}
