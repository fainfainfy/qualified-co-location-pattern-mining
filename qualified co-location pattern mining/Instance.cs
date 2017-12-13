using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qualified_co_location_pattern_mining
{
    public class Instance
    {
       public double x { get; set; }
        public double y { get; set; }
        public int  Id { get; set; }
        public int Type { get; set; }

        public int getx(int d)
        {
            return (int)x / d;
        }
        public int gety(int d)
        {
            return (int)y / d;
        }

        public double getD(double x1, double y1)
        {
            return (x - x1) * (x - x1) + (y - y1) * (y - y1);
        }

        public Boolean distance(double x1,double y1,double x2,double y2, double d)
        {
            if (((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)) > d*d|| ((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2))==d*d)
                return true;
            else
                return false;
        }

        Boolean distance(double d, double x1,  double y1)
        {
            if (((x1 - x) * (x1 - x)) + ((y1 - y) * (y1 - y)) > d * d || ((x1 - x) * (x1 - x)) + ((y1 - y) * (y1 - y)) == d * d)
                return true;
            else
                return false;
        }
    }

    
}
