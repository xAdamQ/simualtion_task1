using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class Server
    {
        static int lastId;
        public Server()
        {
            this.TimeDistribution = new List<TimeDistribution>();
            ID = ++lastId;
        }

        public int ID { get; set; }
        public decimal IdleProbability { get; set; }
        public decimal AverageServiceTime { get; set; }
        public decimal Utilization { get; set; }

        public List<TimeDistribution> TimeDistribution;

        public int serviceTime;

        public int timeServiceEnd;


        //optional if needed use them
        public int FinishTime { get; set; }
        public int TotalWorkingTime { get; set; }
    }
}