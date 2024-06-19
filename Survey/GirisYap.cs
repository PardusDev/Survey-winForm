using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Survey
{
    public partial class GirisYap : Form
    {
        public static MySqlConnection connection = new MySqlConnection("Data Source=ftp.example.one;Initial Catalog = giipjucc_survey; User ID=giipjucc_root; Password=EXAMPLE;");
        public static int userID;
        public static bool logged = false;
        public static bool isAdmin = false;

        public GirisYap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();            

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM accounts WHERE username = @username AND password = @password", connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (Convert.ToInt32(reader["admin"].ToString()) == 1)
                        isAdmin = true;
                    logged = true;
                    userID = int.Parse(reader["id"].ToString());
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username and password do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }

                connection.Close();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        // Register button
        private void button2_Click(object sender, EventArgs e)
        {
            var kyt = new KayitOl(); // Let's launch the Sign Up window
            this.Hide();
            kyt.ShowDialog();           
            kyt = null;
            this.Show();
        }
    }
}
