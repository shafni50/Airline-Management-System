using FlyEasyAirlineManagement;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace test
{
    public partial class AddPassenger : Form
    {
        readonly SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True");

        public AddPassenger()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Add any event handling logic if needed
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Add any event handling logic if needed
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Back button click event
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // View Passenger
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Add any event handling logic if needed
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Add any event handling logic if needed
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Record button 
            if (PassId.Text == "" || PassName.Text == "" || PassportTb.Text == "" || PassAd.Text == "" || PhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    // Check if the Passenger ID is already in use
                    string checkQuery = "SELECT COUNT(*) FROM PassengerTbl WHERE PassId = @PassId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("@PassId", PassId.Text);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Passenger ID already exists. Please use a different ID.");
                            return;
                        }
                    }

                    string query = "INSERT INTO PassengerTbl VALUES(@PassId, @PassName, @Passport, @PassAd, @PassNat, @PassGend, @PassPhone)";
                    using (SqlCommand cmd = new SqlCommand(query, Con))
                    {
                        cmd.Parameters.AddWithValue("@PassId", PassId.Text);
                        cmd.Parameters.AddWithValue("@PassName", PassName.Text);
                        cmd.Parameters.AddWithValue("@Passport", PassportTb.Text);
                        cmd.Parameters.AddWithValue("@PassAd", PassAd.Text);
                        cmd.Parameters.AddWithValue("@PassNat", NationalityCb.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@PassGend", GenderCb.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@PassPhone", PhoneTb.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Passenger Recorded Successfully!");
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Reset button click event
            PassId.Text = "";
            PassName.Text = "";
            PassportTb.Text = "";
            PassAd.Text = "";
            PhoneTb.Text = "";
            NationalityCb.SelectedIndex = -1;
            GenderCb.SelectedIndex = -1;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Back to Dashboard button
            Dashboard viewdas = new Dashboard();
            viewdas.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // View passenger button
            ViewPassenger viewpas = new ViewPassenger();
            viewpas.Show();
            this.Hide();
        }

        private void AddPassenger_Load(object sender, EventArgs e)
        {
            // Load Form
        }
    }
}
