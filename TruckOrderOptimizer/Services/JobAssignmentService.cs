using System.Collections.Generic;
using System.Linq;
using TruckOrderOptimizer.Models;

namespace TruckOrderOptimizer.Services
{
    public static class JobAssignmentService
    {
        public static (Dictionary<int, int> Assignments, List<Job> UnassignedJobs, List<Vehicle> UnassignedVehicles) AssignJobs(List<Vehicle> vehicles, List<Job> jobs)
        {
            var assignments = new Dictionary<int, int>();

            foreach (var job in jobs)
            {
                foreach (var vehicle in vehicles)
                {
                    if (!vehicle.IsAssigned && vehicle.CompatibleJobs.Contains(job.Type))
                    {
                        assignments[job.Id] = vehicle.Id;
                        vehicle.IsAssigned = true;
                        job.IsAssigned = true;
                        break;
                    }
                }
            }

            var unassignedJobs = jobs.Where(j => !j.IsAssigned).ToList();
            var unassignedVehicles = vehicles.Where(v => !v.IsAssigned).ToList();

            return (assignments, unassignedJobs, unassignedVehicles);
        }
    }
}