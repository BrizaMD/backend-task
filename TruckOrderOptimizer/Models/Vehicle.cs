using System.Collections.Generic;

namespace TruckOrderOptimizer.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public List<string> CompatibleJobs { get; set; }
        public bool IsAssigned { get; set; }
    }
}