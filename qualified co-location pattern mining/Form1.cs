using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;



namespace qualified_co_location_pattern_mining
{
    public partial class Form : System.Windows.Forms.Form
    {
        public string inputfilepath = "";
        public static string outputfilepath = "";
        public static string outputname = "";
        public double prev = 0.00;
        public int range = 0;
        public double rand = 0;
         
        public double occ = 0;
        public double quality = 0;
        public double w = 0;


        public List<string> printf = new List<string>();//频繁模式,形式为1+2+3{12-16-19;12-16-20;13-17-21}(0.5,0.4,0.6)=0.4    
        public List<string> printf11 = new List<string>();//频繁模式,形式为1+2+3{12-16-19;12-16-20;13-17-21}(0.5,0.4,0.6)=0.4    
                                                          // public List<List<string>> listfn = new List<List<string>>();//频繁模式,形式为1+2+3
        public List<string> listfn = new List<string>();//频繁模式,形式为1+2+3
                                                        // public List<List<string>> listout = new List<List<string>>();//频繁模式输出表实例1+2+3{1-2-3；2-3-4；}=0.2,0.4,0.4
                                                        //Dictionary<string, Dictionary<string, HashSet<string>>> dicstar = new Dictionary<string, Dictionary<string, HashSet<string>>>();//存放星星表
        public List<string> begindataList = new List<string>();//s1 放的是有序的特征实例及其位置，s22放每个特征的个数，也是有序的
        public List<int> TypeCountList = new List<int>(); // TypeCountList 放的是特征对应的个数 如：[0]=0，[1]=5,[2]=7说明特征1有5个实例，特征2有2个实例 两个数组差为特征的实例范围
        public List<int> TypeInsList = new List<int>();   //TypeInsList 放的是实例对应的特征  如：[1]=1，[2]=1,[3]=2 说明实例1，2都是特征1的实例 实例3是特征2的实例

        public SortedList<int, SortedSet<int>> INs = new SortedList<int, SortedSet<int>>();//实例邻居集，int为实例号，set中是实例的所有邻居
        //public SortedList<string, List<SortedSet<int>>> T2 = new SortedList<string, List<SortedSet<int>>>();//二阶模式表实例集
        public SortedList<string, List<List<int>>> T2 = new SortedList<string, List<List<int>>>();//二阶模式表实例集
        public List<int> sizecounti = new List<int>();//每个特征可能生成的最大阶数
        public List<string> starneighbor = new List<string>();//某特征在星星中与其二阶频繁的特征集
        public List<double> fsn = new List<double>();
        public List<int> fsn2 = new List<int>();

        public Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        //weight+threshold选项----
        private void button_beginWT_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_prev.Text == "" || textBox_r.Text == "")
                { MessageBox.Show("此值不能为缺省，请重新输入！"); }
                else if (double.Parse(textBox_prev.Text) > 0.00 && double.Parse(textBox_prev.Text) < 1.00 && double.Parse(textBox_r.Text) > 0)
                {
                    prev = double.Parse(textBox_prev.Text);
                    rand = double.Parse(textBox_r.Text);
                    occ = double.Parse(textBox_occupancy.Text);
                    w = double.Parse(textBox_weight.Text);
                    quality = double.Parse(textBox_quality.Text);
                    outputfilepath = label_outputpath.Text.ToString();
                }
                else
                {
                    MessageBox.Show("此值不合法，请重新输入！");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("输入错误");
            }
            //  beginfile();

            Thread thread = new Thread(() =>
            {

                InDatasouce F = new DataIO();
                List<string> inputlist = new List<string>();
                inputlist = F.inputdata(inputfilepath);
                //  ------------------------------------------------------------------------------------获得初始数据
                string data = System.DateTime.Today.DayOfYear.ToString() + "weightANDthreshold" + System.DateTime.Now.Minute.ToString();
                outputname = outputfilepath + "\\" + data + ".txt";
                //=============================================================================计算程序运行时间
                Stopwatch timecost = new Stopwatch();
                timecost.Start();
                beginfileWT();
                timecost.Stop();
                TimeSpan ts2 = timecost.Elapsed;
                Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
                MessageBox.Show("计算完毕，共花费" + ts2.TotalMilliseconds + "ms,为您输出结果到txt");

            });
            thread.Start();//启动线程
            thread.IsBackground = true;//后台运行

        }
        //weight+threshold函数----
        public void beginfileWT()
        {
            InDatasouce F = new DataIO();
            List<string> inputlist = new List<string>();
            inputlist = F.inputdata(inputfilepath);
            List<double> ax = new List<double>();
            List<double> ay = new List<double>();
            //  ------------------------------------------------------------------------------------获得初始数据
            #region
            // MessageBox.Show("开始读入数据");
            for (int i = 0; i < inputlist.Count; i++)//获得初始数据
            {
                string[] inputa = inputlist[i].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder s = new StringBuilder();
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCodea = (int)asciiEncoding.GetBytes(inputa[1].ToString())[0];
                s.Append(intAsciiCodea.ToString());
                s.Append(".");
                s.Append(inputa[0].ToString());
                s.Append("(");
                s.Append(System.Convert.ToDouble(inputa[2]));
                s.Append(",");
                s.Append(System.Convert.ToDouble(inputa[3]));
                s.Append(")");
                begindataList.Add(s.ToString());
                ax.Add(System.Convert.ToDouble(inputa[2]));
                ay.Add(System.Convert.ToDouble(inputa[3]));
            }
            MessageBox.Show("数据读取完毕，进入物化阶段");
            //字典序排序

            //-----------------------------------------------------------------------------------------全局变量赋值
            begindataList.Sort();//begindataList 放的是有序的特征实例及其位置，TypeCountList放每个特征的个数，也是有序的
            begindataList.Insert(0, "begin");
            int k = 1;
            int s3no = 1;
            begindataList.Add("over");//begindataList有一个over结尾，避免无法将其中的最后一个特征加不进TypeCountList的情况
            // s2.Add("0");
            TypeCountList.Add(0);
            TypeInsList.Add(0);
            TypeInsList.Add(1);
            DataGrid dg = new DataGrid();//=========================================================datagrad对象
            dg.instancelized(begindataList);
            for (int i = 1; i < begindataList.Count - 1; i++)
            {
                string[] a = begindataList[i].Split('.');
                string[] b = begindataList[i + 1].Split('.');

                if (!b[0].Equals(a[0]))
                {
                    s3no++;
                    TypeInsList.Add(s3no);//TypeInsList放的是实例对应的特征  如：[1]=1，[2]=1,[3]=2 说明实例1，2都是特征1的实例 实例3是特征2的实例
                    TypeCountList.Add(k);//TypeCountList放的是特征对应的个数 如：[0]=0，[1]=5,[2]=7说明特征1有5个实例，特征2有2个实例 两个数组差为特征的实例范围
                    k++;
                }
                else
                {
                    TypeInsList.Add(s3no);
                    k++;
                }
            }

            #endregion
            //--------------------------------------------------------------------------------------物化            
            int maxx = (int)ax.Max() / ((int)rand);//获得最大行数
            int maxy = (int)ay.Max() / ((int)rand);//获得最大列数
            StringBuilder[,] co = dg.Grid(maxx, maxy, (int)rand, dg.instancelized(begindataList));//格化
            //--------------------------------------------------------------------------------------建立实例邻居表
            INs = dg.InstanceNeighbor(maxx, maxy, (int)rand, dg.Grid(maxx, maxy, (int)rand, dg.instancelized(begindataList)), dg.instancelized(begindataList));
            //--------------------------------------------------------------------------------------产生模式 
            Prevalence classprevalence = new Prevalence();
            Constraint_Weight classcons = new Constraint_Weight();
            T2 = classcons.TwoSize(INs, prev, TypeCountList, TypeInsList,w,occ);//得到二阶模式
            //================================================================输出头文件行1特征表行2min_prev

            foreach (var item1 in T2)
            {
                if (!item1.Key.Contains("extend"))
                {
                    StringBuilder fs2string = new StringBuilder();
                    string h1 = item1.Key;
                    string hpi = T2[h1][T2[h1].Count() - 1][0].ToString();
                    string hoi = T2[h1][T2[h1].Count() - 1][1].ToString();
                    string hwpo = T2[h1][T2[h1].Count() - 1][2].ToString();
                    fs2string.Append(h1);
                    fs2string.Append("=PI:");
                    fs2string.Append((double.Parse(hpi) / 100.00).ToString());
                    fs2string.Append("  OI:");
                    fs2string.Append((double.Parse(hoi) / 100.00).ToString());
                    fs2string.Append("  Quality:");
                    fs2string.Append((double.Parse(hwpo) / 100.00).ToString());
                    printf11.Add(fs2string.ToString());
                }
                else { }
            }

            //=======================================================================================================输出二阶aaaaa
            if (!System.IO.File.Exists(outputname))
            {
                FileStream files1 = new FileStream(outputname, FileMode.Create, FileAccess.Write);//创建写入文件
                StreamWriter sw = new StreamWriter(files1);
                sw.WriteLine("min_prev=" + prev.ToString());
                sw.WriteLine("d=" + rand.ToString());
                sw.WriteLine("min_occ=" + occ.ToString());
                sw.WriteLine("weight=" + w.ToString());
                foreach (var item in printf11)//获得初始数据
                {
                    sw.WriteLine(item);//开始写入值
                }
                sw.Close();
                files1.Close();
            }
            //======================================================================================================输出多阶
            SortedList<int, string> listfn = new SortedList<int, string>();
            List<SortedList<string, List<List<int>>>> fnn = new List<SortedList<string, List<List<int>>>>();
            fnn.Add(T2);
            for (int i = 3; i < 10 ; i++)
            {
                 
                SortedList<string, List<List<int>>> CNk = new SortedList<string, List<List<int>>>();
                CNk = classcons.MoreSize1(fnn[0], INs, prev, TypeCountList, TypeInsList,w,occ);//得到k+1阶
               // if (fnn[0].Count == 0) { break; }
                fnn.Add(CNk);
                //按阶输出多阶
                List<string> printfm = new List<string>();
                foreach (var item1 in CNk)
                {
                    if (!item1.Key.Contains("extend"))
                    {
                        StringBuilder fsstring = new StringBuilder();
                        string h1 = item1.Key;
                        string hpi = CNk[h1][CNk[h1].Count() - 1][0].ToString();
                        string hoi = CNk[h1][CNk[h1].Count() - 1][1].ToString();
                        string hwpo = CNk[h1][CNk[h1].Count() - 1][2].ToString();
                        fsstring.Append(h1);
                        fsstring.Append("=PI:");
                        fsstring.Append((double.Parse(hpi) / 100.00).ToString());
                        fsstring.Append("  OI:");
                        fsstring.Append((double.Parse(hoi) / 100.00).ToString());
                        fsstring.Append("  Quality:");
                        fsstring.Append((double.Parse(hwpo) / 100.00).ToString());
                        printfm.Add(fsstring.ToString());
                    }

                    FileStream files2 = new FileStream(outputname, FileMode.Append);
                    StreamWriter sw2 = new StreamWriter(files2);
                    foreach (var item in printfm)//获得初始数据
                    {
                        sw2.WriteLine(item);//开始写入值
                    }
                    sw2.Close();
                }
                fnn.RemoveAt(0);
            }
        }

        #region
        //skyline选项----
        private void button_beginSkyline_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_prev.Text == "" || textBox_r.Text == "")
                { MessageBox.Show("此值不能为缺省，请重新输入！"); }
                else if (double.Parse(textBox_prev.Text) > 0.00 && double.Parse(textBox_prev.Text) < 1.00 && double.Parse(textBox_r.Text) > 0)
                {
                    prev = double.Parse(textBox_prev.Text);
                    rand = double.Parse(textBox_r.Text);
                    occ = double.Parse(textBox_occupancy.Text);
                    outputfilepath = label_outputpath.Text.ToString();
                }
                else
                {
                    MessageBox.Show("此值不合法，请重新输入！");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("输入错误");
            }
            Thread thread = new Thread(() =>
            {

                InDatasouce F = new DataIO();
                List<string> inputlist = new List<string>();
                inputlist = F.inputdata(inputfilepath);
                //  ------------------------------------------------------------------------------------获得初始数据
                string data = System.DateTime.Today.DayOfYear.ToString() + "weightANDthreshold" + System.DateTime.Now.Minute.ToString();
                outputname = outputfilepath + "\\" + data + ".txt";
                //=============================================================================计算程序运行时间
                Stopwatch timecost = new Stopwatch();
                timecost.Start();
                beginfileSkyline();
                timecost.Stop();
                TimeSpan ts2 = timecost.Elapsed;
                Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
                MessageBox.Show("计算完毕，共花费" + ts2.TotalMilliseconds + "ms,为您输出结果到txt");

            });
            thread.Start();//启动线程
            thread.IsBackground = true;//后台运行
        }
        //weight选项
        private void button_beginWeight_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_prev.Text == "" || textBox_r.Text == "")
                { MessageBox.Show("此值不能为缺省，请重新输入！"); }
                else if (double.Parse(textBox_prev.Text) > 0.00 && double.Parse(textBox_prev.Text) < 1.00 && double.Parse(textBox_r.Text) > 0)
                {
                    prev = double.Parse(textBox_prev.Text);
                    rand = double.Parse(textBox_r.Text);
                    occ = double.Parse(textBox_occupancy.Text);
                    w = double.Parse(textBox_weight.Text);
                    quality = double.Parse(textBox_quality.Text);
                    outputfilepath = label_outputpath.Text.ToString();
                }
                else
                {
                    MessageBox.Show("此值不合法，请重新输入！");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("输入错误");
            }
            Thread thread = new Thread(() =>
            {

                InDatasouce F = new DataIO();
                List<string> inputlist = new List<string>();
                inputlist = F.inputdata(inputfilepath);
                //  ------------------------------------------------------------------------------------获得初始数据
                string data = System.DateTime.Today.DayOfYear.ToString() + "weightANDthreshold" + System.DateTime.Now.Minute.ToString();
                outputname = outputfilepath + "\\" + data + ".txt";
                //=============================================================================计算程序运行时间
                Stopwatch timecost = new Stopwatch();
                timecost.Start();
                beginfileWeight();
                timecost.Stop();
                TimeSpan ts2 = timecost.Elapsed;
                Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
                MessageBox.Show("计算完毕，共花费" + ts2.TotalMilliseconds + "ms,为您输出结果到txt");

            });
            thread.Start();//启动线程
            thread.IsBackground = true;//后台运行
        }


        
        //weight函数-----
        public void beginfileWeight()
        {
            InDatasouce F = new DataIO();
            List<string> inputlist = new List<string>();
            inputlist = F.inputdata(inputfilepath);
            List<double> ax = new List<double>();
            List<double> ay = new List<double>();
            //  ------------------------------------------------------------------------------------获得初始数据
            #region
            // MessageBox.Show("开始读入数据");
            for (int i = 0; i < inputlist.Count; i++)//获得初始数据
            {
                string[] inputa = inputlist[i].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder s = new StringBuilder();
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCodea = (int)asciiEncoding.GetBytes(inputa[1].ToString())[0];
                s.Append(intAsciiCodea.ToString());
                s.Append(".");
                s.Append(inputa[0].ToString());
                s.Append("(");
                s.Append(System.Convert.ToDouble(inputa[2]));
                s.Append(",");
                s.Append(System.Convert.ToDouble(inputa[3]));
                s.Append(")");
                begindataList.Add(s.ToString());
                ax.Add(System.Convert.ToDouble(inputa[2]));
                ay.Add(System.Convert.ToDouble(inputa[3]));
            }
            MessageBox.Show("数据读取完毕，进入物化阶段");
            //字典序排序

            //-----------------------------------------------------------------------------------------全局变量赋值
            begindataList.Sort();//begindataList 放的是有序的特征实例及其位置，TypeCountList放每个特征的个数，也是有序的
            begindataList.Insert(0, "begin");
            int k = 1;
            int s3no = 1;
            begindataList.Add("over");//begindataList有一个over结尾，避免无法将其中的最后一个特征加不进TypeCountList的情况
            // s2.Add("0");
            TypeCountList.Add(0);
            TypeInsList.Add(0);
            TypeInsList.Add(1);
            DataGrid dg = new DataGrid();//=========================================================datagrad对象
            dg.instancelized(begindataList);
            for (int i = 1; i < begindataList.Count - 1; i++)
            {
                string[] a = begindataList[i].Split('.');
                string[] b = begindataList[i + 1].Split('.');

                if (!b[0].Equals(a[0]))
                {
                    s3no++;
                    TypeInsList.Add(s3no);//TypeInsList放的是实例对应的特征  如：[1]=1，[2]=1,[3]=2 说明实例1，2都是特征1的实例 实例3是特征2的实例
                    TypeCountList.Add(k);//TypeCountList放的是特征对应的个数 如：[0]=0，[1]=5,[2]=7说明特征1有5个实例，特征2有2个实例 两个数组差为特征的实例范围
                    k++;
                }
                else
                {
                    TypeInsList.Add(s3no);
                    k++;
                }
            }

            #endregion
            //--------------------------------------------------------------------------------------物化            
            int maxx = (int)ax.Max() / ((int)rand);//获得最大行数
            int maxy = (int)ay.Max() / ((int)rand);//获得最大列数
            StringBuilder[,] co = dg.Grid(maxx, maxy, (int)rand, dg.instancelized(begindataList));//格化
            //--------------------------------------------------------------------------------------建立实例邻居表
            INs = dg.InstanceNeighbor(maxx, maxy, (int)rand, dg.Grid(maxx, maxy, (int)rand, dg.instancelized(begindataList)), dg.instancelized(begindataList));
            //--------------------------------------------------------------------------------------产生模式 
            Prevalence classprevalence = new Prevalence();
            T2 = classprevalence.TwoSize(INs, prev, TypeCountList, TypeInsList);//得到二阶模式



            //================================================================输出头文件行1特征表行2min_prev

            foreach (var item1 in T2)
            {
                if (!item1.Key.Contains("extend"))
                {
                    StringBuilder fs2string = new StringBuilder();
                    string h1 = item1.Key;
                    string h2 = T2[h1][T2[h1].Count() - 1].First().ToString();
                    fs2string.Append(h1);
                    for (int ii = 0; ii < T2[h1].Count; ii++)
                    {
                        fs2string.Append("(");
                        foreach (var item11 in T2[h1][ii])//获得初始数据
                        {
                            fs2string.Append(item11);
                            fs2string.Append("-");
                        }
                        //fs2string.Append(T2[h1][ii].Last());
                        fs2string.Append(")");
                    }

                    fs2string.Append("=");
                    fs2string.Append((double.Parse(h2) / 100.00).ToString());
                    printf11.Add(fs2string.ToString());
                }
                else { }
            }

            //=======================================================================================================输出二阶aaaaa
            if (!System.IO.File.Exists(outputname))
            {
                FileStream files1 = new FileStream(outputname, FileMode.Create, FileAccess.Write);//创建写入文件
                StreamWriter sw = new StreamWriter(files1);

                sw.WriteLine("min_prev=" + prev.ToString());
                sw.WriteLine("d=" + rand.ToString());
                sw.WriteLine("min_fd=" + occ.ToString());


                foreach (var item in printf11)//获得初始数据
                {
                    sw.WriteLine(item);//开始写入值
                }
                sw.Close();
                files1.Close();
            }
            //======================================================================================================输出多阶
            SortedList<string, List<List<int>>> CNk1 = new SortedList<string, List<List<int>>>();

            CNk1 = classprevalence.MoreSize1(T2, INs, prev, TypeCountList, TypeInsList);
            //输出多阶
            List<string> printfm = new List<string>();
            foreach (var item1 in CNk1)
            {
                if (!item1.Key.Contains("extend"))
                {
                    StringBuilder fsstring = new StringBuilder();
                    string h1 = item1.Key;
                    string h2 = CNk1[h1][CNk1[h1].Count() - 1].First().ToString();
                    fsstring.Append(h1);
                    for (int ii = 0; ii < CNk1[h1].Count - 1; ii++)
                    {
                        fsstring.Append("(");
                        foreach (var item11 in CNk1[h1][ii])//获得初始数据
                        {
                            fsstring.Append(item11);
                            fsstring.Append("-");
                        }
                        //fs2string.Append(T2[h1][ii].Last());
                        fsstring.Append(")");

                    }

                    fsstring.Append("=");
                    fsstring.Append((double.Parse(h2) / 100.00).ToString());
                    printfm.Add(fsstring.ToString());

                    FileStream files2 = new FileStream(outputname, FileMode.Append);
                    StreamWriter sw2 = new StreamWriter(files2);
                    foreach (var item in printfm)//获得初始数据
                    {
                        sw2.WriteLine(item);//开始写入值
                    }
                    sw2.Close();
                }


            }

        }

        //skyline函数-----
        public void beginfileSkyline()
        {
            InDatasouce F = new DataIO();
            List<string> inputlist = new List<string>();
            inputlist = F.inputdata(inputfilepath);
            List<double> ax = new List<double>();
            List<double> ay = new List<double>();
            //  ------------------------------------------------------------------------------------获得初始数据
            #region
            // MessageBox.Show("开始读入数据");
            for (int i = 0; i < inputlist.Count; i++)//获得初始数据
            {
                string[] inputa = inputlist[i].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder s = new StringBuilder();
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCodea = (int)asciiEncoding.GetBytes(inputa[1].ToString())[0];
                s.Append(intAsciiCodea.ToString());
                s.Append(".");
                s.Append(inputa[0].ToString());
                s.Append("(");
                s.Append(System.Convert.ToDouble(inputa[2]));
                s.Append(",");
                s.Append(System.Convert.ToDouble(inputa[3]));
                s.Append(")");
                begindataList.Add(s.ToString());
                ax.Add(System.Convert.ToDouble(inputa[2]));
                ay.Add(System.Convert.ToDouble(inputa[3]));
            }
            MessageBox.Show("数据读取完毕，进入物化阶段");
            //字典序排序

            //-----------------------------------------------------------------------------------------全局变量赋值
            begindataList.Sort();//begindataList 放的是有序的特征实例及其位置，TypeCountList放每个特征的个数，也是有序的
            begindataList.Insert(0, "begin");
            int k = 1;
            int s3no = 1;
            begindataList.Add("over");//begindataList有一个over结尾，避免无法将其中的最后一个特征加不进TypeCountList的情况
            // s2.Add("0");
            TypeCountList.Add(0);
            TypeInsList.Add(0);
            TypeInsList.Add(1);
            DataGrid dg = new DataGrid();//=========================================================datagrad对象
            dg.instancelized(begindataList);
            for (int i = 1; i < begindataList.Count - 1; i++)
            {
                string[] a = begindataList[i].Split('.');
                string[] b = begindataList[i + 1].Split('.');

                if (!b[0].Equals(a[0]))
                {
                    s3no++;
                    TypeInsList.Add(s3no);//TypeInsList放的是实例对应的特征  如：[1]=1，[2]=1,[3]=2 说明实例1，2都是特征1的实例 实例3是特征2的实例
                    TypeCountList.Add(k);//TypeCountList放的是特征对应的个数 如：[0]=0，[1]=5,[2]=7说明特征1有5个实例，特征2有2个实例 两个数组差为特征的实例范围
                    k++;
                }
                else
                {
                    TypeInsList.Add(s3no);
                    k++;
                }
            }

            #endregion
            //--------------------------------------------------------------------------------------物化            
            int maxx = (int)ax.Max() / ((int)rand);//获得最大行数
            int maxy = (int)ay.Max() / ((int)rand);//获得最大列数
            StringBuilder[,] co = dg.Grid(maxx, maxy, (int)rand, dg.instancelized(begindataList));//格化
            //--------------------------------------------------------------------------------------建立实例邻居表
            INs = dg.InstanceNeighbor(maxx, maxy, (int)rand, dg.Grid(maxx, maxy, (int)rand, dg.instancelized(begindataList)), dg.instancelized(begindataList));
            //--------------------------------------------------------------------------------------产生模式 
            Prevalence classprevalence = new Prevalence();
            T2 = classprevalence.TwoSize(INs, prev, TypeCountList, TypeInsList);//得到二阶模式



            //================================================================输出头文件行1特征表行2min_prev

            foreach (var item1 in T2)
            {
                if (!item1.Key.Contains("extend"))
                {
                    StringBuilder fs2string = new StringBuilder();
                    string h1 = item1.Key;
                    string h2 = T2[h1][T2[h1].Count() - 1].First().ToString();
                    fs2string.Append(h1);
                    for (int ii = 0; ii < T2[h1].Count; ii++)
                    {
                        fs2string.Append("(");
                        foreach (var item11 in T2[h1][ii])//获得初始数据
                        {
                            fs2string.Append(item11);
                            fs2string.Append("-");
                        }
                        //fs2string.Append(T2[h1][ii].Last());
                        fs2string.Append(")");
                    }

                    fs2string.Append("=");
                    fs2string.Append((double.Parse(h2) / 100.00).ToString());
                    printf11.Add(fs2string.ToString());
                }
                else { }
            }

            //=======================================================================================================输出二阶aaaaa
            if (!System.IO.File.Exists(outputname))
            {
                FileStream files1 = new FileStream(outputname, FileMode.Create, FileAccess.Write);//创建写入文件
                StreamWriter sw = new StreamWriter(files1);

                sw.WriteLine("min_prev=" + prev.ToString());
                sw.WriteLine("d=" + rand.ToString());
                sw.WriteLine("min_occ=" + occ.ToString());


                foreach (var item in printf11)//获得初始数据
                {
                    sw.WriteLine(item);//开始写入值
                }
                sw.Close();
                files1.Close();
            }
            //======================================================================================================输出多阶
            SortedList<string, List<List<int>>> CNk1 = new SortedList<string, List<List<int>>>();

            CNk1 = classprevalence.MoreSize1(T2, INs, prev, TypeCountList, TypeInsList);
            //输出多阶
            List<string> printfm = new List<string>();
            foreach (var item1 in CNk1)
            {
                if (!item1.Key.Contains("extend"))
                {
                    StringBuilder fsstring = new StringBuilder();
                    string h1 = item1.Key;
                    string h2 = CNk1[h1][CNk1[h1].Count() - 1].First().ToString();
                    fsstring.Append(h1);
                    for (int ii = 0; ii < CNk1[h1].Count - 1; ii++)
                    {
                        fsstring.Append("(");
                        foreach (var item11 in CNk1[h1][ii])//获得初始数据
                        {
                            fsstring.Append(item11);
                            fsstring.Append("-");
                        }
                        //fs2string.Append(T2[h1][ii].Last());
                        fsstring.Append(")");

                    }

                    fsstring.Append("=");
                    fsstring.Append((double.Parse(h2) / 100.00).ToString());
                    printfm.Add(fsstring.ToString());

                    FileStream files2 = new FileStream(outputname, FileMode.Append);
                    StreamWriter sw2 = new StreamWriter(files2);
                    foreach (var item in printfm)//获得初始数据
                    {
                        sw2.WriteLine(item);//开始写入值
                    }
                    sw2.Close();
                }


            }

        }

        #endregion

        private void button_datasource_Click(object sender, EventArgs e)//选择输入文件
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "txt文件|*.txt|所有文件|*.*";
                openFileDialog.ShowDialog();
                this.Text = openFileDialog.FileName;
                //openFileDialog.InitialDirectory = "c:\\";//注意这里写路径时要用c:\\而不是c:\   
                openFileDialog.RestoreDirectory = true;
                openFileDialog.FilterIndex = 1;
                inputfilepath = this.Text.ToString();
                label_inputpath.Text = "input：" + this.Text.ToString();

            }
            catch { MessageBox.Show("please choose an input file"); }
        }

        private void button_scan_Click(object sender, EventArgs e)//选择文件输出位置
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.Text = path.SelectedPath;
            label_outputpath.Text = path.SelectedPath;
            //  outputfilepath = textBox_file.Text.ToString();
        }

        private void buttonTest_Click(object sender, EventArgs e)//测试
        {
            //测试字符串分割实例
            #region
            /*
            string str = "a.1(2.5,4.3)";
            stringinstance o=new stringinstance();
            o.splitinstance(".","(",",",")",str);
            string m = "";
            for (int i = 0; i < o.splitinstance(".", "(", ",", ")", str).count; i++)
            {
                 
                 m=o.splitinstance(".", "(", ",", ")", str)[i]; //----这就是你要的结果
                messagebox.show(m);
            }
            */
            #endregion

            //测试PA
            #region
            List<SortedSet<int>> list = new List<SortedSet<int>>();//装实例表
            List<SortedSet<int>> listPA = new List<SortedSet<int>>();//装PA包括的实例
            //List<SortedSet<int>> listline = new List<SortedSet<int>>();
            List<int> listline = new List<int>();//装每个PA包括的行号

            SortedSet<int> ssin1 = new SortedSet<int>() { 1, 4, 7 };
            SortedSet<int> ssin2 = new SortedSet<int>() { 1, 4, 8 };
            SortedSet<int> ssin3 = new SortedSet<int>() { 2, 5, 9 };
            SortedSet<int> ssin4 = new SortedSet<int>() { 3, 6, 9 };
            SortedSet<int> ssin5 = new SortedSet<int>() { };
            list.Add(ssin1); list.Add(ssin2); list.Add(ssin3); list.Add(ssin4); list.Add(ssin5);

            List<SortedSet<int>> listcn = new List<SortedSet<int>>();//装实例邻居表
            List<SortedSet<int>> listCA = new List<SortedSet<int>>();//装CA包括的实例

            SortedSet<int> csin1 = new SortedSet<int>() { 1, 4, 7, 10, 11, 14 };
            SortedSet<int> csin2 = new SortedSet<int>() { 1, 4, 8, 10, 14 };
            SortedSet<int> csin3 = new SortedSet<int>() { 2, 5, 9, 12, 15 };
            SortedSet<int> csin4 = new SortedSet<int>() { 3, 6, 9, 13, 16 };
            listcn.Add(csin1); listcn.Add(csin2); listcn.Add(csin3); listcn.Add(csin4);

            //装邻居
            SortedList<int, SortedSet<int>> INs = new SortedList<int, SortedSet<int>>();//装实例邻居表
                                                                                        //例子中共有16个实例，5个特征
            SortedSet<int> In1 = new SortedSet<int>() { 1, 4, 7, 8, 10, 11, 14, 16, 18 };
            INs.Add(1, In1);
            SortedSet<int> In2 = new SortedSet<int>() { 4, 1, 7, 8, 10, 11, 14, 17 };
            INs.Add(4, In2);
            SortedSet<int> In3 = new SortedSet<int>() { 4, 1, 7, 10, 11, 14, 19 };
            INs.Add(7, In3);
            //首先赋值第一行初始化
            //listPA.Add(list[0]);//PA初始化            
            // listline.Add(0);//行号初始化
            //for (int i = 1; i < list.Count(); i++)//讲实例表分到不同的PA中
            //{
            //    int j = listPA.Count - 1;
            //    SortedSet<int> tmpset = new SortedSet<int>();
            //    if (listPA[j].Overlaps(list[i]))
            //    {
            //        listPA[j].UnionWith(list[i]);
            //    }
            //    else
            //    {
            //        listline.Add(i);
            //        listPA.Add(list[i]);
            //    }
            //}

            //Occupation oc = new Occupation();
            //oc.PA(list);
            //for (int i = 0; i < oc.PA(list).Count(); i++)//讲实例表分到不同的PA中
            //{
            //    StringBuilder m = new StringBuilder();
            //    foreach (var item in oc.PA(list)[i])
            //    {
            //        m.Append(item.ToString()); m.Append(",");
            //    }
            //    MessageBox.Show(m.ToString());
            //}
            #endregion

            //=========================================================================测试公共邻域
            #region


            //List<int> line = new List<int>();

            //foreach (var item in oc.PA(list)[oc.PA(list).Count() - 1])//取出listline
            //{
            //    line.Add(item);
            //}
            //for (int i = 0; i < line.Count()-1; i++)
            //{
            //    SortedSet<int> tmpset = new SortedSet<int>();
            //    tmpset = listcn[line[i]];
            //    for (int j = line[i]+1; j < line[i + 1]; j++)
            //    {
            //        tmpset.UnionWith(listcn[j]);
            //    }
            //    listCA.Add(tmpset);
            //}
            //listPA = oc.PA(list);
            //listCA = oc.CA(oc.PA(list)[oc.PA(list).Count() - 1], listcn);
            //for (int i = 0; i < listCA.Count(); i++)//讲实例表分到不同的PA中
            //{
            //    StringBuilder m1 = new StringBuilder();
            //    foreach (var item in listCA[i])
            //    {
            //        m1.Append(item);
            //        m1.Append(",");
            //    }
            //    MessageBox.Show(m1.ToString());
            //}
            //double index = oc.OccupationIndex(oc.CA(oc.PA(list)[oc.PA(list).Count() - 1], listcn), oc.PA(list));
            // MessageBox.Show(index.ToString());

            //for (int i = 0; i < listline.Count(); i++)//讲实例表分到不同的PA中
            //{

            //    MessageBox.Show(listline[i].ToString());
            //}
            #endregion

            //==================================================测试组合函数
            #region
            //Prevalence newp = new Prevalence();
            //StringInstance snew = new StringInstance();
            //string testpname = "A+B+C+D";

            //foreach (var comb in Prevalence.Combinations(snew.SplitString1(testpname, '+'), 0, snew.SplitString1(testpname, '+').Count, snew.SplitString1(testpname, '+').Count - 1))//测试子模式,对于ABC（tlist）,从tlist[0]开始索引，得到tlist.Count个sizek - 1阶的组合
            //{
            //    StringBuilder l = new StringBuilder();
            //    string[] hcomb = comb.Take(snew.SplitString1(testpname, '+').Count - 1).ToArray();
            //    l.Append(hcomb[0]);
            //    for (int jj = 1; jj < comb.Take(snew.SplitString1(testpname, '+').Count - 1).Count(); jj++)
            //    {
            //        l.Append("+");
            //        l.Append(hcomb[jj]);
            //    }
            //    l.ToString();
            //    //if (!lastT.ContainsKey(l.ToString()))
            //    //{
            //    //    sign = 0;
            //    //    break;
            //    //}//剪枝步
            //    MessageBox.Show(l.ToString());
            //}
            #endregion

            //测试枚举
            #region
            //string h = "ABC";
            //SortedSet<int> listve = new SortedSet<int>();
            //listve.Add(1); listve.Add(3); listve.Add(4); listve.Add(6); listve.Add(7); listve.Add(9);
            //var q = from int s5 in listve
            //        where s5>5 //字符串是以A开头，并且长度为4位的
            //        select s5;

            //for (int i = 0; i < q.ToList().Count(); i++)//讲实例表分到不同的pa中
            //{

            //    MessageBox.Show(q.ToList()[i].ToString());
            //}
            #endregion
            //测试筛选频繁模式扩展特征
            #region
            SortedSet<int> newextend = new SortedSet<int>() { 4, 5 };
            List<int> listjj = new List<int>();//存放第j个特征参与在pname+j表实例中的个数
            List<int> extendlist = new List<int>();
            List<int> TypeinsList = new List<int>(); //TypeinsList.Add(0); TypeinsList.Add(3); TypeinsList.Add(6); TypeinsList.Add(9); TypeinsList.Add(13); TypeinsList.Add(16);
            TypeinsList.Add(0);
            TypeinsList.Add(1); TypeinsList.Add(1); TypeinsList.Add(1);
            TypeinsList.Add(2); TypeinsList.Add(2); TypeinsList.Add(2);
            TypeinsList.Add(3); TypeinsList.Add(3); TypeinsList.Add(3);
            TypeinsList.Add(4); TypeinsList.Add(4); TypeinsList.Add(4); TypeinsList.Add(4);
            TypeinsList.Add(5); TypeinsList.Add(5); TypeinsList.Add(5);

            /* extendlist = newextend.ToList();//存放newextend矩阵列对应的特征号
                                             //初始化listjj,其长度为扩展特征的个数
             for (int ii = 0; ii < extendlist.Count; ii++)
             {
                 listjj.Add(0);
             }
             SortedSet<int> unionCN = new SortedSet<int>() { };
             for (int ii = 0; ii < listcn.Count; ii++)//得到所有的有序集合
             {
                 unionCN.UnionWith(listcn[ii]);
             }
             foreach (var unitem in unionCN)
             {
                 if (newextend.Contains(TypeinsList[unitem]))
                 {
                     listjj[extendlist.IndexOf(TypeinsList[unitem])]++;
                 }
             }
             for (int i = 0; i < listjj.Count(); i++)//讲实例表分到不同的PA中
             {

                 MessageBox.Show(listjj[i].ToString());
             }

             //测试行实例邻居
             Prevalence pc = new Prevalence();
             SortedSet<int> row = new SortedSet<int>() { 1, 4, 7 };

             foreach (var item in pc.GetRowCN(row, INs))
             {
                 MessageBox.Show(item.ToString());
             }

         }*/
            #endregion

            string pname = "1+2";
            string hh=pname.Substring(0,pname.Length-2);
            MessageBox.Show(hh);
        }//test函数结尾

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}