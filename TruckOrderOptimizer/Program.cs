using System;
using System.Diagnostics;
using System.IO;
using TruckOrderOptimizer.Helpers;
using TruckOrderOptimizer.Services;
using Serilog;

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

                var stopwatch = Stopwatch.StartNew();

                var (vehicles, jobs) = FileParser.ParseTrucksAndJobs(filePath);
                var (assignments, unassignedJobs, unassignedVehicles) = JobAssignmentService.AssignJobs(vehicles, jobs);

                stopwatch.Stop();

                Log.Information("Elapsed time: {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

                AssignmentReporter.PrintAssignments(assignments, vehicles, jobs);
                AssignmentReporter.PrintUnassignedJobs(unassignedJobs);
                AssignmentReporter.PrintUnassignedVehicles(unassignedVehicles);

                string timeStamp = DateTime.Now.ToString("HH-mm-ss-fff");
                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"output {timeStamp}.txt");

                using (StreamWriter writer = new StreamWriter(outputPath))
                {
                    AssignmentReporter.WriteAssignments(writer, assignments, vehicles, jobs);
                    AssignmentReporter.WriteUnassignedJobs(writer, unassignedJobs);
                    AssignmentReporter.WriteUnassignedVehicles(writer, unassignedVehicles);
                }
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
    }
}