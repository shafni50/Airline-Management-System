using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FlyEasyAirlineManagement
{
    public partial class AdminRegistration : Form
    {
        private readonly SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True");

        public AdminRegistration()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            string adminName = AdminName.Text;
            string password = Apass.Text;

            if (string.IsNullOrWhiteSpace(adminName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            try
            {
                Con.Open();

                // Get the next available AdminId
                string newAdminId = GenerateNextAdminId();

                string queryInsert = "INSERT INTO AdminsTbl (AdminId, AdminName, Password) VALUES (@AdminId, @AdminName, @Password)";
                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, Con))
                {
                    cmdInsert.Parameters.AddWithValue("@AdminId", newAdminId);
                    cmdInsert.Parameters.AddWithValue("@AdminName", adminName);
                    cmdInsert.Parameters.AddWithValue("@Password", password);

                    cmdInsert.ExecuteNonQuery();
                }

                MessageBox.Show("Registration Successful! Your Admin ID is: " + newAdminId);
                AdminName.Clear();
                Apass.Clear();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error registering admin. Please try again. Error Details: " + Ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }
        private void AdminRegistration_Load(object sender, EventArgs e)
        {

        }
        private string GenerateNextAdminId()
        {
            return string.Empty;
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            // Reset button logic
            AdminName.Clear();
            Apass.Clear();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Exit button
            Application.Exit();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            // Back button
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
    }
}