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
    public partial class AnketSecim : Form
    {
        MySqlConnection connection = GirisYap.connection;
        public AnketSecim()
        {
            InitializeComponent();
        }

        private void AnketSecim_Load(object sender, EventArgs e)
        {
            if (GirisYap.logged == false)
            {
                var giris = new GirisYap();
                giris.FormClosed += GirisYap_Closed;
                this.Hide();
                giris.ShowDialog();
            }
            

            if (GirisYap.isAdmin)
                button2.Visible = true;

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM surveys ORDER BY id ASC", connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = reader["title"].ToString();
                    item.dbID = reader["id"].ToString();
                    comboBox1.Items.Add(item);
                }
                comboBox1.SelectedIndex = 0;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }
        }

        private void GirisYap_Closed(object sender, FormClosedEventArgs e)
        {
            if (!GirisYap.logged)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ComboboxItem selectedSurvey = (ComboboxItem)comboBox1.SelectedItem;
            int selectedID = Convert.ToInt32(selectedSurvey.dbID);

            AnketSorular anketSorular = new AnketSorular(selectedID, selectedSurvey.Text);
            this.Hide();
            anketSorular.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Anket anket = new Anket();
            this.Hide();
            anket.ShowDialog();
            this.Close();
        }
    }
}
