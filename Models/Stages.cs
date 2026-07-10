using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class Stages
    {
        
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Groups { get; set; }
        public int Division { get; set; }
        public Stages(string Name) {
            this.Name = Name;
        }

        public Stages()
        {
        }

        public Stages(int id, string name, int v1, int v2)
        {
            Id = id;
            Name = name;
            this.Groups = v1;
            this.Division = v2;
        }
    }
}
