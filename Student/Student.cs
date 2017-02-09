using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData
{
    [Serializable]
    public struct Student : IComparable
    {
        public string FirstName;
        public string LastName;
        public string MiddleName;
        public DateTime DOB;
        public int NumberCard;
        public int NumberSpecialty;
        public string Department;
        public Student(string firstName, string lastName, string middleName, DateTime dbo, int numberCard, int numberSpeciality, string department)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MiddleName = middleName;
            this.DOB = dbo;
            this.NumberCard = numberCard;
            this.NumberSpecialty = numberSpeciality;
            this.Department = department;
        }
        public override string ToString()
        {
            return FirstName + "\n" + LastName + "\n" + MiddleName + "\n" + DOB.ToString() + "\n" + NumberCard.ToString() + "\n" + NumberSpecialty + "\n" + Department+"\n";
        }

        public int CompareTo(object obj)
        {
            Student st = (Student)obj;
            var firstCompare = this.FirstName.CompareTo(st.FirstName);
            if (firstCompare == 0)
            {
                var secondCompare = this.NumberCard.CompareTo(st.NumberCard);
                if (secondCompare == 0)
                {
                    return this.DOB.CompareTo(st.DOB);
                }
                else
                {
                    return secondCompare;
                }
            }
            else
            {
                return firstCompare;
            }
        }
    }
}
