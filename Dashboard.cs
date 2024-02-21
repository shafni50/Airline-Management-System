using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test;

namespace FlyEasyAirlineManagement
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // View AddPassengers Form
            AddPassenger addpas = new AddPassenger();
            addpas.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // View ViewFlight Form 
            FlightTbl addFli = new FlightTbl();
            addFli.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // View Ticket Form
            Tickets addTic = new Tickets();
            addTic.Show();
            this.Hide();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void AdminLogout_Click(object sender, EventArgs e)
        {
            // Admin Logout
            Login addLog = new Login();
            addLog.Show();
            this.Hide();
        }
    }
}
