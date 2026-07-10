namespace WebApplication5.dto
{
    public class Teacher_Lecture_id
    {
        public List<SelectedSubject> SelectedData { get; set; }  // Represents the selected subjects and their teachers
        public List<int> InputValues { get; set; }              // Represents the input values like theoryGroups, practicalGroups
    }

    public class SelectedSubject
    {
        public int SubjectId { get; set; }
        public List<int> TeacherIds { get; set; }
        public bool theory { get; set; }
        public bool practical { get; set; }
    }
}
