using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qualified_co_location_pattern_mining
{
    class Occupation
    {
 
        public List<SortedSet<int>> PA(int k,List<List<int>> list)//输入实例表，划分产生PA的集合//注意在产生的实例表最后一行必须再加上一行空集，以保证行号可以完全输出
        {
            //人为添加空集
            List<int> temlist1 = new List<int>() {};
            list.Add(temlist1);
            List<SortedSet<int>> listPA = new List<SortedSet<int>>();//装PA包括的实例
            SortedSet<int> listline = new SortedSet<int>();//装每个PA包括的行号
            SortedSet<int> templist = new SortedSet<int>();
            for(int i=0;i < k; i++)
            {
                templist.Add(list[0][i]);
            }
            listPA.Add(templist);//PA初始化
            listline.Add(0);
            for (int i = 1; i < list.Count(); i++)//将实例表分到不同的PA中i
            {
                int j = listPA.Count - 1;
                SortedSet<int> line = new SortedSet<int>();
                if (listPA[j].Overlaps(list[i].Take(k)))
                {
                    listPA[j].UnionWith(list[i].Take(k));
                }
                else
                {
                    if (list[i].Count != 0)
                    {
                        SortedSet<int> temp1 = new SortedSet<int>();
                        for (int ii = 0; ii < k; ii++)
                        {
                            temp1.Add(list[i][ii]);
                        }
                        listPA.Add(temp1);
                    }
                    listline.Add(i);
                }
            }
            //if (listPA.Last().Count == 0)
           // { listPA.RemoveAt(listPA.Count-1); }
            listPA.Add(listline);
            return listPA;
        }
         

        public List<SortedSet<int>> CA(SortedSet<int> listline, List<List<int>> listcn)//输入表实例行号以及表实例邻居，产生对应PA的CN的集合
        {
            List<SortedSet<int>> listCA = new List<SortedSet<int>>();//装PA包括的实例
            List<int> line = new List<int>();

            foreach (var item in listline)//取出listline
            {
                line.Add(item);
            }
            for (int i = 0; i < line.Count() - 1; i++)
            {
                 
                SortedSet<int> tmpset = new SortedSet<int>();
                for (int ii = 0; ii < listcn[line[i]].Count; ii++)
                {
                    tmpset.Add(listcn[line[i]][ii]);
                }
                for (int j = line[i] + 1; j < line[i + 1]; j++)
                {
                    tmpset.UnionWith(listcn[j]);
                }
                if (tmpset.Count != 0)
                { listCA.Add(tmpset); }
            }

            return listCA;
        }
        //计算occupantionIndex
        public double OccupationIndex(List<SortedSet<int>> listpa, List<SortedSet<int>> listca)
        {
            double index = 0.00;
            double sum = 0.00;
            for (int i = 0; i < listca.Count(); i++)
            {
                sum += (double.Parse(listpa[i].Count.ToString()) / double.Parse(listca[i].Count.ToString()));
            }
            index = sum / double.Parse(listca.Count().ToString());
                return index;
        }

    }
}
