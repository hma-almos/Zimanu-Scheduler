using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication5.Models
{
    public class Rooms
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public string Type { get; set; }
        
        public Rooms(String Name,String type)
        {
            this.Name =Name;
            this.Type = type;
        }

        
        
    }
}
