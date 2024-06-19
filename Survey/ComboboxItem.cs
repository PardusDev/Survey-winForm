using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey
{
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object dbID { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
