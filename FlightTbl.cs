using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FlyEasyAirlineManagement
{
    public partial class FlightTbl : Form
    {
        readonly SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True");

        public FlightTbl()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            // Back to Dashboard button
            Dashboard viewdas = new Dashboard();
            viewdas.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Record Button
            if (FcodeTb.Text == "" || Fsrc.Text == "" || FDest.Text == "" || SeatNum.Text == "" || FDate.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    // Check if the Flight Code is already in use
                    string checkQuery = "SELECT COUNT(*) FROM FlightTbl WHERE Fcode = @Fcode";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("@Fcode", FcodeTb.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Flight Code already exists. Please use a different Code.");
                            return;
                        }
                    }

                    string query = "INSERT INTO FlightTbl VALUES(@Fcode, @Fsrc, @FDest, @FDate, @FSeats)";
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@Fcode", FcodeTb.Text);
                        cmd.Parameters.AddWithValue("@Fsrc", Fsrc.Text);
                        cmd.Parameters.AddWithValue("@FDest", FDest.Text);
                        cmd.Parameters.AddWithValue("@FDate", FDate.Value);
                        cmd.Parameters.AddWithValue("@FSeats", SeatNum.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Flight Recorded Successfully!");
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Error: " + Ex.Message);
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Back to Dashboard button
            Dashboard viewdas = new Dashboard();
            viewdas.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Reset button click event
            FcodeTb.Text = "";
            Fsrc.SelectedIndex = -1;
            FDest.SelectedIndex = -1;
            FDate.Value = DateTime.Now;
            SeatNum.Text = "";
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // View Flights button click event
            ViewFlights viewFlights = new ViewFlights();
            viewFlights.Show();
            this.Hide();
        }

        private void FlightTbl_Load(object sender, EventArgs e)
        {

        }
    }
}