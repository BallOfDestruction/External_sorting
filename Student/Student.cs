using System;

namespace StudentData
{
    [Serializable]
    public class Student : IComparable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DOB { get; set; }
        public int NumberCard { get; set; }
        public int NumberSpecialty { get; set; }
        public string Department { get; set; }

        public Student()
        {
            
        }

        public Student(string firstName, string lastName, string middleName, DateTime dbo, int numberCard, int numberSpeciality, string department)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            DOB = dbo;
            NumberCard = numberCard;
            NumberSpecialty = numberSpeciality;
            Department = department;
        }
        public override string ToString()
        {
            return $"{FirstName}\n{LastName}\n{MiddleName}\n{DOB}\n{NumberCard}\n{NumberSpecialty}\n{Department}\n";
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Student st)) return 0;

            var firstCompare = FirstName?.CompareTo(st.FirstName) ?? 0;
            if (firstCompare != 0) return firstCompare;

            var secondCompare = NumberCard.CompareTo(st.NumberCard);
            return secondCompare == 0 ? DOB.CompareTo(st.DOB) : secondCompare;
        }
    }
}
