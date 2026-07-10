using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class Teacher_Lecture
    {
        [Key]
        public int Id { get; set; }

        // Foreign key properties (for the relationship with Teacher and Lecture)
        public int Teacher_Id { get; set; }  // Foreign key to Teacher
        public int Lecture_Id { get; set; }     // Foreign key to Lecture
        public bool theory { get; set; }
        public bool practical { get; set; }

        // Navigation properties
        [ForeignKey("Teacher_Id")]
        public Teacher Teacher { get; set; }

        [ForeignKey("Lecture_Id")]
        public Lecture Lecture { get; set; }
    }
}
