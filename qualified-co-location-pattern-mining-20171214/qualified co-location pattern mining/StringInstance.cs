using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qualified_co_location_pattern_mining
{
    public class StringInstance
    {
        public List<String> SplitInstance(string a, string b,string c, string d, string str)
        {
            int Ia = str.IndexOf(a);
            int Ib = str.IndexOf(b);
            int Ic = str.IndexOf(c);
            int Id = str.IndexOf(d);
            List<string> h = new List<string>();
            {
                h.Add(str.Substring(0, Ia));
                h.Add(str.Substring(Ia + 1, Ib - Ia - 1));
                h.Add(str.Substring(Ib + 1, Ic - Ib - 1));
                h.Add(str.Substring(Ic + 1, Id - Ic - 1));               
            }
            return h;
        }
        
        public List<String> SplitString(string[] L, string str)
        {
            List<string> h = new List<string>();
            List<int> index = new List<int>();
            index.Add(0);
            h.Add(str.Substring(0, str.IndexOf(L[0])));
            for (int i = 0; i < L.Length-1; i++)
            {
                index.Add(str.IndexOf(L[i]));//得到第一个分隔符的位置
                index.Add(str.IndexOf(L[i + 1]));//得到下一个分隔符的位置
                h.Add(str.Substring(str.IndexOf(L[i]) + 1, str.IndexOf(L[i + 1]) - str.IndexOf(L[i]) - 1));//得到第一个分隔符的位置+1为所截字符串的第一个字符，其长度为下一个分隔符位置减去上一个再减1             
            }
            return h;
        }

  
      
        public  List<String> SplitString1(String str, char tag)
        {
            List<String> result = new  List<String>();
            String line = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == tag)
                {
                    result.Add(line);
                    line = "";
                }
                else
                {
                    line = line + str[i];
                }
            }
            if (line!="")
            {
                result.Add(line);
            }
             
            return result;
        }
    }
}
