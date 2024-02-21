using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FlyEasyAirlineManagement
{
    public partial class Login : Form
    {
        private readonly SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True");

        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click_1(object sender, EventArgs e)
        {
            string adminId = Aid.Text;
            string password = Apass.Text;

            if (string.IsNullOrWhiteSpace(adminId) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            try
            {
                Con.Open();

                string query = "SELECT * FROM AdminsTbl WHERE AdminId = @AdminId AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, Con))
                {
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            MessageBox.Show("Login Successful!");
                            // Redirect to Dashboard
                            Dashboard dashboardForm = new Dashboard();
                            dashboardForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid credentials. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Exit Button
            Application.Exit();
        }

        private void Aid_TextChanged(object sender, EventArgs e)
        {
            // Admin text box
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // User form
            // (You can implement redirection logic here if needed)
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Register button
            AdminRegistration adminRegistrationForm = new AdminRegistration();
            adminRegistrationForm.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }
    }
}