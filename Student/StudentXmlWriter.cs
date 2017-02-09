using System.Text;
using System.IO;
using System.Xml;

namespace StudentData
{
    public class StudentXmlWriter
    {
        private XmlTextWriter writer;
        private FileStream file;

        public long LengthFile => file.Length;


        public StudentXmlWriter(string path)
        {
            file = new FileStream(path, FileMode.Create);
            this.writer = new XmlTextWriter(new BufferedStream(file), Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("ArrayOfStudent");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
            writer.WriteWhitespace("\n\t");
        }

        public void Write(Student student)
        {
            writer.WriteStartElement("Student");
            writer.WriteWhitespace("\n\t\t");
            writer.WriteElementString("FirstName", student.FirstName);
            writer.WriteWhitespace("\n\t\t");
            writer.WriteElementString("LastName", student.LastName);
            writer.WriteWhitespace("\n\t\t");
            writer.WriteElementString("MiddleName", student.MiddleName);
            writer.WriteWhitespace("\n\t\t");
            writer.WriteElementString("DOB", student.DOB.ToString());
            writer.WriteWhitespace("\n\t\t");
            writer.WriteElementString("NumberCard", student.NumberCard.ToString());
            writer.WriteWhitespace("\n\t\t");
            writer.WriteElementString("NumberSpecialty", student.NumberSpecialty.ToString());
            writer.WriteWhitespace("\n\t\t");
            writer.WriteElementString("Department", student.Department);
            writer.WriteWhitespace("\n\t");
            writer.WriteEndElement();
        }

        public void Close()
        {
            writer.WriteWhitespace("\n");
            writer.WriteEndElement();
            writer.Close();
        }
    }
}
