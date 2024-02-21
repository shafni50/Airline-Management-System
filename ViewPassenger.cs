using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using test;

namespace FlyEasyAirlineManagement
{
    public partial class ViewPassenger : Form
    {
        readonly SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True");

        public ViewPassenger()
        {
            InitializeComponent();
        }

        private void Populate()
        {
            Con.Open();
            string query = "SELECT * FROM PassengerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            PassengerDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void ViewPassenger_Load(object sender, EventArgs e)
        {
            Populate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Back button
            AddPassenger addpas = new AddPassenger();
            addpas.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Delete button
            if (PidTb.Text == "")
            {
                MessageBox.Show("Enter the Passenger to Delete");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "DELETE FROM PassengerTbl WHERE PassId=@PassengerId";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.Parameters.AddWithValue("@PassengerId", PidTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Passenger Deleted Successfully!");
                    Con.Close();
                    Populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Data Grid
            DataGridViewRow row = PassengerDGV.Rows[e.RowIndex];
            PidTb.Text = row.Cells[0].Value.ToString();
            PnameTb.Text = row.Cells[1].Value.ToString();
            PpassTb.Text = row.Cells[2].Value.ToString();
            PaddTb.Text = row.Cells[3].Value.ToString();
            natcb.SelectedItem = row.Cells[4].Value.ToString();
            GendCb.SelectedItem = row.Cells[5].Value.ToString();
            PhoneTb.Text = row.Cells[6].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Update Button
            if (PidTb.Text == "" || PnameTb.Text == "" || PpassTb.Text == "" || PaddTb.Text == "" || PhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();

                    string query = "UPDATE PassengerTbl SET PassId=@PassId, PassName=@PassName, Passport=@Passport, PassAd=@PassAd, PassNat=@PassNat, PassGend=@PassGend, PassPhone=@PassPhone WHERE PassId=@PassId";
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@PassId", PidTb.Text);
                        cmd.Parameters.AddWithValue("@PassName", PnameTb.Text);
                        cmd.Parameters.AddWithValue("@Passport", PpassTb.Text);
                        cmd.Parameters.AddWithValue("@PassAd", PaddTb.Text);
                        cmd.Parameters.AddWithValue("@PassNat", natcb.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@PassGend", GendCb.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@PassPhone", PhoneTb.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Passenger Updated Successfully!");
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

        private void button3_Click(object sender, EventArgs e)
        {
            // Reset Button
            PidTb.Text = "";
            PnameTb.Text = "";
            PpassTb.Text = "";
            PaddTb.Text = "";
            natcb.SelectedIndex = -1;
            GendCb.SelectedIndex = -1;
            PhoneTb.Text = "";
        }
        private void DisplayData()
        {
            // Refresh The Data Grid Table Function
            SqlDataAdapter adapt = new SqlDataAdapter("SELECT * FROM PassengerTbl", Con);
            DataTable dt = new DataTable();
            adapt.Fill(dt);
            PassengerDGV.DataSource = dt;
        }
    }
}