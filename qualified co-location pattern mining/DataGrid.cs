using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qualified_co_location_pattern_mining
{
    class DataGrid
    {

       
        public StringBuilder[,] Grid(int maxx, int maxy, int d, SortedList<int, Instance> stlistins)//物化 返回一个[,]
        {
            

            StringBuilder[,] co = new StringBuilder[maxx + 1, maxy + 1];//确定矩阵的大小
            for (int i = 0; i < maxx + 1; i++)//初始化矩阵
            {
                for (int j = 0; j < maxy + 1; j++)
                {
                    co[i, j] = new StringBuilder("");
                }
            }
            for (int i = 1; i < stlistins.Count+1; i++)//物化
            {
                //将特征按照s1的下标重新顺序编号
                int hx = stlistins[i].getx(d);
                int hy = stlistins[i].gety(d);
                string coo = stlistins[i].Id.ToString();//放入实例编号
                StringBuilder m = new StringBuilder(1);
                co[hx, hy].Append(coo);
                co[hx, hy].Append(";");

            }
            return co;
        }


        public SortedList<int, SortedSet<int>> InstanceNeighbor(int maxx,int maxy,int d, StringBuilder[,]co, SortedList<int, Instance> stlistin)//实例邻居集，输入为矩阵和实例号为主键的实例集合//返回所有实例的有序邻居集
        {
            StringInstance si = new StringInstance();
            SortedList<int, SortedSet<int>> nei = new SortedList<int, SortedSet<int>>();// 
            foreach (var item in stlistin)//初始化邻居集
            {
                SortedSet<int> s = new SortedSet<int>();
                s.Add(item.Key);
                nei.Add(item.Key, s);
            }
            Instance insexample=new Instance();
            for (int i = 0; i < maxx + 1; i++)//-------------------------------------------------------开始遍历格
                for (int j = 0; j < maxy + 1; j++)
                {
                    if (co[i, j].Length!=0)
                    {
                        //string[] cp1 = co[i, j].ToString().Split(';'); 
                        List<string> cp1 = new List<string>();
                        cp1=si.SplitString1(co[i, j].ToString(), ';');//拆分同一格子内实例号,并寻找该格子内实例的所有邻居
                        List<int> indexcount = new List<int>();//存放统一特征编号
                        for (int ii = 0; ii < cp1.Count(); ii++)
                        {
                            int index1 = Int32.Parse(cp1[ii].ToString());
                            indexcount.Add(index1);
                            //存放统一特征编号并且已经经过排序
                        }
                        indexcount.Sort(); 
                        for (int ii = 0; ii < indexcount.Count ; ii++) //----------------一格之间邻居，先查看是否是同一特征
                        {
                            for (int jj = ii + 1; jj < indexcount.Count; jj++)
                            {
                                 // 格内邻居不需要经过计算
                                    if (stlistin[indexcount[ii]].Type != stlistin[indexcount[jj]].Type)
                                    {
                                       nei[indexcount[ii]].Add(indexcount[jj]);
                                       nei[indexcount[jj]].Add(indexcount[ii]);

                                } 
                            }                            
                        }

                        for (int ii = 0; ii < indexcount.Count ; ii++)//----------------不同格之间邻居
                        {
                            int index = Int32.Parse(indexcount[ii].ToString());
                            double x = stlistin[index].x;
                            double y = stlistin[index].y;
                            if (i != maxx && j != maxy && j != 0)//---------------------中间的格子
                            {
                                StringBuilder p0 = new StringBuilder();
                                p0.Append(co[i, j + 1]); p0.Append(co[i + 1, j - 1]); p0.Append(co[i + 1, j]); p0.Append(co[i + 1, j + 1]);
                                //string[]p2=new string[100*1024*1024];
                               // string[] p2 = p0.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                List<string> p2 = new List<string>();
                                p2 = si.SplitString1(p0.ToString(), ';');
                                for (int iii = 0; iii < p2.Count; iii++)
                                {       //string[] p3 = p2[iii].Split('(', ',', ')');
                                    if (stlistin[Int32.Parse(p2[iii].ToString())].Type != stlistin[index].Type)
                                    {
                                        if (insexample.distance(x, y, stlistin[Int32.Parse(p2[iii].ToString())].x, stlistin[Int32.Parse(p2[iii].ToString())].y, d))//若邻近
                                        {
                                            nei[index].Add(Int32.Parse(p2[iii]));
                                            nei[Int32.Parse(p2[iii])].Add(index);
                                        }
                                    }
                                }
                            }
                            else if (j == 0 && i != maxx && j != maxy)//---------------------最左边的格子
                            {
                                StringBuilder p0 = new StringBuilder();
                                p0.Append(co[i, j + 1]); p0.Append(co[i + 1, j]); p0.Append(co[i + 1, j + 1]);
                                //string[] p2 = p0.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                List<string>  p2 = new List<string>();
                                p2 = si.SplitString1(p0.ToString(), ';');
                                for (int iii = 0; iii < p2.Count; iii++)
                                {

                                    if (stlistin[Int32.Parse(p2[iii].ToString())].Type != stlistin[index].Type)
                                    {
                                        if (insexample.distance(x, y, stlistin[Int32.Parse(p2[iii].ToString())].x, stlistin[Int32.Parse(p2[iii].ToString())].y, d))//若邻近
                                        {
                                            nei[index].Add(Int32.Parse(p2[iii]));
                                            nei[Int32.Parse(p2[iii])].Add(index);

                                        }
                                    }
                                }
                            }
                            else if (j == maxy && i != maxx)//---------------------最右边的格子
                            {
                                StringBuilder p0 = new StringBuilder();
                                p0.Append(co[i + 1, j - 1]); p0.Append(co[i + 1, j]);
                                List<string> p2 = new List<string>();
                                p2 = si.SplitString1(p0.ToString(), ';');
                                //string[] p2 = p0.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int iii = 0; iii < p2.Count; iii++)
                                {
                                    if (stlistin[Int32.Parse(p2[iii].ToString())].Type != stlistin[index].Type)
                                    {
                                        if (insexample.distance(x, y, stlistin[Int32.Parse(p2[iii].ToString())].x, stlistin[Int32.Parse(p2[iii].ToString())].y, d))//若邻近
                                        {
                                            nei[index].Add(Int32.Parse(p2[iii]));
                                            nei[Int32.Parse(p2[iii])].Add(index);
                                        }
                                    }
                                }
                            }
                            else if (i == maxx && j != maxy)//---------------------最下的格子
                            {
                                string p1 = co[i, j + 1].ToString();
                                //string[] p2 = p1.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                List<string> p2 = new List<string>();
                                p2 = si.SplitString1(co[i, j + 1].ToString(), ';');
                                for (int iii = 0; iii < p2.Count; iii++)
                                {
                                    if (stlistin[Int32.Parse(p2[iii].ToString())].Type != stlistin[index].Type)
                                    {
                                        if (insexample.distance(x, y, stlistin[Int32.Parse(p2[iii].ToString())].x, stlistin[Int32.Parse(p2[iii].ToString())].y, d))//若邻近
                                        {
                                            nei[index].Add(Int32.Parse(p2[iii]));
                                            nei[Int32.Parse(p2[iii])].Add(index);
                                        }
                                    }
                                }
                            }
                        }//for循环拆分co[i,j]
                    }//----若co[i,j]不等于空
                }//co[i,j]循环         
            return nei;
        }

        public SortedList<int, Instance> instancelized(List<string> m)//所有的实例；读取文件之后对文件中的数据进行处理//这里的m已经经过m.sort排序
        {
            SortedList<int, Instance> listins = new SortedList<int, Instance>();
            StringInstance o = new StringInstance();
            for (int i = 1; i < m.Count-1; i++)
            {
                Instance ino = new Instance();
                List<string> si = new List<string>();
                si=o.SplitInstance(".", "(", ",", ")", m[i]);
                ino.Id = i;
                ino.Type = int.Parse(si[0]);
                ino.x = double.Parse(si[2].ToString());
                ino.y = double.Parse(si[3].ToString());
                listins.Add(i, ino);
            }
            return listins;
        }

        
    }
}
