using System;
using System.Text;
using StudentData;
using Sort;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = ASCIIEncoding.UTF8;
            //GenerationBigData.GenerateData(20);
            //GenerationBigData.GenerateNumber(12340000, 12345000, GenerationBigData.NumberSpecialtyPath);
            GenerationBigData.GenerateBiGData("test.d", 10L*1024L);
            StudentXmlReader reader = new StudentXmlReader("1.d");
            foreach (Student st in reader.Read(10))
            {
                Console.WriteLine(st);
            }
            reader.Close();
            SortSimpleMerge sort = new SortSimpleMerge();
            sort.Sort("test.d");
            reader = new StudentXmlReader("1.d");
            foreach (Student st in reader.Read(10))
            {
                Console.WriteLine(st);
            }
            reader.Close();
            Console.ReadKey();
        }
    }
}
