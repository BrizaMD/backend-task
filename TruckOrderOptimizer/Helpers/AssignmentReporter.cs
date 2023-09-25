using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;
using TruckOrderOptimizer.Models;

namespace TruckOrderOptimizer.Helpers
{
    public static class AssignmentReporter
    {
        public static void PrintAssignments(Dictionary<int, int> assignments, List<Vehicle> vehicles, List<Job> jobs)
        {
            Log.Information("Number of assignments made: {AssignmentsCount}", assignments.Count);

            foreach (var assignment in assignments)
            {
                var vehicle = vehicles.FirstOrDefault(v => v.Id == assignment.Value);
                var job = jobs.FirstOrDefault(j => j.Id == assignment.Key);

                if (vehicle != null && job != null)
                {
                    Log.Information("Vehicle {VehicleId} ({Join}) -> Job {JobId} ({JobType})", vehicle.Id, string.Join(" ", vehicle.CompatibleJobs), job.Id, job.Type);
                }
            }
        }

        public static void PrintUnassignedJobs(List<Job> unassignedJobs)
        {
            Log.Information("Unassigned Jobs:");
            foreach (var unassignedJob in unassignedJobs)
            {
                Log.Information("Job {UnassignedJobId} ({UnassignedJobType})", unassignedJob.Id, unassignedJob.Type);
            }
        }

        public static void PrintUnassignedVehicles(List<Vehicle> unassignedVehicles)
        {
            Log.Information("Unassigned Vehicles:");
            foreach (var unassignedVehicle in unassignedVehicles)
            {
                Log.Information("Vehicle {UnassignedVehicleId} ({Join})", unassignedVehicle.Id, string.Join(" ", unassignedVehicle.CompatibleJobs));
            }
        }

        public static void WriteAssignments(StreamWriter writer, Dictionary<int, int> assignments, List<Vehicle> vehicles, List<Job> jobs)
        {
            writer.WriteLine($"Number of assignments made: {assignments.Count}");

            foreach (var assignment in assignments)
            {
                var vehicle = vehicles.FirstOrDefault(v => v.Id == assignment.Value);
                var job = jobs.FirstOrDefault(j => j.Id == assignment.Key);

                if (vehicle != null && job != null)
                {
                    writer.WriteLine($"Vehicle {vehicle.Id} ({string.Join(" ", vehicle.CompatibleJobs)}) -> Job {job.Id} ({job.Type})");
                }
            }
        }

        public static void WriteUnassignedJobs(StreamWriter writer, List<Job> unassignedJobs)
        {
            writer.WriteLine("Unassigned Jobs:");
            foreach (var unassignedJob in unassignedJobs)
            {
                writer.WriteLine($"Job {unassignedJob.Id} ({unassignedJob.Type})");
            }
        }

        public static void WriteUnassignedVehicles(StreamWriter writer, List<Vehicle> unassignedVehicles)
        {
            writer.WriteLine("Unassigned Vehicles:");
            foreach (var unassignedVehicle in unassignedVehicles)
            {
                writer.WriteLine($"Vehicle {unassignedVehicle.Id} ({string.Join(" ", unassignedVehicle.CompatibleJobs)})");
            }
        }
    }
}