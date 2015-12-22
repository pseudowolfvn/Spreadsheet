using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet
{
    public class Exception : SystemException
    {
        string info;
        protected Exception(string str, int pos)
        {
            info = str + (pos + 1).ToString() + ";";
        }
        protected Exception(string str)
        {
            info = str + ";";
        }
        public string Info { get { return info; } }
    }
    public class BadBracket : Exception
    {
        public BadBracket(int pos) : base("Bad bracket at position: ", pos) { }
    }

    public class BadOperator : Exception
    {
        public BadOperator(int pos) : base("Bad operator at position: ", pos) { }
    }
    public class BadDiv : Exception
    {
        public BadDiv(int pos) : base("Bad division at position: ", pos) { }
    }
    public class BadArgs : Exception
    {
        public BadArgs(int pos) : base("Bad argument at position: ", pos) { }
    }
    public class BadCycle : Exception
    {
        public BadCycle(string cell) : base("Cycle detected in cell: " + cell) { }
    }
}
