using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Drawing;
using System.ComponentModel;

namespace qualified_co_location_pattern_mining
{
    class DataIO: InDatasouce
    {
        public List<string> inputdata(string file)
        {
            string[] Lines;
            Lines = File.ReadAllLines(file);
            int k = 0;
            //  MessageBox.Show(Lines.Length+"");
            List<string> h = new List<string>();
            while (k < Lines.Length)
            {
                h.Add(Lines[k]);
                k++;
            }

            return h;
        }

        public List<string> inputconfig(string file)
        {
            string[] Lines;
            Lines = File.ReadAllLines(file);
            int k = 0;
            List<string> h = new List<string>();
            while (Lines[k] != "")
            {
                h.Add(Lines[k]);
            }
            return h;
        }

       

    }


}
