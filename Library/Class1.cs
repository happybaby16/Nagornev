using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class Func
    {
        public static double AgeAVG(List<DateTime> usersDateBirth)
        {
            double avg = 0;
            int counter = 0;
            DateTime now = DateTime.Now;
            foreach (DateTime BirthDay in usersDateBirth)
            {
                try
                {
                    int temp = (now - BirthDay).Days;
                    avg += (temp / 365);
                    counter++;
                }
                catch{}
            }
            return avg/counter;
        }
    }
}
