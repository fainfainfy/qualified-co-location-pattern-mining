using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qualified_co_location_pattern_mining
{
    class Skyline
    {
        SortedList<string, List<SortedSet<int>>> listT = new SortedList<string, List<SortedSet<int>>>();
        SortedList<string, List<SortedSet<int>>> listN = new SortedList<string, List<SortedSet<int>>>();
        SortedList<string, SortedSet<int>> listExtend = new SortedList<string, SortedSet<int>>();

        //==============================================================================给定一阶模式为TypeInsList给定一阶T为实例邻居表INs
        //==================================================================直接生成二阶模式的时候生成邻居，不再存储任何表实例
        public SortedList<string, List<List<int>>> TwoSize(SortedList<int, SortedSet<int>> INs, double min_prev, List<int> TypeCountList, List<int> TypeinsList)
        {
            SortedList<string, List<List<int>>> T = new SortedList<string, List<List<int>>>();
            List<string> listT2 = new List<string>();
            List<double> listPI = new List<double>();
            List<int> listFI = new List<int>();
            listT2.Add("0");//存放形如i+j=的二阶
            listPI.Add(0);//存放i+j二阶是否频繁
            listFI.Add(0);//存放对于每个i其在队列中的位置，作用s22
            int feature = TypeCountList.Count;
            //首先插入一个list<list<int>>来获得所有模式的extendlist，其与T中有序的模式一一对应
            List<List<int>> extendlist = new List<List<int>>();//extendlist[0]中放1+2模式的extendetypes

            for (int i = 1; i < feature - 1; i++)//-----------------------新建二阶队列,对于每一个i+
            {

                //建立一个矩阵存放特征i的所有二阶邻居，行为特征i的所有实例个数，列为fi邻居特征个数
                //List<List<SortedSet<int>>> matrixij = new List<List<SortedSet<int>>>();
                List<List<List<int>>> listmatrixij = new List<List<List<int>>>();

                //初始化二阶矩阵

                for (int ii = 0; ii < TypeCountList[i] - TypeCountList[i - 1]; ii++)
                {
                    int jjmax = feature - i - 1;
                    List<List<int>> subm = new List<List<int>>();//存放fi的实例的某一特征的邻居实例
                    for (int jj = 0; jj < jjmax; jj++)
                    {
                        List<int> s = new List<int>() { };
                        subm.Add(s);
                    }
                    listmatrixij.Add(subm);
                }

                //赋值,得到特征i开头的所有需要测试的二阶的邻接矩阵

                for (int ii = 0; ii < TypeCountList[i] - TypeCountList[i - 1]; ii++)
                {
                    foreach (var item in INs[TypeCountList[i - 1] + ii + 1])//对fi中每一个实例的邻居来说
                    {
                        if (TypeinsList[item] > i)//

                        {
                            listmatrixij[ii][TypeinsList[item] - i - 1].Add(item);
                        }

                    }

                }
                //针对matrixij中的y列，即某一特征j，返回y列下的每一行x
                List<SortedSet<int>> inslist = new List<SortedSet<int>>();//存放每个与i特征实例临近的j实例，inslist[0]存放特征i+1与特征i临近的实例集合
                List<SortedSet<int>> listfi = new List<SortedSet<int>>();//存放含有j特征实例的i特征实例，listfi[0]存放特征i与特征i+1临近的实例集合
                //初始化含有listfi
                for (int j = i + 1; j < feature; j++)
                {
                    SortedSet<int> sett = new SortedSet<int>() { };
                    listfi.Add(sett);
                }
                //针对每一列该列的特征对应为i+y+1
                for (int y = 0; y < feature - i - 1; y++)
                {
                    SortedSet<int> setj = new SortedSet<int>();
                    for (int x = 0; x < listmatrixij.Count; x++)
                    {
                        if (listmatrixij[x][y].Count != 0)
                        {
                            int ad = TypeCountList[i - 1] + x + 1;
                            listfi[y].Add(ad);//将i特征中参加在y中的实例放进去，x指的是i的第x+1个实例，对应其编号应该是typecount[i-1]+x+1
                            setj.UnionWith(listmatrixij[x][y]);//将y的实例也放在setj中
                        }
                    }
                    inslist.Add(setj); //inslist[0]存放的是第0列特征中的i特征集合      
                }

                List<double> pilist = new List<double>();
                pilist = GetPI(inslist, listfi, TypeinsList, TypeCountList);//得到所有i+所有j特征的参与率list,如ABC的参与率
                                                                            //还需要计算i特征分别与pilist中不同特征的参与率
                List<int> extenedTypeset = new List<int>();//存储所有i特征能够扩展出频繁模式的特征集合
                extenedTypeset.Add(i);//将能够发展频繁模式的特征加入集合例如：i=A的时候，先加入A
                //对比参与度并保存其表实例
                int extendflag = 0;//频繁模式个数标志位
                for (int ii = 0; ii < pilist.Count(); ii++)//对于每一个特征j
                {

                    if (pilist[ii] > min_prev || pilist[ii] == min_prev)//若该i+j模式超过参与度阈值
                    {
                        List<List<int>> listrow = new List<List<int>>();
                        int j = i + ii + 1;
                        string h = i + "+" + j;
                        extenedTypeset.Add(j);//将能够发展频繁模式的特征加入集合。实际是存储与i频繁的所有特征，与其结合才能得到下一阶频繁
                        //i =A的时候，若AB频繁，那么A,B都加入，若C,D也与A频繁，那么AB的扩展就是{A,B,C,D}，在扩展AB到下一阶时，将{A,B,C,D}.except{A,B}就能得到扩展特征
                        for (int x = 0; x < listmatrixij.Count; x++)//对i进行逐行统计
                        {
                            if (listmatrixij[x][ii] != null)
                            {

                                foreach (var item in listmatrixij[x][ii])//将i中同一条行实例中的多个j实例分开//在添加进行实例后可以继续添加行实例邻居 如{A1,B1}不单独保存，直接后续找邻居并保存为{A1，B1，C1，D1}
                                {
                                    // SortedSet<int> rowins = new SortedSet<int>();
                                    List<int> rowins = new List<int>();
                                    rowins.Add(TypeCountList[i - 1] + 1 + x);//保存i实例
                                    rowins.Add(item);//保存j实例
                                    //GetRowCN(rowins,INs);
                                    //SortedSet<int> snset = new SortedSet<int>(l.Cast<int>());
                                    listrow.Add(GetRowCN(rowins, INs));
                                }
                            }
                        }
                        //记录完毕表实例之后，listrow末尾附上参与度//将double为主截止为小数点后两位，之后乘以一百用整数保存
                        double ppi = Math.Round(pilist[ii], 2);
                        int intpi = int.Parse((ppi * 100).ToString());
                        //SortedSet<int> piset = new SortedSet<int>() { intpi };
                        List<int> pilistset = new List<int>() { intpi };
                        //listrow.Add(extenedTypeset);//行实例的倒数第二行存储发展特征
                        listrow.Add(pilistset);//行实例的最后一行存储PI值
                        T.Add(h, listrow);
                        extendflag++;
                    }
                }//测试每一个i+j结尾
                //测试完毕所有的i+j之后可以得到所有与i频繁的所有特征，与其结合才能得到下一阶频繁的extenedTypeset
                //对于每一个i开头的二阶模式，保留一个T作为extendset
                for (int ii = 0; ii < extendflag; ii++)
                {
                    extendlist.Add(extenedTypeset.ToList());
                }
            }//测试i开头的模式结尾，将所有的频繁模式加入T
            T.Add("extend", extendlist);
            return T;

        }


        //计算多阶模式

        public SortedList<string, List<List<int>>> MoreSize1(SortedList<string, List<List<int>>> LastCN, SortedList<int, SortedSet<int>> INs, double min_prev, List<int> TypeCountList, List<int> TypeinsList)
        {
            SortedList<string, List<List<int>>> T = new SortedList<string, List<List<int>>>();//拿出上一阶模式进行扩展 
            int k = 0;
            foreach (var patternitem in LastCN)
            {
                if (patternitem.Key.Contains("extend")) { break; }
                string pname = patternitem.Key;
                var tem = WeightPrevalenceT(pname, k, LastCN, INs, min_prev, TypeCountList, TypeinsList);
                k++;
                for (int i = 0; i < tem.Count - 1; i++)
                {
                    T.Add(tem.ElementAt(i).Key, tem.ElementAt(i).Value);
                }
            }
            return T;
        }
        //得到某个k-1阶模式扩展到k阶的频繁模式

        public SortedList<string, List<List<int>>> WeightPrevalenceT(string pname, int index, SortedList<string, List<List<int>>> LastCN, SortedList<int, SortedSet<int>> INs, double min_prev, List<int> TypeCountList, List<int> TypeinsList)
        {
            Occupation classoccupation = new Occupation();
            SortedList<string, List<List<int>>> T = new SortedList<string, List<List<int>>>();
            List<List<int>> li = new List<List<int>>() { };
            T.Add("extend", li);
            int extendflag = 0;//记录得到多少个频繁模式
            #region
            //===========================================================测试频繁子阶                  
            StringInstance snew = new StringInstance();
            //对于每一个上一阶模式，都对其进行扩展，其模式扩展集合可以确定模式扩展的特征，其邻居矩阵可以使得该模式测试其所有的下一阶
            //SortedSet<int> extendset = new SortedSet<int>();
            var extendlist1 = LastCN["extend"][index];
            // extendlist1 = LastCN[snew.SplitString1(pname, '+')[0] + "+extend"][0].ToList();
            var extendlist2 = snew.SplitString1(pname, '+').Select<string, int>(x => Convert.ToInt32(x));//将扩展特征中属于自身模式的特征去掉
            var extendlist3 = extendlist1.Except(extendlist2);
            List<string> pnamelist1 = snew.SplitString1(pname, '+');
            List<int> pnamelist = new List<int>();

            for (int ii = 0; ii < pnamelist1.ToList().Count; ii++)
            {
                pnamelist.Add(int.Parse(pnamelist1.ToList()[ii]));
            }


            SortedSet<int> newextend = new SortedSet<int>();//====================再次用剪枝把可扩展特征删选一遍以获得模式的最小扩展
            int sign = 1;
            foreach (var exitem in extendlist3)//针对每一个扩展模式
            {
                string testpname = pname + "+" + exitem.ToString();
                var testnamelist = snew.SplitString1(testpname, '+');
                foreach (var comb in Combinations(testnamelist, 0, testnamelist.Count, testnamelist.Count - 1))//测试子模式,对于ABC（tlist）,从tlist[0]开始索引，得到tlist.Count个sizek - 1阶的组合
                {
                    StringBuilder l = new StringBuilder();
                    string[] hcomb = comb.Take(testnamelist.Count - 1).ToArray();
                    l.Append(hcomb[0]);
                    for (int jj = 1; jj < comb.Take(testnamelist.Count - 1).Count(); jj++)
                    {
                        l.Append("+");
                        l.Append(hcomb[jj]);
                    }
                    l.ToString();
                    if (!LastCN.ContainsKey(l.ToString()))
                    {
                        sign = 0;
                        break;
                    }//剪枝步
                }
                if (sign == 1)//若该模式频繁
                {
                    newextend.Add(exitem);          //再次用剪枝把可扩展特征删选一遍以获得模式的最小矩阵
                }
            }

            //扩展特征再进行筛选,去掉参与度不够的特征
            List<int> listjj = new List<int>();//存放第j个特征参与在pname+j表实例中的个数
            List<int> newextend1 = new List<int>();
            newextend1 = newextend.ToList();//存放newextend矩阵列对应的特征号
                                            //初始化listjj,其长度为扩展特征的个数
            for (int ii = 0; ii < newextend1.Count; ii++)
            {
                listjj.Add(0);
            }
            SortedSet<int> unionCN = new SortedSet<int>() { };
            for (int ii = 0; ii < LastCN[pname].Count - 1; ii++)//得到所有的有序集合
            {
                unionCN.UnionWith(LastCN[pname][ii]);
            }
            foreach (var unitem in unionCN)//得到每个候选扩展特征的参与个数
            {
                if (newextend.Contains(TypeinsList[unitem]))
                {
                    listjj[newextend1.IndexOf(TypeinsList[unitem])]++;
                }
            }
            //如果不频繁直接将其从候选特征中删除
            //新建真正的扩展特征集合
            List<double> PIJlist = new List<double>();
            List<int> realextendlist = new List<int>();

            for (int ii = 0; ii < listjj.Count; ii++)
            {
                double f1sum = double.Parse((TypeCountList[newextend1[ii]] - TypeCountList[newextend1[ii] - 1]).ToString());
                double pri = double.Parse(listjj[ii].ToString()) / f1sum;
                if (pri > min_prev)
                {
                    realextendlist.Add(newextend1[ii]);//得到扩展特征j
                    PIJlist.Add(pri);//得到j的参与度
                }
            }

            //=======================================================使用真正的候选realextendset集合中的特征连接行实例并计算，列宽为newextend的秩，对应的值在CN中找
            List<List<int>> listt = new List<List<int>>();//一条行实例单位
            List<SortedSet<int>> listiinj = new List<SortedSet<int>>();//j个 特征分别参与在模式pname+j中的k-1阶的参与率最小值
            List<int> extendset = new List<int>();//开始计算以pname为头的模式的扩展特征
            for (int ii = 0; ii < realextendlist.Count; ii++)//初始化
            {
                SortedSet<int> rowset = new SortedSet<int>() { };
                listiinj.Add(rowset);
            }
            for (int jj = 0; jj < realextendlist.Count; jj++)//对于每一个J特征
            {
                List<SortedSet<int>> listrowinj = new List<SortedSet<int>>();//初始化,长度为上一个阶lastT的阶,pname=ABC的时候，listrowij[0]=Aset,即A的所有参加在ABCD中的实例的集合
                for (int ii = 0; ii < snew.SplitString1(pname, '+').Count; ii++)
                {
                    SortedSet<int> newset = new SortedSet<int>() { };
                    listrowinj.Add(newset);//用来计算参与率
                }
                //针对每一列，建立包含该特征实例的行实例表
                List<int> rowset = new List<int>();
                for (int ii = 0; ii < LastCN[pname].Count - 1; ii++)//针对每一行遍历有j的lastT中的行实例
                {
                    //==============================================这里是一个剪纸，如果rowset的秩/pname中实例总数max的那个<prev,那么pname+j不用计算就丢掉了
                    foreach (var item in LastCN[pname][ii])
                    {
                        if (TypeinsList[item] == realextendlist[jj])//如果该行含有特征j
                        {
                            rowset.Add(ii);//收集参加了含有特征j的pname模式的行实例的行号
                        }
                    }

                }
                //用行实例行数剪纸
                List<int> pnametypecount = new List<int>();

                for (int ii = 0; ii < pnamelist.Count(); ii++)
                {
                    pnametypecount.Add(TypeCountList[pnamelist.ToList()[ii]] - TypeCountList[pnamelist.ToList()[ii] - 1]);
                }
                double pirow = double.Parse(rowset.Count().ToString()) / double.Parse(pnametypecount.Max().ToString());

                if (pirow > min_prev || pirow == min_prev)//用行实例行数剪纸条件下
                {

                    //求得行事例邻居表
                    for (int ii = 0; ii < rowset.Count; ii++)//对于每一行
                    {
                        List<int> takepnameins = new List<int>();
                        for (int iii = 0; iii < pnamelist.Count(); iii++)//
                        {
                            listrowinj[iii].Add(LastCN[pname][rowset.ToList()[ii]][iii]);//存放了所有参与在T中的k-1阶特征的实例投影，及ABC的话，listrowinj.count=3,listrowinj[0]放A在ABCD中的实例集合
                            //takepnameins.Add(LastCN[pname][ii][iii]);//存放ii行的pname的行实例
                        }//每一列
                        takepnameins.AddRange(LastCN[pname][rowset.ToList()[ii]].Take(pnamelist.Count()));
                        listt.Add(GetRowCN(takepnameins, INs));

                    }
                    //得到j特征的listrowinj之后要对其进行计算
                    List<double> PIIlist = new List<double>();//ABC的参与率                    
                    //求得testpname参与率
                    for (int iii = 0; iii < listrowinj.Count; iii++)//计算 listrowinj每一个特征在其中的参与率，即pname在testpname中的特征参与率
                    {
                        List<int> plist = new List<int>();
                        plist = pnamelist.ToList();//拆分上一阶pname模式的所有特征
                        double f1sum = double.Parse((TypeCountList[plist[iii]] - TypeCountList[plist[iii] - 1]).ToString());
                        double pri = double.Parse(listrowinj[iii].Count.ToString()) / f1sum;
                        PIIlist.Add(Math.Round(pri, 2));//对每一个上一阶模式特征分别计算参与率

                    }
                    //PIIlist.Add(Math.Round(PIJlist[jj], 2));//得到pname+j的所有特征参与率
                    List<int> PIIset = new List<int>();
                    //for (int iii = 0; iii < PIIlist.Count; iii++)
                    //{
                    //    PIIset.Add(int.Parse((PIIlist[iii] * 100.00).ToString()));//===============这里其实也可以存放所有特征的pr
                    //}
                    PIIset.Add(int.Parse((PIIlist.Min() * 100.00).ToString()));
                    #endregion
                    //================================================================================开始计算所有的值
                    if (PIIlist.Min() > min_prev)
                    {
                        string newpname = pname + "+" + realextendlist[jj];
                        extendset.Add(realextendlist[jj]);
                        List<SortedSet<int>> palist = classoccupation.PA(PIIlist.Count,listt);
                        List<SortedSet<int>> calist = classoccupation.CA(palist[palist.Count - 1], listt);
                        double OI = Math.Round(classoccupation.OccupationIndex(palist, calist), 2);
                       
                         
                        double PI = PIIlist.Min();
                        List<int> resultset = new List<int>();
                        resultset.Add(int.Parse((PI * 100.00).ToString()));
                        resultset.Add(int.Parse((OI * 100.00).ToString()));
                         
                        listt.Add(PIIset);//末尾加上参与度
                        T.Add(newpname, listt);
                        extendflag++;
                    }
                    //=======================================满足参与度之后看是否满足占有率

                }//计算每个pname+jj模式结尾

                // 测试完毕所有的pname + j之后可以得到所有与i频繁的所有特征，与其结合才能得到下一阶频繁的extenedTypeset
                //对于每一个i开头的二阶模式，保留一个T作为extendset
            }
            for (int ii = 0; ii < extendflag; ii++)
            {
                T["extend"].Add(extendset);
            }

            return T;
        }
        //==================================================================生成模式邻居
        public SortedList<string, List<SortedSet<int>>> CommonNeighbor(SortedList<string, List<SortedSet<int>>> TS, SortedList<int, SortedSet<int>> INs)
        {
            SortedList<string, List<SortedSet<int>>> CN = new SortedList<string, List<SortedSet<int>>>();
            foreach (var item in TS)
            {
                string h = item.Key;
                List<SortedSet<int>> listh = new List<SortedSet<int>>();

                for (int i = 0; i < TS[h].Count() - 2; i++)//最后两行存的set分别是扩展特征集和PI
                {
                    List<int> l = new List<int>();
                    l = INs[TS[h][i].First()].ToList();
                    foreach (var item1 in TS[h][i])
                    {
                        l = l.Intersect(INs[item1]).ToList();
                    }
                    SortedSet<int> snset = new SortedSet<int>(l.Cast<int>());
                    listh.Add(snset);
                }
                CN.Add(h, listh);
            }
            return CN;
        }
        public List<int> GetRowCN(List<int> rowset, SortedList<int, SortedSet<int>> INs)//生成行实例邻居
        {
            List<int> l = new List<int>();
            foreach (var item in rowset)//先将行实例放在前面
            {
                l.Add(item);
            }
            List<int> l1 = new List<int>();
            l1 = INs[rowset.First()].ToList();//取邻居交集
            foreach (var item in rowset)
            {
                l1 = l1.Intersect(INs[item]).ToList();
            }
            SortedSet<int> rowcn = new SortedSet<int>(l1.Cast<int>());
            rowcn.ExceptWith(rowset);//去掉邻居交集中的行事例的部分
            l.AddRange(rowcn.ToList());//将该部分追加在行事例后面
            return l;
        }
        //===================================================================计算参与度
        public List<double> GetPI(List<SortedSet<int>> inslist, List<SortedSet<int>> listfi, List<int> typeins, List<int> typecount)
        {
            List<double> pilist = new List<double>();

            for (int iii = 0; iii < inslist.Count; iii++)
            {
                //首先计算fi的参与度
                int fi = typeins[listfi[iii].First()];
                double f1sum = double.Parse((typecount[fi] - typecount[fi - 1]).ToString());
                double pri = double.Parse(listfi[iii].Count().ToString()) / f1sum;
                int f2 = typeins[inslist[iii].First()];
                double fsum = double.Parse((typecount[f2] - typecount[f2 - 1]).ToString());
                double prj = double.Parse(inslist[iii].Count().ToString()) / fsum;
                pilist.Add(Math.Min(pri, prj));
            }

            return pilist;
        }
        public static IEnumerable<List<string>> Combinations(List<string> sq, int i0, int n, int c)//将特征组合成候选模式
        {
            if (c == 0) yield return sq;
            else
            {
                for (int i = 0; i < n; i++)
                {
                    foreach (var perm in Combinations(sq, i0 + 1, n - 1 - i, c - 1))
                        yield return perm;
                    RL(sq, i0, n); //
                }
            }
        }
        private static void RL(List<string> sq, int i0, int n)
        {
            string tmp = sq[i0];
            sq.RemoveAt(i0);
            sq.Insert(i0 + n - 1, tmp);
        }
    }
}

