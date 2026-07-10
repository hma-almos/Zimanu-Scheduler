using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication5.Models
{
    public class Lecture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
        //[ForeignKey("Stage_id")] 
        public required int Stages { get; set; }
        public int Room { get; set; }
        public int hours {  get; set; }


    }
}
