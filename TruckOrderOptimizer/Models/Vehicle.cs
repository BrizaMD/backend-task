using System.Collections.Generic;

namespace TruckOrderOptimizer.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public HashSet<string> CompatibleJobs { get; set; } = new HashSet<string>();
        public bool IsAssigned { get; set; }
    }
}