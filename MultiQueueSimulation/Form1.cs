using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;

namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        public Form1(SimulationSystem SimulationSystemobj)
        {
            InitializeComponent();

            for (int i = 0; i < SimulationSystemobj.SimulationTable.Count(); i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = SimulationSystemobj.SimulationTable[i].CustomerNumber;
                dataGridView1.Rows[i].Cells[1].Value = SimulationSystemobj.SimulationTable[i].RandomInterArrival;
                dataGridView1.Rows[i].Cells[2].Value = SimulationSystemobj.SimulationTable[i].InterArrival;
                dataGridView1.Rows[i].Cells[3].Value = SimulationSystemobj.SimulationTable[i].ArrivalTime;
                dataGridView1.Rows[i].Cells[4].Value = SimulationSystemobj.SimulationTable[i].RandomService;
                dataGridView1.Rows[i].Cells[5].Value = SimulationSystemobj.SimulationTable[i].AssignedServer.ID;
                dataGridView1.Rows[i].Cells[6].Value = SimulationSystemobj.SimulationTable[i].StartTime;
                dataGridView1.Rows[i].Cells[7].Value = SimulationSystemobj.SimulationTable[i].ServiceTime;
                dataGridView1.Rows[i].Cells[8].Value = SimulationSystemobj.SimulationTable[i].EndTime;
                dataGridView1.Rows[i].Cells[9].Value = SimulationSystemobj.SimulationTable[i].TimeInQueue;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
