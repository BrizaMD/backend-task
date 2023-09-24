namespace TruckOrderOptimizer.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsAssigned { get; set; }
    }
}