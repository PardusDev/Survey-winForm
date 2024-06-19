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
    public partial class Anket : Form
    {
        MySqlConnection connection = GirisYap.connection;
        bool check;

        int selected_survey;
        int selected_question;
        int selected_choice;

        public Anket()
        {
            InitializeComponent();
        }

        private void Anket_Load(object sender, EventArgs e)
        {
            #region LET'S REFILL THE SURVEYS DATAGRIDVIEW
            refreshAnketler();
            #endregion
        }

        private void GirisYap_Closed(object sender, FormClosedEventArgs e)
        {
            if (!GirisYap.logged)
            {
                this.Close();
            }
        }

        #region BUTTONS
        private void button1_Click(object sender, EventArgs e)
        { // Add New Survey
            if(textBox1.TextLength<=4)
            {
                MessageBox.Show("The survey title should be more descriptive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                connection.Open();

                string getTitleText = textBox1.Text;

                string command = "INSERT INTO surveys(title) VALUES(@titleText)";
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.Parameters.AddWithValue("@titleText", getTitleText);
                cmd.ExecuteNonQuery();

                connection.Close();

                refreshAnketler();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        { // Choose Survey
            var selectedRow = dataGridView3.CurrentRow;
            if (selectedRow == null)
                return;
            object selectedindex = selectedRow.Cells[0].Value;
            object selectedItem = selectedRow.Cells[1].Value;

            selected_question = Convert.ToInt32(selectedindex);
            selected_survey = Convert.ToInt32(selectedindex);
            selected_question = 0;
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            textBox2.Text = selectedItem.ToString();

            refreshSorular();
            textBox3.Text = "";

            groupBox3.Visible = false;

            panel1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        { // Change Survey Title
            if (textBox2.TextLength <= 1)
            {
                MessageBox.Show("The survey title should be more descriptive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                connection.Open();
                string updateCommand = "UPDATE surveys SET title=@title WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(updateCommand, connection);

                string getTitleText = textBox2.Text;
                cmd.Parameters.AddWithValue("@title", getTitleText);

                cmd.Parameters.AddWithValue("@id", selected_survey);

                cmd.ExecuteNonQuery();

                connection.Close();

                refreshAnketler();
                MessageBox.Show("Information saved successfully.", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool isSelected = radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton7.Checked || radioButton9.Checked;
            if ((!isSelected) || textBox3.TextLength <= 0)
            {
                MessageBox.Show("Question type not selected or question is not long enough.", "Errır", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            int selected_type = 0;
            if (radioButton1.Checked)
                selected_type = 0;
            else if (radioButton2.Checked)
                selected_type = 1;
            else if (radioButton3.Checked)
                selected_type = 2;
            else if (radioButton7.Checked)
                selected_type = 3;
            else if (radioButton9.Checked)
                selected_type = 4;
                
            try
            {
                connection.Open();

                string getQuestionText = textBox3.Text;

                int placement = dataGridView2.RowCount;
                string command = "INSERT INTO questions(survey, placement, question, type) VALUES('"+ selected_survey +"', '"+ placement + "', @questionText, '"+ selected_type + "'); SELECT LAST_INSERT_ID();";
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.Parameters.AddWithValue("@questionText", getQuestionText);
                cmd.CommandType = CommandType.Text;

                selected_question = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();

                refreshSorular();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }
            insertChoice(selected_type);
            groupBox3.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        { // Choose question
            var selectedRow = dataGridView2.CurrentRow;
            if (selectedRow == null)
                return;
            object selectedindex = selectedRow.Cells[0].Value;
            object selectedItem = selectedRow.Cells[1].Value;

            selected_question = Convert.ToInt32(selectedindex);
            groupBox3.Visible = true;
            panel1.Visible = false;
            textBox5.Clear();

            refreshChoices();

            textBox4.Text = selectedItem.ToString();
            int question_type = 0;
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT type FROM questions WHERE id = @id", connection);
                cmd.Parameters.AddWithValue("@id", selected_question);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    question_type = Convert.ToInt32(reader["type"]);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }

            switch (question_type)
            {
                case 0:
                    radioButton4.Checked = true;
                    break;
                case 1:
                    radioButton5.Checked = true;
                    break;
                case 2:
                    radioButton6.Checked = true;
                    break;
                case 3:
                    radioButton8.Checked = true;
                    break;
                case 4:
                    radioButton10.Checked = true;
                    break;
                default:
                    break;
            }
            setRadioPanelVisible(question_type);
        }

        private void button6_Click(object sender, EventArgs e)
        { // Change question content
            panel1.Visible = false;
            if (textBox4.TextLength <= 0)
            {
                MessageBox.Show("The question content should be more descriptive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            } 
            try
            {
                connection.Open();
                string updateCommand = "UPDATE questions SET question=@content WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(updateCommand, connection);

                string getQuestionText = textBox4.Text;
                cmd.Parameters.AddWithValue("@content", getQuestionText);
                cmd.Parameters.AddWithValue("@id", selected_question);

                cmd.ExecuteNonQuery();

                connection.Close();

                refreshSorular();
                MessageBox.Show("Information saved successfully.", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        { // Add new choice
            if (textBox5.TextLength <= 0)
            {
                MessageBox.Show("The option content should be more descriptive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                connection.Open();

                string getChoiceText = textBox5.Text;

                int placement = dataGridView1.RowCount;
                string command = "INSERT INTO choices(question, choice_content, placement) VALUES('" + selected_question + "', @choiceText, '" + placement +"')";
                MySqlCommand cmd = new MySqlCommand(command, connection);
                cmd.Parameters.AddWithValue("@choiceText", getChoiceText);

                cmd.ExecuteNonQuery();

                connection.Close();

                refreshChoices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox6.TextLength <= 0)
            {
                MessageBox.Show("The option content should be more descriptive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            try
            {
                connection.Open();
                string updateCommand = "UPDATE choices SET choice_content=@content WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(updateCommand, connection);

                cmd.Parameters.AddWithValue("@content", textBox6.Text);

                cmd.Parameters.AddWithValue("@id", selected_choice);

                cmd.ExecuteNonQuery();

                connection.Close();

                refreshChoices();
                MessageBox.Show("Information saved successfully.", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        { // Decrease the placements one by one when deleted
            try
            {
                connection.Open();
                string deleteAnswers = "DELETE FROM answers WHERE choice = @choice";
                MySqlCommand answersCmd = new MySqlCommand(deleteAnswers, connection);
                answersCmd.Parameters.AddWithValue("@choice", selected_choice);
                answersCmd.ExecuteNonQuery();


                string deleteCommand = "DELETE FROM choices WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(deleteCommand, connection);
                cmd.Parameters.AddWithValue("@id", selected_choice);

                cmd.ExecuteNonQuery();

                

                connection.Close();

                refreshChoices();
                MessageBox.Show("The option was deleted successfully.", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                choiceResort();
                refreshChoices();
                panel1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        { // Change question type.
            int selected_type = 0;
            if (radioButton4.Checked)
                selected_type = 0;
            else if (radioButton5.Checked)
                selected_type = 1;
            else if (radioButton6.Checked)
                selected_type = 2;
            else if (radioButton8.Checked)
                selected_type = 3;
            else if (radioButton10.Checked)
                selected_type = 4;
            List<int> choiceIds = new List<int>();
            // Let's get choice ids
            try
            {
                connection.Open();
                string selectChoices = "SELECT id FROM choices WHERE question=@question";
                MySqlCommand selectChoiceCmd = new MySqlCommand(selectChoices, connection);
                selectChoiceCmd.Parameters.AddWithValue("@question", selected_question);
                MySqlDataReader reader = selectChoiceCmd.ExecuteReader();

                while (reader.Read())
                {
                    choiceIds.Add(Convert.ToInt32(reader["id"]));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            try
            {
                connection.Open();
                foreach (int a in choiceIds)
                {
                    string deleteAnswers = "DELETE FROM answers WHERE choice = @choice";
                    MySqlCommand answersCmd = new MySqlCommand(deleteAnswers, connection);
                    answersCmd.Parameters.AddWithValue("@choice", a);
                    answersCmd.ExecuteNonQuery();
                }

                string updateCommand = "UPDATE questions SET type=@type WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(updateCommand, connection);

                cmd.Parameters.AddWithValue("@type", selected_type);
                cmd.Parameters.AddWithValue("@id", selected_question);

                cmd.ExecuteNonQuery();

                string deleteCommand = "DELETE FROM choices WHERE question=@question";
                MySqlCommand deleteCmd = new MySqlCommand(deleteCommand, connection);
                deleteCmd.Parameters.AddWithValue("@question", selected_question);

                deleteCmd.ExecuteNonQuery();
                
                connection.Close();

                refreshChoices();
                MessageBox.Show("Information saved successfully.", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            setRadioPanelVisible(selected_type);
            insertChoice(selected_type);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<int> choiceIds = new List<int>();
            try
            {
                connection.Open();
                string selectChoices = "SELECT id FROM choices WHERE question=@question";
                MySqlCommand selectChoiceCmd = new MySqlCommand(selectChoices, connection);
                selectChoiceCmd.Parameters.AddWithValue("@question", selected_question);
                MySqlDataReader reader = selectChoiceCmd.ExecuteReader();

                while (reader.Read())
                {
                    choiceIds.Add(Convert.ToInt32(reader["id"]));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            try
            {
                connection.Open();
                foreach (int a in choiceIds)
                {
                    string deleteAnswers = "DELETE FROM answers WHERE choice = @choice";
                    MySqlCommand answersCmd = new MySqlCommand(deleteAnswers, connection);
                    answersCmd.Parameters.AddWithValue("@choice", a);
                    answersCmd.ExecuteNonQuery();
                }
                


                string deleteChoices = "DELETE FROM choices WHERE question = @question";
                MySqlCommand choiceCmd = new MySqlCommand(deleteChoices, connection);
                choiceCmd.Parameters.AddWithValue("@question", selected_question);

                choiceCmd.ExecuteNonQuery();

                string deleteQuestion = "DELETE FROM questions WHERE id = @id";
                MySqlCommand deleteQuestionCmd = new MySqlCommand(deleteQuestion, connection);
                deleteQuestionCmd.Parameters.AddWithValue("@id", selected_question);

                deleteQuestionCmd.ExecuteNonQuery();



                connection.Close();

                MessageBox.Show("The question was deleted successfully.", "Operation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refreshSorular();
                groupBox3.Visible = false;
                panel1.Visible = false;
                questionResort();
                refreshSorular();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        #endregion

        #region EVENTS
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // We will hide all other panels.
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        { // When the selection of options changes:
            object selectedindex = dataGridView1.CurrentRow.Cells[0].Value;
            object selectedItem = dataGridView1.CurrentRow.Cells[1].Value;
            selected_choice = Convert.ToInt32(selectedindex);
            string selected_item = selectedItem.ToString();

            textBox6.Text = selected_item;
            panel1.Visible = true;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AnketSecim anketSecim = new AnketSecim();
            this.Hide();
            anketSecim.ShowDialog();
            this.Close();
        }
        #endregion

        #region UTILITIES
        private void refreshAnketler()
        {
            dataGridView3.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM surveys", connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView3.Rows.Add(reader["id"].ToString(), reader["title"].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }
        }

        private void refreshSorular()
        {
            dataGridView2.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM questions WHERE survey = @surveyid ORDER BY placement ASC", connection);
                cmd.Parameters.AddWithValue("@surveyid", selected_survey);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView2.Rows.Add(reader["id"].ToString(), reader["question"].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }
        }

        private void refreshChoices()
        {
            dataGridView1.Rows.Clear();
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM choices WHERE question = @question ORDER BY placement ASC", connection);
                cmd.Parameters.AddWithValue("@question", selected_question);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader["id"].ToString(), reader["choice_content"].ToString());
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
            }
        }

        private void setRadioPanelVisible(int type)
        {
            if (type == 0)
                radioPanel.Visible = true;
            else if (type == 3)
                radioPanel.Visible = true;
            else if (type == 4)
                radioPanel.Visible = true;
            else if (type == 1)
                radioPanel.Visible = false;
            else if (type == 2)
                radioPanel.Visible = false;
        }

        private void insertChoice(int type)
        {

            if (type == 1 || type == 2)
            {
                try
                {
                    connection.Open();

                    string getChoiceText = "Text";
                    if (type == 1)
                        getChoiceText = "Text";
                    else if (type == 2)
                        getChoiceText = "Satisfaction";

                    string command = "INSERT INTO choices(question, choice_content, placement) VALUES('" + selected_question + "', '" + getChoiceText + "', '1')";
                    MySqlCommand cmd = new MySqlCommand(command, connection);
                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK);
                }
            }
        }

        private void choiceResort()
        {
            foreach(DataGridViewRow a in dataGridView1.Rows)
            {
                try
                {
                    connection.Open();
                    string updateCommand = "UPDATE choices SET placement=@placement WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(updateCommand, connection);

                    cmd.Parameters.AddWithValue("@placement", Convert.ToInt32(a.Index) + 1);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(a.Cells[0].Value));

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void questionResort()
        {
            foreach (DataGridViewRow a in dataGridView2.Rows)
            {
                try
                {
                    connection.Open();
                    string updateCommand = "UPDATE questions SET placement=@placement WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(updateCommand, connection);

                    cmd.Parameters.AddWithValue("@placement", Convert.ToInt32(a.Index) + 1);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(a.Cells[0].Value));

                    cmd.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }







        #endregion

        
    }
}
