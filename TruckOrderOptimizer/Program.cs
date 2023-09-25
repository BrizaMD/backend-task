using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TruckOrderOptimizer.Helpers;
using TruckOrderOptimizer.Services;
using Serilog;
using TruckOrderOptimizer.Models;

namespace TruckOrderOptimizer
{
    public static class Program
    {
        public static void Main()
        {
            ConfigureLogging();

            try
            {
                var filePath = ResolveFilePath("mediordev.txt");

                // Start the stopwatch
                var stopwatch = Stopwatch.StartNew();

                var (vehicles, jobs) = FileParser.ParseTrucksAndJobs(filePath);
                var (assignments, unassignedJobs, unassignedVehicles) = JobAssignmentService.AssignJobs(vehicles, jobs);

                // Stop the stopwatch
                stopwatch.Stop();

                // Log the elapsed time
                Log.Information("Elapsed time: {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

                // PrintAssignments(assignments, vehicles, jobs);
                // PrintUnassignedJobs(unassignedJobs);
                // PrintUnassignedVehicles(unassignedVehicles);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, "The specified file could not be found");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static string ResolveFilePath(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            if (!File.Exists(path) || new FileInfo(path).Length == 0)
            {
                throw new FileNotFoundException($"The specified file {fileName} does not exist or is empty");
            }
            return path;
        }

        private static void PrintAssignments(Dictionary<int, int> assignments, List<Vehicle> vehicles, List<Job> jobs)
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

        private static void PrintUnassignedJobs(List<Job> unassignedJobs)
        {
            Log.Information("Unassigned Jobs:");
            foreach (var unassignedJob in unassignedJobs)
            {
                Log.Information($"Job {unassignedJob.Id} ({unassignedJob.Type})");
            }
        }

        private static void PrintUnassignedVehicles(List<Vehicle> unassignedVehicles)
        {
            Log.Information("Unassigned Vehicles:");
            foreach (var unassignedVehicle in unassignedVehicles)
            {
                Log.Information($"Vehicle {unassignedVehicle.Id} ({string.Join(" ", unassignedVehicle.CompatibleJobs)})");
            }
        }
    }
}