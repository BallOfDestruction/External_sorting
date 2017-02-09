using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace StudentData
{
    public class StudentFactory
    {
        public string FirstNamePath { get; set; } = "FirstName.d";
        public string LastNamePath { get; set; } = "LastName.d";
        public string MiddleNamePath { get; set; } = "MiddleName.d";
        public string DBOPath { get; set; } = "DBO.d";
        public string NumberCardPath { get; set; } = "NumberCard.d";
        public string NumberSpecialtyPath { get; set; } = "NumerSpeciality.d";
        public string DepartnmentPath { get; set; } = "Department.d";

        private static Random random = new Random();

        private string[] firstName;
        private string[] lastName;
        private string[] middleName;
        private DateTime[] DBO;
        private int[] numberCard;
        private int[] numberSpeciality;
        private string[] department;

        public StudentFactory()
        {
            firstName = GetArrayData<string>(FirstNamePath);
            lastName = GetArrayData<string>(LastNamePath);
            middleName = GetArrayData<string>(MiddleNamePath);
            DBO = GetArrayData<DateTime>(DBOPath);
            numberCard = GetArrayData<int>(NumberCardPath);
            numberSpeciality = GetArrayData<int>(NumberSpecialtyPath);
            department = GetArrayData<string>(DepartnmentPath);
        }

        public Student GenerateElement()
        {
            Student student = new Student();
            student.FirstName = GetRandomValue(firstName);
            student.LastName = GetRandomValue(lastName);
            student.MiddleName = GetRandomValue(middleName);
            student.DOB = GetRandomValue(DBO);
            student.NumberCard = GetRandomValue(numberCard);
            student.NumberSpecialty = GetRandomValue(numberSpeciality);
            student.Department = GetRandomValue(department);
            return student;
        }

        private T GetRandomValue<T>(T[] array)
        {
            return array[random.Next(array.Length)];
        }

        private T[] GetArrayData<T>(string path)
        {
            var reader = new StreamReader(path);
            var data = reader.ReadToEnd();
            return data.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(w => Parse<T>(w)).ToArray();
        }

        private T Parse<T>(string str)
        {
            T obj = default(T);
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
