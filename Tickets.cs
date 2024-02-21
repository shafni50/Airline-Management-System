using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FlyEasyAirlineManagement
{
    public partial class Tickets : Form
    {
        private readonly SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True");

        public Tickets()
        {
            InitializeComponent();
        }

        private void FillPassengerId()
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();

                Con.Open();
                SqlCommand cmd = new SqlCommand("SELECT PassId FROM PassengerTbl", Con);
                SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("PassId", typeof(string));
                dt.Load(rdr);
                PidCb.ValueMember = "PassId";
                PidCb.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void FillFlightCode()
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();

                Con.Open();
                SqlCommand cmd = new SqlCommand("SELECT FCode FROM FlightTbl", Con);
                SqlDataReader rdr;
                rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("FCode", typeof(string));
                dt.Load(rdr);
                FCode.ValueMember = "FCode";
                FCode.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        string pname, ppass, pnat;

        private void FetchPassenger()
        {
            Con.Open();
            string query = "SELECT * FROM PassengerTbl WHERE PassId=@PassengerId";
            using (SqlCommand cmd = new SqlCommand(query, Con))
            {
                cmd.Parameters.AddWithValue("@PassengerId", PidCb.SelectedValue.ToString());
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    pname = dr["PassName"].ToString();
                    ppass = dr["Passport"].ToString();
                    pnat = dr["PassNat"].ToString();

                    PNameTb.Text = pname;
                    PPassTb.Text = ppass;
                    PNatTb.Text = pnat;
                }
            }
            Con.Close();
        }

        private void PidCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FetchPassenger();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Book Ticket
            if (string.IsNullOrEmpty(Tid.Text) || FCode.Text == "" || PidCb.SelectedValue == null || PNameTb.Text == "" || PPassTb.Text == "" || PNatTb.Text == "" || AmtCb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    if (!int.TryParse(Tid.Text, out _))
                    {
                        MessageBox.Show("Invalid Ticket ID. Please enter a valid positive integer.");
                        return;
                    }

                    using (SqlConnection Con = new SqlConnection(@"Data Source=SHA-PC\SQLEXPRESS;Initial Catalog=AirlineDb;Integrated Security=True"))
                    {
                        Con.Open();

                        string query = "INSERT INTO TicketTbl VALUES(@Tid, @Fcode, @Pid, @PName, @PPass, @PNation, @Amt)";
                        using (SqlCommand cmd = new SqlCommand(query, Con))
                        {
                            cmd.Parameters.AddWithValue("@Tid", int.Parse(Tid.Text));
                            cmd.Parameters.AddWithValue("@Fcode", FCode.Text);
                            cmd.Parameters.AddWithValue("@Pid", PidCb.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@PName", PNameTb.Text);
                            cmd.Parameters.AddWithValue("@PPass", PPassTb.Text);
                            cmd.Parameters.AddWithValue("@PNation", PNatTb.Text);
                            cmd.Parameters.AddWithValue("@Amt", AmtCb.Text);

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Ticket Booked Successfully!");
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
            Populate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Reset Button
            Tid.Text = "";
            FCode.SelectedIndex = -1;
            PidCb.SelectedIndex = -1;
            PNameTb.Text = "";
            PPassTb.Text = "";
            PNatTb.Text = "";
            AmtCb.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Cancel a Ticket
            if (TicketDGV.CurrentRow != null)
            {
                try
                {
                    Con.Open();
                    string ticketId = TicketDGV.CurrentRow.Cells["Tid"].Value.ToString();
                    string deleteQuery = "DELETE FROM TicketTbl WHERE Tid = @TicketId";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, Con))
                    {
                        cmd.Parameters.AddWithValue("@TicketId", ticketId);
                        cmd.ExecuteNonQuery();
                    }

                    // After canceling, refresh the DataGridView
                    Populate();

                    MessageBox.Show("Ticket canceled successfully!");
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
            else
            {
                MessageBox.Show("Please select a ticket to cancel.");
            }
            Populate();
        }

        private void TicketDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Data grid view
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = TicketDGV.Rows[e.RowIndex];

                Tid.Text = row.Cells["Tid"].Value.ToString();
                FCode.Text = row.Cells["Fcode"].Value.ToString();
                PidCb.SelectedValue = row.Cells["Pid"].Value.ToString();
                PNameTb.Text = row.Cells["PName"].Value.ToString();
                PPassTb.Text = row.Cells["PPass"].Value.ToString();
                PNatTb.Text = row.Cells["PNation"].Value.ToString();
                AmtCb.Text = row.Cells["Amt"].Value.ToString();
            }
        }

        private void Tickets_Load(object sender, EventArgs e)
        {
            FillPassengerId();
            FillFlightCode();
            Populate();
        }

        private void Populate()
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM TicketTbl";
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                TicketDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Back To Dasboard
            Dashboard addDas = new Dashboard();
            addDas.Show();
            this.Hide();
        }
    }
}