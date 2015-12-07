using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet
{
    enum LexemType { OpenBracket, ClosingBracket, Unary, Binary, Number };
    class Lexem
    {
        LexemType type;
        string value;
        int position;
        public LexemType Type { get { return type; } }
        public string Value { get { return value; } }
        public int Position { get { return position; } }
        public Lexem(LexemType t, int pos)
        {
            type = t;
            value = "";
            position = pos;
        }
        public Lexem(LexemType t, int pos, string str) : this(t, pos)
        {
            value = str;
        }
        public Lexem(LexemType t, int pos, char symb) : this(t, pos)
        {
            value = symb.ToString();
        }
    }
}
