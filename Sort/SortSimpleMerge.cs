using System;
using System.IO;
using StudentData;

namespace Sort
{
    public class SortSimpleMerge
    {
        /// <summary>
        /// Сортировка простым слиянием Источник → http://www.intuit.ru/studies/courses/648/504/lecture/11473?page=1
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void Sort(string path)
        {
            //Длинна отсортированной фазы
            long lengthFase = 1;
            //Счетчик количества проходов
            int count = 0;
            while (true)
            {
                count++;
                //Разделение
                StudentXmlReader reader = new StudentXmlReader(path);
                StudentXmlWriter writerOne = new StudentXmlWriter("1.d");
                StudentXmlWriter writerTwo = new StudentXmlWriter("2.d");
                Devision(reader, writerOne, writerTwo, lengthFase);
                reader.Close();
                writerOne.Close();
                writerTwo.Close();
                //140 байт - это начальные теги xml
                using (var l = new FileStream("2.d", FileMode.Open))
                {
                    if (l.Length < 200)
                    {
                        break;
                    }
                }
                //Слияние
                StudentXmlReader readerOne = new StudentXmlReader("1.d");
                StudentXmlReader readerTwo = new StudentXmlReader("2.d");
                StudentXmlWriter writer = new StudentXmlWriter(path);
                Merge(readerOne, readerTwo, writer, lengthFase);
                writer.Close();
                readerOne.Close();
                readerTwo.Close();
                lengthFase *= 2;
            }
            Console.WriteLine(count);
            //Console.ReadKey();
        }
        /// <summary>
        /// Фаза разделения, разделяем данный на два файла по сортированным фазам длинной step
        /// </summary>
        /// <param name="reader">Источник</param>
        /// <param name="writerOne">Первый получатель</param>
        /// <param name="writerTwo">Второй получатель</param>
        /// <param name="step">Длино сортированной фазы</param>
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
        /// <summary>
        /// Фаза слияния, сливаем из двух источников в один документ, попутно получая сортированный фазы длинной step * 2
        /// </summary>
        /// <param name="readerOne">Первый источник</param>
        /// <param name="readerTwo">Второй источник</param>
        /// <param name="writer">получатель</param>
        /// <param name="step">Ширина исходной сортированной фазы</param>
        private void Merge(StudentXmlReader readerOne, StudentXmlReader readerTwo, StudentXmlWriter writer, long step)
        {
            //Отсчет фазы для каждого источника
            long stepOne = step;
            long stepTwo = step;
            //Костыль, если файл был уже почитан, но его элемент не записан
            //При помощи правильной расстановки условий и данный флагов
            //Реализуем правильное поведение при 4 возможных исходах
            //Считаны оба, считан первый, считан второй, оба не считаны
            bool first = false;
            bool second = false;
            while (true)
            {
                //Если флак неактивный, то читает из документ, если активен, то не читает
                //Ленивое сравнение
                if ((stepOne > 0) && (first || readerOne.IsNext()))
                {
                    if ((stepTwo > 0) && (second || readerTwo.IsNext()))
                    {
                        if (readerOne.Current.CompareTo(readerTwo.Current) > 0)
                        {
                            writer.Write(readerTwo.Current);
                            //Типо первый считали, но он не использовался в записи
                            //Потому помечаем что его не нужно читать, иначе потеряются элементы
                            second = false;
                            first = true; 
                            stepTwo--;
                        }
                        else
                        {
                            writer.Write(readerOne.Current);
                            //Также ↑
                            second = true;
                            first = false;
                            stepOne--;
                        }
                    }
                    else
                    {
                        writer.Write(readerOne.Current);
                        stepOne--;
                        while ((stepOne > 0) && readerOne.IsNext())
                        {
                            writer.Write(readerOne.Current);
                            stepOne--;
                        }
                        //Условие окончание слияния фазы и переход к следующей
                        //Если ОБА документа закончились, то слияние тоже окончено
                        if (!readerOne.CurrentRead && !readerTwo.CurrentRead)
                        {
                            return;
                        }
                        else
                        {
                            stepOne = step;
                            stepTwo = step;
                            first = false;
                            if ((stepTwo <= 0) && second)
                            {
                                second = true;
                            }
                            else
                            {
                                second = false;
                            }
                        }
                    }
                }
                else
                {
                    if ((stepTwo > 0) && (second || readerTwo.IsNext()))
                    {
                        writer.Write(readerTwo.Current);
                        stepTwo--;
                        while ((stepTwo > 0) && readerTwo.IsNext())
                        {
                            writer.Write(readerTwo.Current);
                            stepTwo--;
                        }
                        if (!readerOne.CurrentRead && !readerTwo.CurrentRead) 
                        {
                            return;
                        }
                        else
                        {
                            stepOne = step;
                            stepTwo = step;
                            second = false;
                            if ((stepOne <= 0) && first)
                            {
                                first = true;
                            }
                            else
                            {
                                first = false;
                            }
                        }
                    }
                    else
                    {
                        if (!readerOne.CurrentRead && !readerTwo.CurrentRead)
                        {
                            return;
                        }
                        else
                        {
                            stepOne = step;
                            stepTwo = step;
                            if ((stepOne <= 0) && first)
                            {
                                first = true;
                            }
                            else
                            {
                                first = false;
                            }
                            if ((stepTwo <= 0) && second)
                            {
                                second = true;
                            }
                            else
                            {
                                second = false;
                            }
                        }
                    }
                }
            }
        }
    }
}