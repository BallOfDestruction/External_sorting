using System;
using System.IO;

namespace StudentData
{
    /// <summary>
    /// Класс генерацияя данных
    /// </summary>
    public static class GenerationBigData
    {
        /// <summary>
        /// Генерация большого файла
        /// </summary>
        /// <param name="maxSize">Его максимальный размер в байтах</param>
        public static void GenerateBiGData(string path, long maxSize)
        {
            using (var writer = new StudentXmlWriter(path))
            {
                StudentFactory factory = new StudentFactory();
                while (writer.LengthFile < maxSize)
                {
                    writer.Write(factory.GenerateElement());
                }
            }
        }
        /// <summary>
        /// Генерация словаря с датами
        /// </summary>
        /// <param name="age">Возраст людишек</param>
        public static void GenerateDate(int age, string DBOPath)
        {
            using (var writer = new StreamWriter(DBOPath))
            {
                int year = DateTime.Now.Year - age;
                int count = 0;
                for (int i = 1; i < 13; i++)
                {
                    for (int j = 1; j <= DateTime.DaysInMonth(year, i); j++)
                    {
                        writer.WriteLine(new DateTime(year, i, j).ToShortDateString());
                        count++;
                    }
                }
            }
        }
        public static void GenerateNumber(long start, long end, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                for (long i = start; i <= end; i++)
                {
                    writer.WriteLine(i.ToString());
                }
            }
        }
    }
}
