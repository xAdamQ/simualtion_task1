using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MultiQueueModels;

namespace MultiQueueModels
{
    public class SimulationSystem
    {
        private int customerN, time;
        private int customerQueue;

        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();
        }

        
        
        void simulate()
        {
            SetCummProp(InterarrivalDistribution);
            foreach (var server in Servers)
                SetCummProp(server.TimeDistribution);

            while (!shouldStop())
            {
                var enterArrivalTime = GetRandomTime(InterarrivalDistribution);
                arrival();
                
            }
        }

        void arrival()
        {
            var chosenServer = Servers[0].isIdle ? Servers[0] : Servers[1];
            if (!chosenServer.isIdle)
            {
                customerQueue++;
                return;
            }
            var serviceTime = GetRandomTime(chosenServer.TimeDistribution);

            chosenServer.serviceTime += serviceTime;
        }
        void depature()
        {
            
        }

        bool shouldStop()
        {
            if (StoppingCriteria == Enums.StoppingCriteria.NumberOfCustomers)
                return customerN >= StoppingNumber;
            else
                return time >= StoppingNumber;
        }

        public void SetCummProp(List<TimeDistribution> timeDistributions)
        {
            decimal cummProp = 0;
            for (int i = 0; i < timeDistributions.Count; i++)
            {
                cummProp += timeDistributions[i].Probability;
                timeDistributions[i].CummProbability = cummProp;
            }
        }

        private Random rand;
        public int GetRandomTime(List<TimeDistribution> timeDistributions)
        {
            var prop = rand.NextDouble();
            for (int i = 0; i < timeDistributions.Count; i++)
            {
                if (timeDistributions[i].CummProbability <= (decimal) prop)
                    return timeDistributions[i].Time;
            }
            throw new Exception("time dist is wrong");
        }

        ///////////// INPUTS ///////////// 
        public int NumberOfServers { get; set; }
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

    }
}
