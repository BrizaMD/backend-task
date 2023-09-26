using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruckOrderOptimizer.Models;
using TruckOrderOptimizer.Services;

namespace TruckOrderOptimizer.Tests
{
    [TestClass]
    public class JobAssignmentServiceTests
    {
        [TestMethod]
        public void AssignJobs_ShouldAssignCompatibleJobsAndVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, CompatibleJobs = new HashSet<string> { "A" } },
                new Vehicle { Id = 2, CompatibleJobs = new HashSet<string> { "B" } }
            };
            var jobs = new List<Job>
            {
                new Job { Id = 1, Type = "A" },
                new Job { Id = 2, Type = "B" }
            };

            // Act
            var assignments = JobAssignmentService.AssignJobs(vehicles, jobs);

            // Assert
            Assert.AreEqual(2, assignments.Count);
            Assert.IsTrue(assignments.ContainsKey(1));
            Assert.AreEqual(1, assignments[1]);
            Assert.IsTrue(assignments.ContainsKey(2));
            Assert.AreEqual(2, assignments[2]);
        }

        [TestMethod]
        public void AssignJobs_ShouldNotAssignIncompatibleJobsAndVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, CompatibleJobs = new HashSet<string> { "A" } }
            };
            var jobs = new List<Job>
            {
                new Job { Id = 1, Type = "B" }
            };

            // Act
            var assignments = JobAssignmentService.AssignJobs(vehicles, jobs);

            // Assert
            Assert.AreEqual(0, assignments.Count);
        }

        [TestMethod]
        public void AssignJobs_ShouldHandleEmptyLists()
        {
            // Arrange
            var vehicles = new List<Vehicle>();
            var jobs = new List<Job>();

            // Act
            var assignments = JobAssignmentService.AssignJobs(vehicles, jobs);

            // Assert
            Assert.AreEqual(0, assignments.Count);
        }
    }
}