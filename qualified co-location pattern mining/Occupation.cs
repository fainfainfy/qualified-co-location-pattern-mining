using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qualified_co_location_pattern_mining
{
    class Occupation
    {
        public List<SortedSet<int>>PA(List<SortedSet<int>>list)//输入实例表，划分产生PA的集合//注意在产生的实例表最后一行必须再加上一行空集，以保证行号可以完全输出
        {
            List<SortedSet<int>> listPA = new List<SortedSet<int>>();//装PA包括的实例
            SortedSet<int> listline = new  SortedSet<int>();//装每个PA包括的行号
            listPA.Add(list[0]);//PA初始化
            listline.Add(0);
            for (int i = 1; i < list.Count(); i++)//将实例表分到不同的PA中
            {
                int j = listPA.Count - 1;
                SortedSet<int> line = new SortedSet<int>();
                if (listPA[j].Overlaps(list[i]))
                {
                    listPA[j].UnionWith(list[i]);
                }
                else
                {
                    listPA.Add(list[i]);
                    listline.Add(i);
                }
            }
            listPA.Add(listline);
            return listPA;
        }

        //得到相应的PA邻居CN
        public List<SortedSet<int>> CA(SortedSet<int> listline,List<SortedSet<int>> listcn)//输入表实例行号以及表实例邻居，产生对应PA的CN的集合
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
                tmpset = listcn[line[i]];
                for (int j = line[i] + 1; j < line[i + 1]; j++)
                {
                    tmpset.UnionWith(listcn[j]);
                }
                listCA.Add(tmpset);
            }
                       
            return listCA;
        }
        //计算occupantionIndex
        public double OccupationIndex(List<SortedSet<int>> listca, List<SortedSet<int>> listpa)
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
