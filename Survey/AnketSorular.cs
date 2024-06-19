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
using System.Collections;
using MySql.Data.MySqlClient;

namespace Survey
{
    public partial class AnketSorular : Form
    {
        MySqlConnection connection = GirisYap.connection;
        int top = 20;
        List<question> questions = new List<question>();
        Dictionary<int, string> choiceIds = new Dictionary<int, string>();
        int selectedQuestion;
        string surveyTitle;

        public AnketSorular(int question, string surveyT)
        {
            InitializeComponent();
            selectedQuestion = question;
            surveyTitle = surveyT;
        }

        private void AnketSorular_Load(object sender, EventArgs e)
        {
            this.Text = surveyTitle;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM questions WHERE survey=@survey", connection);
                cmd.Parameters.AddWithValue("@survey", selectedQuestion);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    question T = new question();
                    T.dbid = Convert.ToInt32(reader["id"]);
                    T.questioncontent = reader["question"].ToString();
                    T.type = Convert.ToInt32(reader["type"]);
                    questions.Add(T);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }


            foreach (question a in questions)
            {
                Label label = new Label();
                label.Top = top;
                label.Left = 10;
                label.MaximumSize = new Size(400, 0);
                label.AutoSize = true;
                label.Font = new Font("Arial", 9, FontStyle.Bold);
                label.Text = a.questioncontent;
                container.Controls.Add(label);
                top += 30;

                try
                {
                    connection.Open();
                    MySqlCommand choicecmd = new MySqlCommand("SELECT * FROM choices WHERE question=@question", connection);
                    choicecmd.Parameters.AddWithValue("@question", a.dbid);
                    MySqlDataReader Choicereader = choicecmd.ExecuteReader();
                    while (Choicereader.Read())
                    {
                        string type = Choicereader["choice_content"].ToString();
                        if (a.type == 1)
                        {
                            choiceTxtBx txtBx = new choiceTxtBx();
                            txtBx.Top = top;
                            txtBx.dbID = Convert.ToInt32(Choicereader["id"]);
                            container.Controls.Add(txtBx);
                            top += 40;
                        }
                        else if (a.type == 2)
                        {
                            choiceTrckBr tbar = new choiceTrckBr(container, top);
                            tbar.Top = top;
                            tbar.dbID = Convert.ToInt32(Choicereader["id"]);
                            container.Controls.Add(tbar);
                            top += 60;
                        }
                        else if (a.type == 0)
                        {
                            choiceRbt rbt = new choiceRbt();
                            rbt.Top = top;
                            rbt.Text = type;
                            rbt.dbID = Convert.ToInt32(Choicereader["id"]);
                            rbt.AutoCheck = false;
                            rbt.GroupName = Choicereader["question"].ToString();
                            rbt.Click += choiceRbt_Clicked;
                            container.Controls.Add(rbt);
                            top += 30;
                        }
                        else if (a.type == 3)
                        {
                            choiceChckBx chk = new choiceChckBx();
                            chk.Top = top;
                            chk.Text = type;
                            chk.dbID = Convert.ToInt32(Choicereader["id"]);
                            container.Controls.Add(chk);
                            top += 30;
                        }
                        else if (a.type == 4)
                        {
                            bool isHere = false;
                            foreach (choiceCmbBx cmbBx in container.Controls.OfType<choiceCmbBx>())
                            {
                                if (cmbBx.dbID == a.dbid)
                                {
                                    isHere = true;
                                    ComboboxItem item = new ComboboxItem();
                                    item.Text = type;
                                    item.dbID = Convert.ToInt32(Choicereader["id"]);
                                    cmbBx.Items.Add(item);
                                    cmbBx.SelectedIndex = 0;
                                }
                                    
                            }
                            if (isHere == false)
                            {
                                choiceCmbBx cmbBx = new choiceCmbBx();
                                cmbBx.dbID = a.dbid;
                                cmbBx.Top = top;
                                container.Controls.Add(cmbBx);

                                ComboboxItem item = new ComboboxItem();
                                item.Text = type;
                                item.dbID = Convert.ToInt32(Choicereader["id"]);
                                cmbBx.Items.Add(item);

                                top += 30;
                            }
                        }
                    }
                    connection.Close();
                    top += 20;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                }
            }

            // Let's get the answers from the database
            foreach (choiceRbt rBtn in container.Controls.OfType<choiceRbt>())
            {
                try
                {
                    connection.Open();
                    MySqlCommand choicecmd = new MySqlCommand("SELECT * FROM answers WHERE account=@account AND choice=@choice", connection);
                    choicecmd.Parameters.AddWithValue("@account", GirisYap.userID);
                    choicecmd.Parameters.AddWithValue("@choice", rBtn.dbID);
                    MySqlDataReader Choicereader = choicecmd.ExecuteReader();
                    while (Choicereader.Read())
                    {
                        rBtn.Checked = Convert.ToBoolean(Convert.ToInt32(Choicereader["answer"]));
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                }
            }
            foreach (choiceTxtBx txtBx in container.Controls.OfType<choiceTxtBx>())
            {
                try
                {
                    connection.Open();
                    MySqlCommand choicecmd = new MySqlCommand("SELECT * FROM answers WHERE account=@account AND choice=@choice", connection);
                    choicecmd.Parameters.AddWithValue("@account", GirisYap.userID);
                    choicecmd.Parameters.AddWithValue("@choice", txtBx.dbID);
                    MySqlDataReader Choicereader = choicecmd.ExecuteReader();
                    while (Choicereader.Read())
                    {
                        txtBx.Text = Choicereader["answer"].ToString();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                }
            }
            foreach (choiceTrckBr trckBr in container.Controls.OfType<choiceTrckBr>())
            {
                try
                {
                    connection.Open();
                    MySqlCommand choicecmd = new MySqlCommand("SELECT * FROM answers WHERE account=@account AND choice=@choice", connection);
                    choicecmd.Parameters.AddWithValue("@account", GirisYap.userID);
                    choicecmd.Parameters.AddWithValue("@choice", trckBr.dbID);
                    MySqlDataReader Choicereader = choicecmd.ExecuteReader();
                    while (Choicereader.Read())
                    {
                        trckBr.Value = Convert.ToInt32(Choicereader["answer"]);
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                }
            }
            foreach (choiceChckBx chkBx in container.Controls.OfType<choiceChckBx>())
            {
                try
                {
                    connection.Open();
                    MySqlCommand choicecmd = new MySqlCommand("SELECT * FROM answers WHERE account=@account AND choice=@choice", connection);
                    choicecmd.Parameters.AddWithValue("@account", GirisYap.userID);
                    choicecmd.Parameters.AddWithValue("@choice", chkBx.dbID);
                    MySqlDataReader Choicereader = choicecmd.ExecuteReader();
                    while (Choicereader.Read())
                    {
                        chkBx.Checked = Convert.ToBoolean(Convert.ToInt32(Choicereader["answer"]));
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                }
            }

            // ComboBox
            foreach (choiceCmbBx cmbBx in container.Controls.OfType<choiceCmbBx>())
            {
                foreach (ComboboxItem cmbbxItem in cmbBx.Items)
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand choicecmd = new MySqlCommand("SELECT * FROM answers WHERE account=@account AND choice=@choice", connection);
                        choicecmd.Parameters.AddWithValue("@account", GirisYap.userID);
                        choicecmd.Parameters.AddWithValue("@choice", cmbbxItem.dbID);
                        MySqlDataReader Choicereader = choicecmd.ExecuteReader();
                        while (Choicereader.Read())
                        {
                            int index = cmbBx.Items.IndexOf(cmbbxItem);
                            if(Convert.ToInt32(Choicereader["answer"]) == 1)
                            {
                                cmbBx.SelectedIndex = index;
                            }
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                    }
                }
            }
            button1.Top = top;
        }

        private void button1_Click(object sender, EventArgs e)
        {


            // Let's add controls to the dictionary
            foreach (choiceRbt rBtn in container.Controls.OfType<choiceRbt>())
            {
                choiceIds[rBtn.dbID] = Convert.ToInt32(rBtn.Checked).ToString();
            }
            foreach (choiceTxtBx txtBx in container.Controls.OfType<choiceTxtBx>())
            {
                choiceIds[txtBx.dbID] = txtBx.Text;
            }
            foreach (choiceTrckBr trckBr in container.Controls.OfType<choiceTrckBr>())
            {
                choiceIds[trckBr.dbID] = (trckBr.Value).ToString();
            }
            foreach (choiceChckBx chckBx in container.Controls.OfType<choiceChckBx>())
            {
                choiceIds[chckBx.dbID] = Convert.ToInt32(chckBx.Checked).ToString();
            }
            foreach (choiceCmbBx cmbBx in container.Controls.OfType<choiceCmbBx>())
            {
                foreach (ComboboxItem cmbbxItem in cmbBx.Items)
                {
                    if(cmbBx.SelectedItem == cmbbxItem)
                    {
                        choiceIds[Convert.ToInt32(cmbbxItem.dbID)] = "1";
                    }
                    else
                    {
                        choiceIds[Convert.ToInt32(cmbbxItem.dbID)] = "0";
                    }
                    
                }
            }

            
            foreach (var a in choiceIds)
            {
                try
                {
                    connection.Open();

                    MySqlCommand deleteChoices = new MySqlCommand("DELETE FROM answers WHERE account=@account AND choice=@choice", connection);
                    deleteChoices.Parameters.AddWithValue("@account", GirisYap.userID); 
                    deleteChoices.Parameters.AddWithValue("@choice", a.Key);
                    deleteChoices.ExecuteNonQuery();

                    // Tekrar INSERT INTO yapalım
                    string command = "INSERT INTO answers(account, choice, answer) VALUES('" + GirisYap.userID + "', '" + a.Key + "', '" + a.Value + "')";

                    MySqlCommand cmd = new MySqlCommand(command, connection);
                    cmd.ExecuteNonQuery();

                    connection.Close();               
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                }
            }

            MessageBox.Show("Information saved successfully.", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        #region UTILITIES
        private void choiceRbt_Clicked(object sender, EventArgs e)
        {
            choiceRbt rb = (sender as choiceRbt);

            if (!rb.Checked)
            {
                foreach (var c in container.Controls)
                {
                    if (c is choiceRbt && (c as choiceRbt).GroupName == rb.GroupName)
                    {
                        (c as choiceRbt).Checked = false;
                    }
                }

                rb.Checked = true;
            }
        }
        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AnketSecim anketSecim = new AnketSecim();
            this.Hide();
            anketSecim.ShowDialog();
            this.Close();
        }
    }
}
