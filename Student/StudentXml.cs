using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData
{
    public class StudentXml
    {
        protected string CollectionXml { get; } = "ArrayOfStudent";
        protected string StartElementXml { get; } = "Student";
        protected string FirstNameXml { get; } = "FirstName";
        protected string LastNameXml { get; } = "LastName";
        protected string MiddleNameXml { get; } = "MiddleName";
        protected string DOBXml { get; } = "DOB";
        protected string NumberCardXml { get; } = "FirstName";
        protected string NumberSpecialtyXml { get; } = "NumberSpecialty";
        protected string DepartmentXml { get; } = "Department";
    }
}
