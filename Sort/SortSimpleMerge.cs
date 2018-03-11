using System;
using System.IO;
using StudentData;

namespace Sort
{
    public class SortSimpleMerge
    {
        private const string FirstFilename = "1.d";
        private const string SecondFilename = "2.d";

        /// <summary>
        /// Сортировка двухпутевого слияния Источник → http://www.intuit.ru/studies/courses/648/504/lecture/11473?page=1
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void Sort(string path)
        {
            //Длинна отсортированной фазы
            var lengthFase = 1L;
            //Счетчик количества проходов
            int count = 0;
            while (true)
            {
                count++;
                //Разделение
                using (var reader = new StudentXmlReader(path))
                {
                    using (var writerOne = new StudentXmlWriter(FirstFilename))
                    {
                        using (var writerTwo = new StudentXmlWriter(SecondFilename))
                        {
                            Devision(reader, writerOne, writerTwo, lengthFase);
                        }
                    }
                }


                //140 байт - это начальные теги xml
                using (var l = new FileStream(SecondFilename, FileMode.Open))
                {
                    if (l.Length < 200)
                        break;
                }

                //Слияние
                using (var writer = new StudentXmlWriter(path))
                {
                    using (var readerOne = new StudentXmlReader(FirstFilename))
                    {
                        using (var readerTwo = new StudentXmlReader(SecondFilename))
                        {
                            Merge(readerOne, readerTwo, writer, lengthFase);
                        }
                    }
                }

                lengthFase *= 2;
            }
        }
        /// <summary>
        /// Фаза разделения, разделяем данный на два файла по сортированным фазам длинной step
        /// </summary>
        /// <param name="reader">Источник</param>
        /// <param name="writerOne">Первый получатель</param>
        /// <param name="writerTwo">Второй получатель</param>
        /// <param name="step">Длина сортированной фазы</param>
        private void Devision(StudentXmlReader reader, StudentXmlWriter writerOne, StudentXmlWriter writerTwo, long step)
        {
            while (reader.IsNext())
            {
                writerOne.Write(reader.Current);
                for (long i = 1; (i < step) && reader.IsNext(); i++)
                {
                    writerOne.Write(reader.Current);
                }
                for (long i = 0; (i < step) && reader.IsNext(); i++)
                {
                    writerTwo.Write(reader.Current);
                }
            }
        }
        
        ///// <summary>
        ///// Фаза слияния, сливаем из двух источников в один документ, попутно получая сортированный фазы длинной step * 2
        ///// </summary>
        ///// <param name="readerOne">Первый источник</param>
        ///// <param name="readerTwo">Второй источник</param>
        ///// <param name="writer">получатель</param>
        ///// <param name="step">Длина исходной сортированной фазы</param>
        private void Merge(StudentXmlReader readerOne, StudentXmlReader readerTwo, StudentXmlWriter writer, long step)
        {
            while(true)
            {
                if (!readerOne.IsCanRead && !readerTwo.IsCanRead)
                    return;

                DoPhase(readerOne, readerTwo, writer, step);
            }
        }

        private void DoPhase(StudentXmlReader readerOne, StudentXmlReader readerTwo, StudentXmlWriter writer, long step)
        {
            var stepOne = step;
            var stepTwo = step;

            Student? currentFirst = null;
            Student? currentSecond = null;

            while (true)
            {
                currentFirst = ReadNextStudentInPhase(stepOne, readerOne, currentFirst);
                currentSecond = ReadNextStudentInPhase(stepTwo, readerTwo, currentSecond);

                if (!currentFirst.HasValue && !currentSecond.HasValue) return;

                if(!currentFirst.HasValue && currentSecond.HasValue)
                {
                    FillRest(stepTwo, readerTwo, writer, currentSecond.Value);
                    return;
                }

                if (currentFirst.HasValue && !currentSecond.HasValue)
                {
                    FillRest(stepOne, readerOne, writer, currentFirst.Value);
                    return;
                }

                if (currentFirst.Value.CompareTo(currentSecond.Value) > 0)
                {
                    writer.Write(currentSecond.Value);
                    stepTwo--;
                    currentSecond = null;
                }
                else
                {
                    writer.Write(currentFirst.Value);
                    stepOne--;
                    currentFirst = null;
                }
            }
        }

        private void FillRest(long step, StudentXmlReader reader, StudentXmlWriter writer, Student student)
        {
            var localStep = step;
            Student? localStudent = student;

            while(localStudent.HasValue)
            {
                writer.Write(localStudent.Value);
                localStep--;
                localStudent = ReadNextStudentInPhase(localStep, reader, null);
            }
        }

        private Student? ReadNextStudentInPhase(long step, StudentXmlReader reader, Student? currentStudent)
        {
            if (step <= 0) return null;

            if (currentStudent.HasValue) return currentStudent;

            if (reader.IsNext())
                return reader.Current;

            return null;
        }

        /// <summary>
        /// Является ли отсортированным данный файл
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsSorted(string path)
        {
            using (var reader = new StudentXmlReader(path))
            {
                reader.IsNext();
                var current = reader.Current;
                while (reader.IsNext())
                {
                    if (current.CompareTo(reader.Current) > 0) return false;
                    else
                        current = reader.Current;
                }
                return true;
            }
        }
    }
}