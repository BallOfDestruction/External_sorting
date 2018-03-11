using System.Text;
using System.IO;
using System.Xml;
using System;

namespace StudentData
{
    public class StudentXmlWriter : StudentXml, IDisposable
    {
        private XmlTextWriter _writer;
        private FileStream _file;

        public long LengthFile => _file?.Length ?? 0;


        public StudentXmlWriter(string path)
        {
            _file = new FileStream(path, FileMode.Create);
            _writer = new XmlTextWriter(new BufferedStream(_file), Encoding.UTF8);
            _writer.WriteStartDocument();
            _writer.WriteWhitespace("\n");
            _writer.WriteStartElement(CollectionXml);
            _writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            _writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
            _writer.WriteWhitespace("\n\t");
        }

        public void Write(Student student)
        {
            _writer.WriteStartElement(StartElementXml);
            _writer.WriteWhitespace("\n\t\t");
            _writer.WriteElementString(FirstNameXml, student.FirstName);
            _writer.WriteWhitespace("\n\t\t");
            _writer.WriteElementString(LastNameXml, student.LastName);
            _writer.WriteWhitespace("\n\t\t");
            _writer.WriteElementString(MiddleNameXml, student.MiddleName);
            _writer.WriteWhitespace("\n\t\t");
            _writer.WriteElementString(DOBXml, student.DOB.ToString());
            _writer.WriteWhitespace("\n\t\t");
            _writer.WriteElementString(NumberCardXml, student.NumberCard.ToString());
            _writer.WriteWhitespace("\n\t\t");
            _writer.WriteElementString(NumberSpecialtyXml, student.NumberSpecialty.ToString());
            _writer.WriteWhitespace("\n\t\t");
            _writer.WriteElementString(DepartmentXml, student.Department);
            _writer.WriteWhitespace("\n\t");
            _writer.WriteEndElement();
        }

        public void Dispose()
        {
            _writer?.WriteWhitespace("\n");
            _writer?.WriteEndElement();
            _writer?.Dispose();
        }
    }
}
