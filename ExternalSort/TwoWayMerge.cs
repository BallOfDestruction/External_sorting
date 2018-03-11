using System;
using System.IO;
using ExternalSort.Core;

namespace ExternalSort
{
    /// <summary>
    /// Сортировка двухпутевым слиянием
    /// </summary>
    public class TwoWayMerge : IExternalSort
    {
        private const string FirstFilename = "1.d";
        private const string SecondFilename = "2.d";

        /// <summary>
        /// Сортировка двухпутевого слияния Источник → https://prog-cpp.ru/sort-merge/
        /// Исходная последовательность разбивается на две подпоследовательности:
        /// Двухпутевое слияние
        /// Исходная последовательность разбивается на две подпоследовательности:
        /// Эти две подпоследовательности объединяются в одну, содержащую упорядоченные пары.
        /// Полученная последовательность снова разбивается на две, и пары объединяются в упорядоченные четверки:
        /// Полученная последовательность снова разбивается на две и собирается в упорядоченные восьмерки.
        /// Данная операция повторяется до тех пор, пока полученная упорядоченная последовательность не будет иметь такой же размер, как у сортируемой.
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void DoSorting<TReader, TWriter, TInnerType>(string path)
            where TInnerType : class, IComparable
            where TReader : IExternalReader<TInnerType>
            where TWriter : IExternalWriter<TInnerType>
        {
            //Длинна отсортированной фазы
            var lengthFase = 1L;
            while (true)
            {
                //Разделение
                using (var reader = Activator.CreateInstance<TReader>())
                {
                    reader.OpenFile(path);
                    using (var writerOne = Activator.CreateInstance<TWriter>())
                    {
                        writerOne.OpenFile(FirstFilename);
                        using (var writerTwo = Activator.CreateInstance<TWriter>())
                        {
                            writerTwo.OpenFile(SecondFilename);
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
                using (var writer = Activator.CreateInstance<TWriter>())
                {
                    writer.OpenFile(path);
                    using (var readerOne = Activator.CreateInstance<TReader>())
                    {
                        readerOne.OpenFile(FirstFilename);
                        using (var readerTwo = Activator.CreateInstance<TReader>())
                        {
                            readerTwo.OpenFile(SecondFilename);
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
        private void Devision<TInnerType>(IExternalReader<TInnerType> reader, IExternalWriter<TInnerType> writerOne,
            IExternalWriter<TInnerType> writerTwo, long step)
            where TInnerType : class, IComparable
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
        private void Merge<TInnerType>(IExternalReader<TInnerType> readerOne, IExternalReader<TInnerType> readerTwo,
            IExternalWriter<TInnerType> writer, long step)
            where TInnerType : class, IComparable
        {
            while (true)
            {
                if (!readerOne.IsCanRead && !readerTwo.IsCanRead)
                    return;

                DoPhase(readerOne, readerTwo, writer, step);
            }
        }

        private void DoPhase<TInnerType>(IExternalReader<TInnerType> readerOne, IExternalReader<TInnerType> readerTwo, 
            IExternalWriter<TInnerType> writer, long step)
            where TInnerType : class, IComparable
        {
            var stepOne = step;
            var stepTwo = step;

            TInnerType currentFirst = null;
            TInnerType currentSecond = null;

            while (true)
            {
                currentFirst = ReadNextStudentInPhase(stepOne, readerOne, currentFirst);
                currentSecond = ReadNextStudentInPhase(stepTwo, readerTwo, currentSecond);

                if (currentFirst == null && currentSecond == null) return;

                if (currentFirst == null && currentSecond != null)
                {
                    FillRest(stepTwo, readerTwo, writer, currentSecond);
                    return;
                }

                if (currentFirst != null && currentSecond == null)
                {
                    FillRest(stepOne, readerOne, writer, currentFirst);
                    return;
                }

                if (currentFirst.CompareTo(currentSecond) > 0)
                {
                    writer.Write(currentSecond);
                    stepTwo--;
                    currentSecond = null;
                }
                else
                {
                    writer.Write(currentFirst);
                    stepOne--;
                    currentFirst = null;
                }
            }
        }

        private void FillRest<TInnerType>(long step, IExternalReader<TInnerType> reader,
            IExternalWriter<TInnerType> writer, TInnerType student)
            where TInnerType : class, IComparable
        {
            var localStep = step;
            var localStudent = student;

            while (localStudent != null)
            {
                writer.Write(localStudent);
                localStep--;
                localStudent = ReadNextStudentInPhase(localStep, reader, null);
            }
        }

        private TInnerType ReadNextStudentInPhase<TInnerType>(long step, IExternalReader<TInnerType> reader,
            TInnerType currentStudent)
            where TInnerType : class, IComparable
        {
            if (step <= 0) return null;

            if (currentStudent != null) return currentStudent;

            return reader.IsNext() ? reader.Current : null;
        }

        /// <summary>
        /// Является ли отсортированным данный файл
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsSorted<TReader, TInnerType>(string path)
            where TReader : IExternalReader<TInnerType>
            where TInnerType : class, IComparable
        {
            using (var reader = Activator.CreateInstance<TReader>())
            {
                reader.OpenFile(path);
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