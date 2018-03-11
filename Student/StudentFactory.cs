using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace StudentData
{
    public class StudentFactory
    {
        public string FirstNamePath { get; set; } = "FirstName.d";
        public string LastNamePath { get; set; } = "LastName.d";
        public string MiddleNamePath { get; set; } = "MiddleName.d";
        public string DboPath { get; set; } = "DBO.d";
        public string NumberCardPath { get; set; } = "NumberCard.d";
        public string NumberSpecialtyPath { get; set; } = "NumerSpeciality.d";
        public string DepartnmentPath { get; set; } = "Department.d";

        private static readonly Random Random = new Random();

        private readonly string[] _firstName;
        private readonly string[] _lastName;
        private readonly string[] _middleName;
        private readonly DateTime[] _dbo;
        private readonly int[] _numberCard;
        private readonly int[] _numberSpeciality;
        private readonly string[] _department;

        public StudentFactory()
        {
            _firstName = GetArrayData<string>(FirstNamePath);
            _lastName = GetArrayData<string>(LastNamePath);
            _middleName = GetArrayData<string>(MiddleNamePath);
            _dbo = GetArrayData<DateTime>(DboPath);
            _numberCard = GetArrayData<int>(NumberCardPath);
            _numberSpeciality = GetArrayData<int>(NumberSpecialtyPath);
            _department = GetArrayData<string>(DepartnmentPath);
        }

        public Student GenerateElement()
        {
            var student = new Student
            {
                FirstName = GetRandomValue(_firstName),
                LastName = GetRandomValue(_lastName),
                MiddleName = GetRandomValue(_middleName),
                DOB = GetRandomValue(_dbo),
                NumberCard = GetRandomValue(_numberCard),
                NumberSpecialty = GetRandomValue(_numberSpeciality),
                Department = GetRandomValue(_department)
            };
            return student;
        }

        private static T GetRandomValue<T>(IReadOnlyList<T> array)
        {
            return array[Random.Next(array.Count)];
        }

        private T[] GetArrayData<T>(string path)
        {
            var reader = new StreamReader(path);
            var data = reader.ReadToEnd();
            return data.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(Parse<T>).ToArray();
        }

        private static T Parse<T>(string str)
        {
            var obj = default(T);
            switch (typeof(T).ToString())
            {
                case "System.String":
                    {
                        obj = (T)(object)str;
                        break;
                    }
                case "System.Int32":
                    {
                        obj = (T)(object)int.Parse(str);
                        break;
                    }
                case "System.DateTime":
                    {
                        obj = (T)(object)DateTime.Parse(str);
                        break;
                    }
            }
            return obj;
        }
    }
}
