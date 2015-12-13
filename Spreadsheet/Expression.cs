using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet
{
    abstract class Expression
    {
        protected string expression;
        protected int position;
        protected int openedBrackets;
        protected Lexem prevLexem;
        protected Lexem currLexem;
        public int OpenedBrackets { get { return openedBrackets; } }
        public Lexem CurrLexem { get { return CurrLexem; } }
        abstract public Lexem NextLexem { get; }
    }
    
}
