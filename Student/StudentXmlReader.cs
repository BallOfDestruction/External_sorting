using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace StudentData
{
    public class StudentXmlReader
    {
        private XmlTextReader reader;
        private Student _current;

        /// <summary>
        /// Текущий считанный студент
        /// </summary>
        public Student Current => _current;

        public StudentXmlReader(string path)
        {
            this.reader = new XmlTextReader(new BufferedStream(new FileStream(path, FileMode.Open), 1024 * 1024 * 50));
        }

        /// <summary>
        /// Метка конца файла
        /// </summary>
        public bool CurrentRead = true;

        /// <summary>
        /// Читает из потока, если считано удачно = true, иначе false
        /// </summary>
        /// <returns></returns>
        public bool IsNext()
        {
            if (!reader.Read() || !CurrentRead)
            {
                CurrentRead = false;
                return false;
            }
            Student student = new Student();
            reader.ReadToFollowing("FirstName");
            if (!reader.Read() || !CurrentRead)
            {
                CurrentRead = false;
                return false;
            }
            student.FirstName = reader.Value;
            reader.ReadToFollowing("LastName");
            reader.Read();
            student.LastName = reader.Value;
            reader.ReadToFollowing("MiddleName");
            reader.Read();
            student.MiddleName = reader.Value;
            reader.ReadToFollowing("DOB");
            reader.Read();
            student.DOB = DateTime.Parse(reader.Value);
            reader.ReadToFollowing("NumberCard");
            reader.Read();
            student.NumberCard = Int32.Parse(reader.Value);
            reader.ReadToFollowing("NumberSpecialty");
            reader.Read();
            student.NumberSpecialty = Int32.Parse(reader.Value);
            reader.ReadToFollowing("Department");
            reader.Read();
            student.Department = reader.Value;
            _current = student;
            return true;
        }

        public Student[] Read(long count)
        {
            List<Student> students = new List<Student>();
            for (int i = 0; this.IsNext() && count > i; i++)
            {
                students.Add(this.Current);
            }
            return students.ToArray();
        }

        public void Close()
        {
            reader.Close();
        }
    }
}
