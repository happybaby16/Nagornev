using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Возвращает динамический массив класса List, который содержит Id пользователей, которые содержат в имени
        /// строку SearchLine
        /// </summary>
        /// <param name="Id">List<int></param>
        /// <param name="Name">List<string></param>
        /// <param name="SearchLine">string</param>
        /// <returns></returns>
        public static List<int> SelectUsersByName(List<int> Id, List<string> Name, string SearchLine)
        {
            List<int> answer = new List<int>();
            string pattern = $".{{0,}}{SearchLine}.{{0,}}";

            if (Id.Count == Name.Count)
            {
                for (int i = 0; i < Name.Count; i++)
                {
                    if (Regex.IsMatch(Name[i], pattern))
                    {
                        answer.Add(Id[i]);
                    }
                }
                return answer;
            }

            return null;
        }
    }
}
