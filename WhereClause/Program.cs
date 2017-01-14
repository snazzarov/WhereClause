using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhereClause
{
    class Program
    {
        //static List<string> listParams = new List<string>() { "Mary", "Ann", "WALLIN", "Julia" };
        static List<string> listParams = new List<string>() { "Mary", "Ann", "WALLIN" };
        static void Main(string[] args)
        {
            string sqlWhere = Names.BuildWhereForPatientsSql(listParams);
            Console.WriteLine(sqlWhere);
        }
    }
}

