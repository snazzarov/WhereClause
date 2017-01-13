﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Kim, Mary, Ann
p1    p2  p3  

Kim Jons Mary Ann
p1  p2   p3   p4

Patient: Mary Ann Kim
select distinct MemberFirstName, MemberLastName from Patients
where 
((SOUNDEX(MemberFirstName) = SOUNDEX(@p1) and SOUNDEX(MemberLastName) = SOUNDEX(p2)) or 
 (SOUNDE(MemberFirstName) = SOUNDEX(@p2) and SOUNDEX(MemberLastName) = SOUNDEX(p1)))
or 
((SOUNDEX(MemberFirstName) = SOUNDEX(@p1) and SOUNDEX(MemberLastName) = SOUNDEX(p3)) or 
 (SOUNDEX(MemberFirstName) = SOUNDEX(@p3) and SOUNDEX(MemberLastName) = SOUNDEX(p1)))
or 
((SOUNDEX(MemberFirstName) = SOUNDEX(@p1) and SOUNDEX(MemberLastName) = SOUNDEX(p2+p3)) or
 (SOUNDEX(MemberFirstName) = SOUNDEX(p2+p3) and SOUNDEX(MemberLastName) = SOUNDEX(p1)))
or 
((SOUNDEX(MemberFirstName) = SOUNDEX(@p1+p2) and SOUNDEX(MemberLastName) = SOUNDEX(p3)) or 
 (SOUNDEX(MemberFirstName) = SOUNDEX(p3) and SOUNDEX(MemberLastName) = SOUNDEX(p1+p2)))

    
string BuildWhereForPatientsSql(List<string> patients)

1) Build allowed pairs
2) build where clause
1, 2, 3, 4 elements in the List
 * 
 * 
 * 
 * 
 * 
 */
namespace WhereClause
{
    public class Names
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Names(string firstname, string lastname)
        {
            firstName = firstname;
            lastName = lastname;
        }
    }
    class Program
    {
        static List<string> listParams = new List<string>() { "Mary", "Ann", "Kim" , "Julia" };
        static void Main(string[] args)
        {
            string sqlWhere = BuildWhereForPatientsSql(listParams);
            Console.WriteLine(sqlWhere);
        }
        static string BuildWhereForPatientsSql(List<string> patients)
        {
            List<Names> listNames = new List<Names>();
            // 1 name
            if (patients.Count == 0)
                return "";
            if (patients.Count == 1)
            {
                Names couple1 = new Names(patients[0], patients[0]);
                listNames.Add(couple1);
                return FormatList(listNames);
            }
            // 1 name and 1 name
            for (int i = 0; i < patients.Count - 1; i++)
            {
                for (int j = i + 1; j < patients.Count; j++)
                {
                    Names couple1 = new Names(patients[i], patients[j]);
                    Names couple2 = new Names(patients[j], patients[i]);
                    listNames.Add(couple1);
                    listNames.Add(couple2);
                }
            }
            if (patients.Count == 2)
                return FormatList(listNames);
            // 1 name and 2 names
            for (int i = 0; i < patients.Count - 2; i++)
            {
                for (int j = i + 1; j < patients.Count - 1; j++)
                {
                    Names couple1 = new Names(patients[i], patients[j] + patients[j + 1]);
                    Names couple2 = new Names(patients[j] + patients[j + 1], patients[i]);
                    listNames.Add(couple1);
                    listNames.Add(couple2);
                }
            }
            // 2 names and 1 name
            for (int i = 0; i < patients.Count - 2; i++)
            {
                for (int j = i + 2; j < patients.Count; j++)
                {
                    Names couple1 = new Names(patients[i] + patients[i+1], patients[j]);
                    Names couple2 = new Names(patients[j], patients[i] + patients[i+1]);
                    listNames.Add(couple1);
                    listNames.Add(couple2);
                }
            }
            return FormatList(listNames);
        }
        static string FormatList(List<Names> list)
        {
            string result = "";
            if (list.Count == 1)
            {
                result = "Where " +
                        string.Format("(Soundex(MemberFirstName) = Soundex('{0}') or Soundex(MemberLastName) = Soundex('{0}'))\n", list[0].firstName, list[0].lastName);
                return result;
            }
            for(int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    result = "Where " +
                        string.Format("(Soundex(MemberFirstName) = Soundex('{0}') and Soundex(MemberLastName) = Soundex('{1}'))\n", list[i].firstName, list[i].lastName);
                }
                else
                {
                    result = result + " OR ";
                    result = result +
                        string.Format(" (Soundex(MemberFirstName) = Soundex('{0}') and Soundex(MemberLastName) = Soundex('{1}'))\n", list[i].firstName, list[i].lastName);
                }
            }
            return result;
        }
        static void PrintNames(List<Names> list)
        {
            for (int i = 0; i < list.Count; i++)
                Console.WriteLine(list[i].firstName + "---" + list[i].lastName);
            Console.WriteLine(FormatList(list));
        }
    }
}
