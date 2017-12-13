using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qualified_co_location_pattern_mining
{
    interface InDatasouce
    {

        //  void output(string file);//输出文件
        List<string> inputconfig(string file);//读取输入参数文件
        List<string> inputdata(string file);//读取输入数据文件

    }
}
