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
    public partial class KayitOl : Form
    {
        MySqlConnection connection = GirisYap.connection;
        public KayitOl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            string surname = textBox2.Text.Trim();
            string username = textBox3.Text.Trim();
            string password = textBox4.Text.Trim();
            bool isThere = false;

            if (!(name.Length <=26 && surname.Length <= 26 && username.Length <=16 && password.Length <= 16)){
                MessageBox.Show("You have exceeded the maximum character limit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                #region LET'S CHECK IF THE USERNAME IS ALREADY TAKEN
                connection.Open();
                MySqlCommand cmdSelect = new MySqlCommand("SELECT * FROM accounts WHERE username = @username", connection);
                cmdSelect.Parameters.AddWithValue("@username", username);
                MySqlDataReader reader = cmdSelect.ExecuteReader();

                if (reader.Read())
                {
                    isThere = true; // If registered, assign the value true to this variable.
                    MessageBox.Show("There is another account with the same username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    isThere = false;
                }
                connection.Close();
                #endregion

                #region LET'S PERFORM THE REGISTRATION PROCESS
                if (!isThere){
                    connection.Open();

                    string command = "INSERT INTO accounts(username, password, name, surname) VALUES(@username, @password, @name, @surname)";
                    MySqlCommand cmd = new MySqlCommand(command, connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@surname", surname);
                    cmd.ExecuteNonQuery();            
                    
                    MessageBox.Show("You have successfully registered. Please log in..", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    connection.Close();
                    this.Close();
                }
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
