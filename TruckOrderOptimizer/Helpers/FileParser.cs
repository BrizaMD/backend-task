using System.Collections.Generic;
using System.IO;
using TruckOrderOptimizer.Models;

namespace TruckOrderOptimizer.Helpers
{
    public static class FileParser
    {
        public static void ParseTrucksAndJobs(string filepath, out List<Vehicle> vehicles, out List<Job> jobs)
        {
            vehicles = new List<Vehicle>();
            jobs = new List<Job>();

            using (var reader = new StreamReader(filepath))
            {
                int vehicleCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < vehicleCount; i++)
                {
                    string[] tokens = reader.ReadLine().Split(' ');
                    int vehicleId = int.Parse(tokens[0]);
                    var compatibleJobs = new List<string>(tokens).GetRange(1, tokens.Length - 1);
                    var compatibleJobs = new HashSet<string>(tokens.Skip(1));
                    vehicles.Add(new Vehicle { Id = vehicleId, CompatibleJobs = compatibleJobs });
                }

                int jobCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < jobCount; i++)
                {
                    string[] tokens = reader.ReadLine().Split(' ');
                    int jobId = int.Parse(tokens[0]);
                    string jobType = tokens[1];
                    jobs.Add(new Job { Id = jobId, Type = jobType });
                }
            }
        }
    }
}