using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using System.Linq;
using TruckOrderOptimizer.Models;

namespace TruckOrderOptimizer.Helpers
{
    public static class FileParser
    {
        public static (List<Vehicle> Vehicles, List<Job> Jobs) ParseTrucksAndJobs(string filepath)
        {
            var vehicles = new List<Vehicle>();
            var jobs = new List<Job>();

            try
            {
                using var reader = new StreamReader(filepath);

                int vehicleCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < vehicleCount; i++)
                {
                    string[] tokens = reader.ReadLine().Split(' ');
                    int vehicleId = int.Parse(tokens[0]);
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

                return (vehicles, jobs);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error("File not found: {ExFileName}", ex.FileName);
                throw;
            }
            catch (FormatException ex)
            {
                Log.Error("Invalid file format: {ExMessage}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred: {ExMessage}", ex.Message);
                throw;
            }
        }
    }
}