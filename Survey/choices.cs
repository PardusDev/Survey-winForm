using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Survey
{
    public class choiceTxtBx : TextBox
    {
        public int dbID { get; set; }

        public choiceTxtBx() : base()
        {
            this.Left = 20;
            this.Width = 200;
            this.MaxLength = 96;
        }
    }

    public class choiceTrckBr : TrackBar
    {
        public int dbID { get; set; }

        public choiceTrckBr(Panel container, int top) : base()
        {
            this.Left = 20;
            this.Minimum = 0;
            this.Maximum = 4;
            this.Value = 0;
            this.Width = 200;
            this.Top = top;

            // Her bir değer için label oluşturalım.
            Label one = new Label();
            one.Left = 27;
            one.Text = "1";
            one.Top = Top + 30;
            one.AutoSize = true;
            container.Controls.Add(one);

            Label two = new Label();
            two.Left = 71;
            two.Text = "2";
            two.Top = Top + 30;
            two.AutoSize = true;
            container.Controls.Add(two);

            Label three = new Label();
            three.Left = 115;
            three.Text = "3";
            three.Top = Top + 30;
            three.AutoSize = true;
            container.Controls.Add(three);

            Label four = new Label();
            four.Left = 158;
            four.Text = "4";
            four.Top = Top + 30;
            four.AutoSize = true;
            container.Controls.Add(four);

            Label five = new Label();
            five.Left = 201;
            five.Text = "5";
            five.Top = Top + 30;
            five.AutoSize = true;
            container.Controls.Add(five);
        }
    }

    public class choiceRbt : RadioButton
    {
        public int dbID { get; set; }
        public string GroupName { get; set; }

        public choiceRbt() : base()
        {
            this.Font = new Font("Arial", 9);
            this.Width = 250;
            this.Left = 20;
        }
    }

    public class choiceChckBx : CheckBox
    {
        public int dbID { get; set; }

        public choiceChckBx() : base()
        {
            this.Font = new Font("Arial", 9);
            this.Width = 250;
            this.Left = 20;
        }
    }

    public class choiceCmbBx : ComboBox
    {
        public int dbID { get; set; }

        public choiceCmbBx() : base()
        {
            this.Width = 170;
            this.Left = 20;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
