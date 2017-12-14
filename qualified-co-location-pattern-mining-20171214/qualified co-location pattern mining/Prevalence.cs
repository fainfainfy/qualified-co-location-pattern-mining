using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using System.Collections.Generic;

namespace qualified_co_location_pattern_mining
{
    class Prevalence
    {
        SortedList<string, List<SortedSet<int>>> listT = new SortedList<string, List<SortedSet<int>>>();
        SortedList<string, List<SortedSet<int>>> listN = new SortedList<string, List<SortedSet<int>>>();
        //==============================================================================给定一阶模式为TypeInsList给定一阶T为实例邻居表INs

        //==================================================================生成二阶模式// 
        /*
        public SortedList<string, List<SortedSet<int>>>TwoSize(SortedList<int, SortedSet<int>> INs, double min_prev, List<int> TypeCountList, List<int> TypeinsList)
        {
            SortedList<string, List<SortedSet<int>>> T = new SortedList<string, List<SortedSet<int>>>();
            List<string> listT2 = new List<string>();
            List<double> listPI = new List<double>();
            List<int> listFI = new List<int>();
            listT2.Add("0");//存放形如i+j=的二阶
            listPI.Add(0);//存放i+j二阶是否频繁
            listFI.Add(0);//存放对于每个i其在队列中的位置，作用s22
            int feature = TypeCountList.Count;

            for (int i = 1; i < feature - 1; i++)//-----------------------新建二阶队列
            {

                //建立一个矩阵存放特征i的所有二阶邻居，行为特征i的所有实例个数，列为fi邻居特征个数
                List<List<SortedSet<int>>> matrixij = new List<List<SortedSet<int>>>();
                //初始化二阶矩阵
                
                for (int ii = 0; ii < TypeCountList[i] - TypeCountList[i - 1]; ii++)
                {
                    int jjmax = feature - i - 1;
                    List<SortedSet<int>> subm = new List<SortedSet<int>>();//存放fi的实例的某一特征的邻居实例
                    for (int jj = 0; jj < jjmax; jj++)
                    {
                        SortedSet<int> s = new SortedSet<int>() {};
                        subm.Add(s);
                    }
                    matrixij.Add(subm);
                }

                //赋值,得到特征i开头的所有需要测试的二阶的邻接矩阵
               
                for (int ii = 0; ii < TypeCountList[i] - TypeCountList[i - 1]; ii++)
                {
                    foreach (var item in INs[TypeCountList[i - 1]+ii +1])
                    {
                        if(TypeinsList[item]>i)//若实例TypeCountList[i - 1]+ii +1]的邻居中元素不等于他自己且该元素的特征号比i大
                        {
                            matrixij[ii][TypeinsList[item] - i - 1].Add(item);
                        }
                        
                    }                     
                    
                }
                //针对matrixij中的y列，即某一特征j，返回y列下的每一行x
                List<SortedSet<int>> inslist = new List<SortedSet<int>>();//存放每个与i特征实例临近的j实例，inslist[0]存放特征i+1与特征i临近的实例集合
                List<SortedSet<int>> listfi = new List<SortedSet<int>>();//存放含有j特征实例的i特征实例，listfi[0]存放特征i与特征i+1临近的实例集合
                
                for (int j = i + 1; j < feature; j++)//初始化含有listfi
                {
                    SortedSet<int> sett = new SortedSet<int>() {};
                    listfi.Add(sett);
                }
                for (int y = 0; y < feature-i-1 ; y++)//针对每一列该列的特征对应为i+y+1
                {
                    SortedSet<int> setj = new SortedSet<int>();
                    for (int x = 0; x < matrixij.Count; x++)
                    {
                        if (matrixij[x][y] !=null)
                        {
                            int ad = TypeCountList[i - 1] + x + 1;
                            listfi[y].Add(ad);//将i特征中参加在y中的实例放进去，x指的是i的第x+1个实例，对应其编号应该是typecount[i-1]+x+1
                            setj.UnionWith(matrixij[x][y]);//将y的实例也放在setj中
                        }
                    }
                    inslist.Add(setj); //inslist[0]存放的是第0列特征中的i特征集合      
                }

                List<double> pilist = new List<double>();
                pilist= GetPI(inslist,listfi,  TypeinsList,TypeCountList);//得到所有i+所有j特征的参与度
                SortedSet<int> extenedTypeset = new SortedSet<int>();//存储所有i特征能够扩展出频繁模式的特征集合
                //对比参与度并保存其表实例
                for (int ii = 0; ii < pilist.Count(); ii++)//对于每一个特征j
                {
                    
                    if (pilist[ii] > min_prev || pilist[ii] == min_prev)//若该i+j模式超过参与度阈值
                    {
                        List<SortedSet<int>> listrow = new List<SortedSet<int>>();                        
                        int j = i + ii + 1;
                        string h = i + "+" + j;
                        extenedTypeset.Add(j);//将能够发展频繁模式的特征加入集合
                        for (int x = 0; x < matrixij.Count; x++)//对i进行逐行统计
                        {
                            if (matrixij[x][ii]!= null)
                            {
                                
                                foreach (var item in matrixij[x][ii])//将i中同一条行实例中的多个j实例分开
                                {
                                    SortedSet<int> rowins = new SortedSet<int>();
                                    rowins.Add(TypeCountList[i-1]+1+x);
                                    rowins.Add(item);
                                    listrow.Add(rowins);//
                                }
                            }
                        }
                        //记录完毕表实例之后，listrow末尾附上参与度//将double为主截止为小数点后两位，之后乘以一百用整数保存
                        double ppi = Math.Round(pilist[ii], 2);
                        int intpi = int.Parse((ppi*100).ToString());
                        SortedSet<int> piset = new SortedSet<int>() { intpi };
                        listrow.Add(extenedTypeset);//行实例的倒数第二行存储发展特征
                        listrow.Add(piset);//行实例的最后一行存储PI值
                        T.Add(h, listrow);
                    }
                }
            }
           
            return T;
        }*/

        //==================================================================直接生成二阶模式的时候生成邻居，不再存储任何表实例
        public SortedList<string, List<SortedSet<int>>> TwoSize(SortedList<int, SortedSet<int>> INs, double min_prev, List<int> TypeCountList, List<int> TypeinsList)
        {
            SortedList<string, List<SortedSet<int>>> T = new SortedList<string, List<SortedSet<int>>>();
            List<string> listT2 = new List<string>();
            List<double> listPI = new List<double>();
            List<int> listFI = new List<int>();
            listT2.Add("0");//存放形如i+j=的二阶
            listPI.Add(0);//存放i+j二阶是否频繁
            listFI.Add(0);//存放对于每个i其在队列中的位置，作用s22
            int feature = TypeCountList.Count;

            for (int i = 1; i < feature - 1; i++)//-----------------------新建二阶队列
            {

                //建立一个矩阵存放特征i的所有二阶邻居，行为特征i的所有实例个数，列为fi邻居特征个数
                List<List<SortedSet<int>>> matrixij = new List<List<SortedSet<int>>>();
                //初始化二阶矩阵

                for (int ii = 0; ii < TypeCountList[i] - TypeCountList[i - 1]; ii++)
                {
                    int jjmax = feature - i - 1;
                    List<SortedSet<int>> subm = new List<SortedSet<int>>();//存放fi的实例的某一特征的邻居实例
                    for (int jj = 0; jj < jjmax; jj++)
                    {
                        SortedSet<int> s = new SortedSet<int>() { };
                        subm.Add(s);
                    }
                    matrixij.Add(subm);
                }

                //赋值,得到特征i开头的所有需要测试的二阶的邻接矩阵

                for (int ii = 0; ii < TypeCountList[i] - TypeCountList[i - 1]; ii++)
                {
                    foreach (var item in INs[TypeCountList[i - 1] + ii + 1])
                    {
                        if (TypeinsList[item] > i)//若实例TypeCountList[i - 1]+ii +1]的邻居中元素不等于他自己且该元素的特征号比i大
                        {
                            matrixij[ii][TypeinsList[item] - i - 1].Add(item);
                        }

                    }

                }
                //针对matrixij中的y列，即某一特征j，返回y列下的每一行x
                List<SortedSet<int>> inslist = new List<SortedSet<int>>();//存放每个与i特征实例临近的j实例，inslist[0]存放特征i+1与特征i临近的实例集合
                List<SortedSet<int>> listfi = new List<SortedSet<int>>();//存放含有j特征实例的i特征实例，listfi[0]存放特征i与特征i+1临近的实例集合

                for (int j = i + 1; j < feature; j++)//初始化含有listfi
                {
                    SortedSet<int> sett = new SortedSet<int>() { };
                    listfi.Add(sett);
                }
                for (int y = 0; y < feature - i - 1; y++)//针对每一列该列的特征对应为i+y+1
                {
                    SortedSet<int> setj = new SortedSet<int>();
                    for (int x = 0; x < matrixij.Count; x++)
                    {
                        if (matrixij[x][y] != null)
                        {
                            int ad = TypeCountList[i - 1] + x + 1;
                            listfi[y].Add(ad);//将i特征中参加在y中的实例放进去，x指的是i的第x+1个实例，对应其编号应该是typecount[i-1]+x+1
                            setj.UnionWith(matrixij[x][y]);//将y的实例也放在setj中
                        }
                    }
                    inslist.Add(setj); //inslist[0]存放的是第0列特征中的i特征集合      
                }

                List<double> pilist = new List<double>();
                pilist = GetPI(inslist, listfi, TypeinsList, TypeCountList);//得到所有i+所有j特征的参与度
                SortedSet<int> extenedTypeset = new SortedSet<int>();//存储所有i特征能够扩展出频繁模式的特征集合
                //对比参与度并保存其表实例
                for (int ii = 0; ii < pilist.Count(); ii++)//对于每一个特征j
                {

                    if (pilist[ii] > min_prev || pilist[ii] == min_prev)//若该i+j模式超过参与度阈值
                    {
                        List<SortedSet<int>> listrow = new List<SortedSet<int>>();
                        int j = i + ii + 1;
                        string h = i + "+" + j;
                        extenedTypeset.Add(j);//将能够发展频繁模式的特征加入集合
                        for (int x = 0; x < matrixij.Count; x++)//对i进行逐行统计
                        {
                            if (matrixij[x][ii] != null)
                            {

                                foreach (var item in matrixij[x][ii])//将i中同一条行实例中的多个j实例分开//在添加进行实例后可以继续添加行实例邻居 如{A1,B1}不单独保存，直接后续找邻居并保存为{A1，B1，C1，D1}
                                {
                                    SortedSet<int> rowins = new SortedSet<int>();
                                    rowins.Add(TypeCountList[i - 1] + 1 + x);//保存i实例
                                    rowins.Add(item);//保存j实例
                                    GetRowCN(rowins,INs);
                                    //SortedSet<int> snset = new SortedSet<int>(l.Cast<int>());
                                    listrow.Add(GetRowCN(rowins, INs));
                                }
                            }
                        }
                        //记录完毕表实例之后，listrow末尾附上参与度//将double为主截止为小数点后两位，之后乘以一百用整数保存
                        double ppi = Math.Round(pilist[ii], 2);
                        int intpi = int.Parse((ppi * 100).ToString());
                        SortedSet<int> piset = new SortedSet<int>() { intpi };
                        listrow.Add(extenedTypeset);//行实例的倒数第二行存储发展特征
                        listrow.Add(piset);//行实例的最后一行存储PI值
                        T.Add(h, listrow); 
                    }
                }
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
                            
                for (int i = 0; i < TS[h].Count()-2; i++)//最后两行存的set分别是扩展特征集和PI
                {
                    List<int> l = new List<int>();
                    l = INs[TS[h][i].First()].ToList();
                    foreach (var item1 in TS[h][i])
                    {
                        l = l.Intersect(INs[item1]).ToList();
                    }                                
                    SortedSet<int>snset = new SortedSet<int>(l.Cast<int>());
                    listh.Add(snset);
                }
                CN.Add(h, listh);
            }
            return CN;
        }
        public SortedSet<int> GetRowCN(SortedSet<int>rowset, SortedList<int, SortedSet<int>> INs)//生成行实例邻居
        {
            List<int> l = new List<int>();
            l = INs[rowset.First()].ToList();
            foreach (var item in rowset)
            {
                l = l.Intersect(INs[item]).ToList();
            }
                SortedSet<int> rowcn = new SortedSet<int>(l.Cast<int>());                
                return rowcn;
        }
        //===================================================================计算参与度
        public List<double> GetPI(List<SortedSet<int>> inslist, List<SortedSet<int>> listfi, List<int> typeins, List<int> typecount)
        {
            List<double> pilist = new List<double>();

            for (int iii = 0; iii < inslist.Count; iii++)
            {
                //首先计算fi的参与度
                int fi= typeins[listfi[iii].First()];
                double f1sum= double.Parse((typecount[fi] - typecount[fi - 1]).ToString());
                double pri = double.Parse(listfi[iii].Count().ToString()) / f1sum;
                int f2 = typeins[inslist[iii].First()];
                double fsum = double.Parse((typecount[f2] - typecount[f2 - 1]).ToString());
                double prj = double.Parse(inslist[iii].Count().ToString()) / fsum;                
                pilist.Add(Math.Min(pri, prj));
            }
            
            return pilist;
        }
        //计算多阶模式
        /*  public SortedList<string, List<SortedSet<int>>> MoreSize(SortedList<string, List<SortedSet<int>>> lastT, SortedList<string, List<SortedSet<int>>> CN,double min_prev, List<int> TypeCountList, List<int> TypeinsList)
          {
              SortedList<string, List<SortedSet<int>>> T = new SortedList<string, List<SortedSet<int>>>();
              //拿出上一阶模式进行扩展

              foreach (var patternitem in lastT)
              {
                  string pname = patternitem.Key;
                  //对于每一个上一阶模式，都对其进行扩展，其模式扩展集合可以确定模式扩展的特征，其邻居矩阵可以使得该模式测试其所有的下一阶
                  SortedSet<int> extendset = new SortedSet<int>();
                  extendset = lastT[pname][(lastT[pname].Count) - 2];//得到扩展特征有序集
                  //===========================================================测试频繁子阶
                  SortedSet<int> newextend = new SortedSet<int>();//再次用剪枝把可扩展特征删选一遍以获得模式的最小矩阵
                  int sign = 1;
                  StringInstance snew = new StringInstance();
                  foreach (var exitem in extendset)
                  {
                      string testpname = pname + "+" + exitem.ToString();                    
                      snew.SplitString1(testpname, '+');
                      foreach (var comb in Combinations(snew.SplitString1(testpname, '+'), 0, snew.SplitString1(testpname, '+').Count, snew.SplitString1(testpname, '+').Count - 1))//测试子模式,对于ABC（tlist）,从tlist[0]开始索引，得到tlist.Count个sizek - 1阶的组合
                      {
                          StringBuilder l = new StringBuilder();
                          string[] hcomb = comb.Take(snew.SplitString1(testpname, '+').Count - 1).ToArray();
                          l.Append(hcomb[0]);
                          for (int jj = 1; jj < comb.Take(snew.SplitString1(testpname, '+').Count - 1).Count(); jj++)
                          {
                              l.Append("+");
                              l.Append(hcomb[jj]);
                          }
                          l.ToString();
                          if (!lastT.ContainsKey(l.ToString()))
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
                  List<int> extendlist = new List<int>();
                  extendlist = newextend.ToList();//存放newextend矩阵列对应的特征号
                 //初始化listjj,其长度为扩展特征的个数
                  for (int ii = 0; ii < extendlist.Count; ii++)
                  {
                      listjj.Add(0);
                  }
                  SortedSet<int> unionCN = new SortedSet<int>() { };
                  for (int ii = 0; ii < CN[pname].Count; ii++)//得到所有的有序集合
                  {
                      unionCN.UnionWith(CN[pname][ii]);
                  }
                  foreach (var unitem in unionCN)//得到每个候选扩展特征的参与个数
                  {
                      if (newextend.Contains(TypeinsList[unitem]))
                      {
                          listjj[extendlist.IndexOf(TypeinsList[unitem])]++;
                      }
                  }
                  //如果不频繁直接将其从候选特征中删除
                  //新建真正的扩展特征集合
                  List<double> PIJlist = new List<double>();
                  List<int> realextendlist = new List<int>();

                  for (int ii = 0; ii < listjj.Count; ii++)
                  {                    
                      double f1sum = double.Parse((TypeCountList[extendlist[ii]] - TypeCountList[extendlist[ii] - 1]).ToString());
                      double pri = double.Parse(listjj[ii].ToString()) / f1sum;
                      if (pri > min_prev)
                      {                        
                          realextendlist.Add(extendlist[ii]);//得到扩展特征j
                          PIJlist.Add(pri);//得到j的参与度
                      }
                  }

                  //=======================================================使用真正的候选realextendset集合中的特征连接行实例并计算，列宽为newextend的秩，对应的值在CN中找
                  List<SortedSet<int>> listt = new List<SortedSet<int>>();//一条行实例单位
                  List<SortedSet<int>> listiinj = new List<SortedSet<int>>();//j个 特征分别参与在模式pname+j中的k-1阶的参与率最小值
                  for (int ii = 0; ii < realextendlist.Count; ii++)//初始化
                  {
                      SortedSet<int> rowset = new SortedSet<int>() { };
                      listiinj.Add(rowset);
                  }
                  for (int jj = 0; jj < realextendlist.Count; jj++)//对于每一个J特征
                  {
                      List<SortedSet<int>> listrowinj = new List<SortedSet<int>>();//初始化,长度为lastT的阶
                      for (int ii = 0; ii < snew.SplitString1(pname, '+').Count; ii++)
                      {
                          SortedSet<int> newset = new SortedSet<int>() {};
                          listrowinj.Add(newset);
                      }
                      //针对每一列，建立包含该特征实例的行实例表
                      SortedSet<int> rowset = new SortedSet<int>();
                      for (int ii = 0; ii < CN[pname].Count; ii++)//针对每一行遍历有j的lastT中的行实例
                      {
                          if (CN[pname][ii].Contains(realextendlist[jj]))//如果该行含有特征j
                          {
                              rowset.Add(ii);
                              foreach (var item in CN[pname][ii])
                              {
                                  if (TypeinsList[item] == realextendlist[jj])//构建行实例
                                  {
                                      SortedSet<int> newrowset = new SortedSet<int>() { item };
                                      newrowset.UnionWith(lastT[pname][ii]);
                                      listt.Add(newrowset);
                                      //记录pname中行实例的特征投影                                      
                                  }                                
                              }
                              SortedSet<int> takeinsset = new SortedSet<int>();
                              takeinsset=lastT[pname][ii];//按序取出行实例中的前k-1个则为原行实例
                              for (int iii = 0; iii < takeinsset.Count; iii++)//
                              {
                                  listrowinj[iii].Add(takeinsset.ToList()[iii]);//存放了所有参与在T中的k-1阶特征的实例投影，及ABC的话，listrowinj.count=3,listrowinj[0]放A在ABCD中的实例集合
                              }                            

                          }

                      }

                      //得到j特征的listrowinj之后要对其进行计算
                      List<double> PIIlist = new List<double>();//ABC的参与率                    

                      for (int iii = 0; iii < listrowinj.Count; iii++)
                      {
                          List<string> plist = new List<string>();
                          plist = snew.SplitString1(pname, '+');
                          double f1sum = double.Parse((TypeCountList[int.Parse(plist[iii])] - TypeCountList[int.Parse(plist[iii]) - 1]).ToString());
                          double pri = double.Parse(listrowinj[iii].ToString()) / f1sum;
                          PIIlist.Add(pri);
                      }
                      PIIlist.Add(PIJlist[jj]);//得到pname+j的所有特征参与率
                      if (PIIlist.Min() > min_prev)
                      {
                          string newpname = pname + "+" + realextendlist[jj];
                          T.Add(newpname,listt);
                      }

                  }


              }


              return T;
          }
          */
           public SortedList<string, List<SortedSet<int>>> MoreSize(SortedList<string, List<SortedSet<int>>> LastCN, double min_prev, List<int> TypeCountList, List<int> TypeinsList)
           {
               SortedList<string, List<SortedSet<int>>> T = new SortedList<string, List<SortedSet<int>>>();
               //拿出上一阶模式进行扩展

               foreach (var patternitem in LastCN)
               {
                   string pname = patternitem.Key;
                   //对于每一个上一阶模式，都对其进行扩展，其模式扩展集合可以确定模式扩展的特征，其邻居矩阵可以使得该模式测试其所有的下一阶
                   SortedSet<int> extendset = new SortedSet<int>();
                   extendset = LastCN[pname][(LastCN.Count) - 2];//得到扩展特征有序集
                   //===========================================================测试频繁子阶
                   SortedSet<int> newextend = new SortedSet<int>();//再次用剪枝把可扩展特征删选一遍以获得模式的最小矩阵
                   int sign = 1;
                   StringInstance snew = new StringInstance();
                   foreach (var exitem in extendset)
                   {
                       string testpname = pname + "+" + exitem.ToString();
                       snew.SplitString1(testpname, '+');
                       foreach (var comb in Combinations(snew.SplitString1(testpname, '+'), 0, snew.SplitString1(testpname, '+').Count, snew.SplitString1(testpname, '+').Count - 1))//测试子模式,对于ABC（tlist）,从tlist[0]开始索引，得到tlist.Count个sizek - 1阶的组合
                       {
                           StringBuilder l = new StringBuilder();
                           string[] hcomb = comb.Take(snew.SplitString1(testpname, '+').Count - 1).ToArray();
                           l.Append(hcomb[0]);
                           for (int jj = 1; jj < comb.Take(snew.SplitString1(testpname, '+').Count - 1).Count(); jj++)
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
                   List<int> extendlist = new List<int>();
                   extendlist = newextend.ToList();//存放newextend矩阵列对应的特征号
                                                   //初始化listjj,其长度为扩展特征的个数
                   for (int ii = 0; ii < extendlist.Count; ii++)
                   {
                       listjj.Add(0);
                   }
                   SortedSet<int> unionCN = new SortedSet<int>() { };
                   for (int ii = 0; ii < LastCN[pname].Count-2; ii++)//得到所有的有序集合
                   {
                       unionCN.UnionWith(LastCN[pname][ii]);
                   }
                   foreach (var unitem in unionCN)//得到每个候选扩展特征的参与个数
                   {
                       if (newextend.Contains(TypeinsList[unitem]))
                       {
                           listjj[extendlist.IndexOf(TypeinsList[unitem])]++;
                       }
                   }
                   //如果不频繁直接将其从候选特征中删除
                   //新建真正的扩展特征集合
                   List<double> PIJlist = new List<double>();
                   List<int> realextendlist = new List<int>();

                   for (int ii = 0; ii < listjj.Count; ii++)
                   {
                       double f1sum = double.Parse((TypeCountList[extendlist[ii]] - TypeCountList[extendlist[ii] - 1]).ToString());
                       double pri = double.Parse(listjj[ii].ToString()) / f1sum;
                       if (pri > min_prev)
                       {
                           realextendlist.Add(extendlist[ii]);//得到扩展特征j
                           PIJlist.Add(pri);//得到j的参与度
                       }
                   }

                   //=======================================================使用真正的候选realextendset集合中的特征连接行实例并计算，列宽为newextend的秩，对应的值在CN中找
                   List<SortedSet<int>> listt = new List<SortedSet<int>>();//一条行实例单位
                   List<SortedSet<int>> listiinj = new List<SortedSet<int>>();//j个 特征分别参与在模式pname+j中的k-1阶的参与率最小值
                   for (int ii = 0; ii < realextendlist.Count; ii++)//初始化
                   {
                       SortedSet<int> rowset = new SortedSet<int>() { };
                       listiinj.Add(rowset);
                   }
                   for (int jj = 0; jj < realextendlist.Count; jj++)//对于每一个J特征
                   {
                       List<SortedSet<int>> listrowinj = new List<SortedSet<int>>();//初始化,长度为lastT的阶
                       for (int ii = 0; ii < snew.SplitString1(pname, '+').Count; ii++)
                       {
                           SortedSet<int> newset = new SortedSet<int>() { };
                           listrowinj.Add(newset);
                       }
                       //针对每一列，建立包含该特征实例的行实例表
                       SortedSet<int> rowset = new SortedSet<int>();
                       for (int ii = 0; ii < LastCN[pname].Count-2; ii++)//针对每一行遍历有j的lastT中的行实例
                       {
                           if (LastCN[pname][ii].Contains(realextendlist[jj]))//如果该行含有特征j
                           {
                               SortedSet<int> takeinsset = new SortedSet<int>();//按序取出行实例邻居中的前k-1个则为原行实例
                               takeinsset = LastCN[pname][ii];//索引到行         
                               for (int iii = 0; iii < snew.SplitString1(pname, '+').Count(); iii++)//
                               {
                                   listrowinj[iii].Add(takeinsset.ToList()[iii]);//存放了所有参与在T中的k-1阶特征的实例投影，及ABC的话，listrowinj.count=3,listrowinj[0]放A在ABCD中的实例集合
                               }
                               rowset.Add(ii);
                               foreach (var item in LastCN[pname][ii])
                               {
                                   if (TypeinsList[item] == realextendlist[jj])//构建行实例邻居
                                   {
                                       SortedSet<int> newrowset = new SortedSet<int>() { item };
                                       newrowset.UnionWith(LastCN[pname][ii]);//======================================
                                       listt.Add(newrowset);
                                       //记录pname中行实例的特征投影                                      
                                   }
                               }
                               

                           }

                       }

                       //得到j特征的listrowinj之后要对其进行计算
                       List<double> PIIlist = new List<double>();//ABC的参与率                    

                       for (int iii = 0; iii < listrowinj.Count; iii++)
                       {
                           List<string> plist = new List<string>();
                           plist = snew.SplitString1(pname, '+');
                           double f1sum = double.Parse((TypeCountList[int.Parse(plist[iii])] - TypeCountList[int.Parse(plist[iii]) - 1]).ToString());
                           double pri = double.Parse(listrowinj[iii].ToString()) / f1sum;
                           PIIlist.Add(pri);
                       }
                       PIIlist.Add(PIJlist[jj]);//得到pname+j的所有特征参与率
                       if (PIIlist.Min() > min_prev)
                       {
                           string newpname = pname + "+" + realextendlist[jj];
                           T.Add(newpname, listt);
                       }

                   }


               }


               return T;
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
