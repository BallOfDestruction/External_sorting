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
        /// <param name="path">Путь к файлу</param>
        /// <param name="maxSize">Его максимальный размер в байтах</param>
        public static void GenerateBiGData(string path, long maxSize)
        {
            using (var writer = new StudentXmlWriter())
            {
                writer.OpenFile(path);
                var factory = new StudentFactory();
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
        /// <param name="dboPath">Путь к вайлу</param>
        public static void GenerateDate(int age, string dboPath)
        {
            using (var writer = new StreamWriter(dboPath))
            {
                var year = DateTime.Now.Year - age;
                for (var i = 1; i < 13; i++)
                {
                    for (var j = 1; j <= DateTime.DaysInMonth(year, i); j++)
                    {
                        writer.WriteLine(new DateTime(year, i, j).ToShortDateString());
                    }
                }
            }
        }
        public static void GenerateNumber(long start, long end, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                for (var i = start; i <= end; i++)
                {
                    writer.WriteLine(i.ToString());
                }
            }
        }
    }
}
