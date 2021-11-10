using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueTesting;
using MultiQueueModels;
using System.IO;

namespace MultiQueueSimulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var system = new SimulationSystem();

            var testCaseText = File.ReadAllText(@"../../TestCases/TestCase1.txt");
            var testCaseLines = testCaseText.Split(new string[] {Environment.NewLine}, StringSplitOptions.None)
                .Select(s => s.Replace("\r", "")).ToArray();
            system.NumberOfServers = int.Parse(testCaseLines[Array.IndexOf(testCaseLines, "NumberOfServers") + 1]);
            system.StoppingNumber = int.Parse(testCaseLines[Array.IndexOf(testCaseLines, "StoppingNumber") + 1]);
            system.StoppingCriteria =
                (Enums.StoppingCriteria) int.Parse(testCaseLines[Array.IndexOf(testCaseLines, "StoppingCriteria") + 1]);
            system.SelectionMethod = (Enums.SelectionMethod) int.Parse(testCaseLines[Array.IndexOf(testCaseLines, "SelectionMethod") + 1]);
            var counter = Array.IndexOf(testCaseLines, "InterarrivalDistribution") + 1;
            while (!string.IsNullOrWhiteSpace(testCaseLines[counter]))
            {
                var vals = testCaseLines[counter].Split(',');
                var time = int.Parse(vals[0]);
                var prob = decimal.Parse(vals[1]);
                system.InterarrivalDistribution.Add(new TimeDistribution
                {
                    Time = time,
                    Probability = prob,
                });
                counter++;
            }

            counter = Array.IndexOf(testCaseLines, "ServiceDistribution_Server1") + 1;
            system.Servers.Add(new Server());
            while (counter < testCaseLines.Length)
            {
                if (string.IsNullOrWhiteSpace(testCaseLines[counter]))
                {
                    counter += 2;
                    system.Servers.Add(new Server());
                    continue;
                }

                var vals = testCaseLines[counter].Split(',');
                var time = int.Parse(vals[0]);
                var prob = decimal.Parse(vals[1]);
                system.Servers.Last().TimeDistribution.Add(new TimeDistribution
                {
                    Time = time,
                    Probability = prob,
                });

                counter++;
            }

            system.simulate();

            string result = TestingManager.Test(system, Constants.FileNames.TestCase1);
            Console.WriteLine(result);
            MessageBox.Show(result);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}