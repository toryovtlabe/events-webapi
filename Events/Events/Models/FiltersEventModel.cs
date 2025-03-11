namespace Events.Models
{
    public class FiltersEventModel
    {
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Place { get; set; }
        public DateTime? Date { get; set; }
        public int Page {  get; set; }
    }
}
