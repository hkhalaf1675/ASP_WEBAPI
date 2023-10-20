namespace Day2_Lab_WebAPI.DTO
{
    public class DepartmentDataWithListOfStudentDTO
    {
        public int ID { get; set; }
        public string DepartmentName { get; set; }
        public List<string?> StudentName { get; set; } = new List<string?>();
    }
}
