using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace qualified_co_location_pattern_mining
{
    class fileIO
    {

        private void  DatasourceInput(string openfilepath) //选择输入文件
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "txt文件|*.txt|所有文件|*.*";
                openFileDialog.ShowDialog();
                openfilepath = openFileDialog.FileName;
                //openFileDialog.InitialDirectory = "c:\\";//注意这里写路径时要用c:\\而不是c:\   
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                //inputfilepath = openfilepath;                 
            }
            catch { MessageBox.Show("please choose an input file"); }
        }

        private string ResulrOutput()//选择文件输出位置
        {
            string outputfile="";
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            outputfile = path.SelectedPath;              
            return outputfile;
        }      

    }
}
