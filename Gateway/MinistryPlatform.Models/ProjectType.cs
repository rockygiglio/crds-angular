namespace MinistryPlatform.Models
{
    public class ProjectType
    {
        public int ProjectTypeId { get; set; }
        public string Description { get; set; }
        public int MinAge { get; set; }
        public int SortOrder { get; set; }
        public string ImageUrl { get; set; }
    }
}
