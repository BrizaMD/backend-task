using System;
using System.IO;
using System.Linq;
using TruckOrderOptimizer.Helpers;
using TruckOrderOptimizer.Services;

namespace TruckOrderOptimizer
{
    public static class Program
    {
        public static void Main()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "mediordev.txt");
            FileParser.ParseTrucksAndJobs(filePath, out var vehicles, out var jobs);

            var (assignments, unassignedJobs, unassignedVehicles) = JobAssignmentService.AssignJobs(vehicles, jobs);

            // Printing the assignments
            foreach (var assignment in assignments)
            {
                var vehicle = vehicles.First(v => v.Id == assignment.Value);
                var job = jobs.First(j => j.Id == assignment.Key);
                Console.WriteLine($"Vehicle {vehicle.Id} ({string.Join(" ", vehicle.CompatibleJobs)}) -> Job {job.Id} ({job.Type})");
            }

            Console.WriteLine(($"Number of assignments made: {assignments.Count}"));

            
            // Printing the unassigned jobs
            Console.WriteLine("Unassigned Jobs:");
            foreach (var unassignedJob in unassignedJobs)
            {
                Console.WriteLine($"Job {unassignedJob.Id} ({unassignedJob.Type})");
            }

            // Printing the unassigned vehicles
            Console.WriteLine("Unassigned Vehicles:");
            foreach (var unassignedVehicle in unassignedVehicles)
            {
                Console.WriteLine($"Vehicle {unassignedVehicle.Id} ({string.Join(" ", unassignedVehicle.CompatibleJobs)})");
            }
        }
    }
}