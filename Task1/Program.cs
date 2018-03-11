using StudentData;
using Sort;
using System.Diagnostics;
using System;

namespace TestSorting
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "test.d";
            var fileSize = 1000L * 1024L;

            GenerationBigData.GenerateBiGData(fileName, fileSize);
            SortSimpleMerge sort = new SortSimpleMerge();
            var timer = new Stopwatch();
            timer.Start();
            sort.Sort("test.d");
            timer.Stop();
            Console.WriteLine($"{timer.ElapsedMilliseconds} мс");
            Console.WriteLine(sort.IsSorted(fileName));
            Console.ReadLine();
        }
    }
}
