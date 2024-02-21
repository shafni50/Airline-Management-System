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
    public partial class ViewFlights : Form
    {
        public ViewFlights()
        {
            InitializeComponent();
        }

        readonly SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True");
       
        private void Populate()
        {
            Con.Open();
            string query = "SELECT * FROM FlightTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            FlightsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Back button
            FlightTbl viewFliT = new FlightTbl();
            viewFliT.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Reset button
            FcodeTb.Text = "";
            Fsrc.SelectedIndex = -1;
            FDest.SelectedIndex = -1;
            FDate.Value = DateTime.Now;
            SeatNum.Text = "";
        }

        private void ViewFlights_Load(object sender, EventArgs e)
        {
            Populate();
        }

        private void FlightsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Flight Data Grid View
            DataGridViewRow row = FlightsDGV.Rows[e.RowIndex];
            FcodeTb.Text = row.Cells[0].Value.ToString();
            Fsrc.Text = row.Cells[1].Value.ToString();
            FDest.Text = row.Cells[2].Value.ToString();
            if (row.Cells[3].Value != null && DateTime.TryParse(row.Cells[3].Value.ToString(), out DateTime flightDate))
            {
                FDate.Value = flightDate;
            }
            SeatNum.Text = row.Cells[4].Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Update Button
            if (FcodeTb.Text == "" || Fsrc.Text == "" || FDest.Text == "" || SeatNum.Text == "" || FDate.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();

                    string query = "UPDATE FlightTbl SET Fcode=@Fcode, Fsrc=@Fsrc, FDest=@FDest, Fseats=@Fseats, FDate=@FDate WHERE Fcode=@Fcode";
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@Fcode", FcodeTb.Text);
                        cmd.Parameters.AddWithValue("@Fsrc", Fsrc.Text);
                        cmd.Parameters.AddWithValue("@FDest", FDest.Text);
                        cmd.Parameters.AddWithValue("@Fseats", SeatNum.Text);
                        cmd.Parameters.AddWithValue("@FDate", FDate.Value);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Flight Updated Successfully!");
                    DisplayData();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Delete Button
            if (FcodeTb.Text == "")
            {
                MessageBox.Show("Enter the Flight Code to Delete");
            }
            else
            {
                try
                {
                    Con.Open();

                    string query = "DELETE FROM FlightTbl WHERE Fcode=@Fcode";
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@Fcode", FcodeTb.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Flight Deleted Successfully!");
                        DisplayData();
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                finally
                {
                    Con.Close();
                }
            }
        }


        private void DisplayData()
        {
            // Refresh The Data Grid Table Function
            SqlDataAdapter adapt = new SqlDataAdapter("SELECT * FROM FlightTbl", Con);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            FlightsDGV.DataSource = dt;
        }
    }
}
