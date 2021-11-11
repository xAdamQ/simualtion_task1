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
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();
        }

        public void simulate()
        {
            setCummProp(InterarrivalDistribution);
            foreach (var server in Servers)
                setCummProp(server.TimeDistribution);

            var customerN = 0;
            var clock = 0;

            while (!shouldStop())
            {
                if(customerN == 5)
                { 
                }
                var sCase = new SimulationCase();
                SimulationTable.Add(sCase);

                sCase.CustomerNumber = customerN++;
                var prop = 0;
                sCase.InterArrival = customerN==1?0: getRandomTime(InterarrivalDistribution, out prop);
                sCase.RandomInterArrival =customerN==1?1: prop;

                clock += sCase.InterArrival;
                
                sCase.ArrivalTime = clock;

                var chosenServer = getServer();
                if (chosenServer.FinishTime > clock)
                {
                    sCase.TimeInQueue = chosenServer.FinishTime - clock;
                }

                sCase.ServiceTime = getRandomTime(chosenServer.TimeDistribution, out int prop2);
                sCase.AssignedServer = chosenServer;
                sCase.RandomService = prop2;

                sCase.StartTime = sCase.TimeInQueue ==0? clock:chosenServer.FinishTime;
                sCase.EndTime = sCase.StartTime + sCase.ServiceTime;

                chosenServer.FinishTime += sCase.EndTime;
            }

            bool shouldStop()
            {
                if (StoppingCriteria == Enums.StoppingCriteria.NumberOfCustomers)
                    return customerN >= StoppingNumber;
                else
                    return clock >= StoppingNumber;
            }

            Server getServer()
            {
                switch (SelectionMethod)
                {
                    case Enums.SelectionMethod.HighestPriority:
                        var freeServers = Servers.Where(s => s.FinishTime <= clock).ToList();
                        if (freeServers.Count != 0) return freeServers.OrderBy(s => s.ID).First();
                        return Servers.OrderBy(s => s.FinishTime).First();
                    case Enums.SelectionMethod.Random:
                        return Servers[new Random().Next(Servers.Count)];
                    case Enums.SelectionMethod.LeastUtilization:
                        throw new NotImplementedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        private void setCummProp(List<TimeDistribution> timeDistributions)
        {
            decimal cummProp = 0;
            foreach (var t in timeDistributions)
            {
                cummProp += t.Probability;
                t.CummProbability = cummProp;
            }
        }

        private Random rand = new Random();
        private int getRandomTime(List<TimeDistribution> timeDistributions, out int prop)
        {
            prop = GetRandomNumber();
            for (int i = 0; i < timeDistributions.Count; i++)
            {
                if (prop<=timeDistributions[i].CummProbability*100)
                    return timeDistributions[i].Time;
            }
            throw new Exception("time dist is wrong");
        }

        public int GetRandomNumber()
        {
            return rand.Next(1, 101);
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