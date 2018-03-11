using System;
using System.IO;
using System.Xml;
using ExternalSort.Core;

namespace StudentData
{
    public class StudentXmlReader : StudentXml, IExternalReader<Student>
    {
        private XmlTextReader _reader;

        /// <summary>
        /// Текущий считанный студент
        /// </summary>
        public Student Current { get; private set; }

        public void OpenFile(string path)
        {
            _reader = new XmlTextReader(new BufferedStream(new FileStream(path, FileMode.Open), 1024 * 1024 * 50));
        }

        /// <summary>
        /// Метка конца файла
        /// </summary>
        public bool IsCanRead { get; set; } = true;

        /// <summary>
        /// Читает из потока, если считано удачно = true, иначе false
        /// </summary>
        /// <returns></returns>
        public bool IsNext()
        {
            if (!_reader.Read() || !IsCanRead)
            {
                IsCanRead = false;
                return false;
            }
            _reader.ReadToFollowing(FirstNameXml);
            if (!_reader.Read() || !IsCanRead)
            {
                IsCanRead = false;
                return false;
            }

            var student = new Student();
            student.FirstName = _reader.Value;
            _reader.ReadToFollowing(LastNameXml);
            _reader.Read();
            student.LastName = _reader.Value;
            _reader.ReadToFollowing(MiddleNameXml);
            _reader.Read();
            student.MiddleName = _reader.Value;
            _reader.ReadToFollowing(DOBXml);
            _reader.Read();
            student.DOB = DateTime.Parse(_reader.Value);
            _reader.ReadToFollowing(NumberCardXml);
            _reader.Read();
            student.NumberCard = int.Parse(_reader.Value);
            _reader.ReadToFollowing(NumberSpecialtyXml);
            _reader.Read();
            student.NumberSpecialty = int.Parse(_reader.Value);
            _reader.ReadToFollowing(DepartmentXml);
            _reader.Read();
            student.Department = _reader.Value;
            Current = student;
            return true;
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _reader?.Close();
        }
    }
}
