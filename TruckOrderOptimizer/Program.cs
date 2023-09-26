using System;
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

                var (vehicles, jobs) = FileParser.ParseTrucksAndJobs(filePath);
                var assignments = JobAssignmentService.AssignJobs(vehicles, jobs);

                foreach (var assignment in assignments)
                {
                    Console.WriteLine($"{assignment.Value} {assignment.Key}");
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