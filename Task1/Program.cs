using StudentData;
using System.Diagnostics;
using System;
using ExternalSort;

namespace TestSorting
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "test.d";
            var fileSize = 1000 * 1024L;

            GenerationBigData.GenerateBiGData(fileName, fileSize);
            var sort = new SortSimpleMerge();
            var timer = new Stopwatch();
            timer.Start();
            sort.DoSorting<StudentXmlReader, StudentXmlWriter, Student>("test.d");
            timer.Stop();
            Console.WriteLine($"{timer.ElapsedMilliseconds} мс");
            Console.WriteLine(sort.IsSorted<StudentXmlReader, Student>(fileName));
            Console.ReadLine();
        }
    }
}
