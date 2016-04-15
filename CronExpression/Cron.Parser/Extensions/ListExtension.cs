using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Extensions
{
    public static class ListExtension
    {

        public static void AddRange(this IList<int> list, IEnumerable<int> values)
        {
            foreach (var item in values)
            {
                list.Add(item);
            }
        }

        public static void Cut(this IList<int> list, int min, int max, int cutNotEveryNth)
        {
            list = list.Where((f, i) => f >= min && f < max && i % cutNotEveryNth != 0).ToList();
        }

        public static void Cut(this IList<int> list, string min, string max, string cutNotEveryNth)
        {
            list.Cut(int.Parse(min), int.Parse(max), int.Parse(cutNotEveryNth));
        }

        public static IList<int> CutMe(this IList<int> list, string min, string max, string cutNotEveryNth)
        {
            ListExtension.Cut(list, min, max, cutNotEveryNth);
            return list;
        }

        public static List<int> Empty(this IList<int> list)
        {
            return new List<int>();
        }

        public static List<int> Empty()
        {
            return new List<int>();
        }
        public static IList<int> Expand(int from, int to, int inc)
        {
            var values = new List<int>();
            var i = from;
            for (int j = to - inc; i <= j; i += inc)
            {
                values.Add(i);
            }
            if (i <= to)
            {
                values.Add(i);
            }
            return values;
        }
    }
}
