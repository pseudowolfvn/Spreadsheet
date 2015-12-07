using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet
{
    class ArithmExpr
    {
        string expression;
        int position;
        int openedBrackets;
        Lexem prevLexem;
        Lexem currLexem;
        void skipBlanks()
        {
            while (!endOfExpr() && Char.IsWhiteSpace(expression[position])) position++;
        }
        void readLexem()
        {
            prevLexem = currLexem;
            if (!endOfExpr())
            {
                if (Char.IsDigit(expression[position]))
                {
                    bool xOnce = true;
                    string value = "";
                    while (!endOfExpr() 
                        && (Char.IsDigit(expression[position]) 
                        || (expression[position] == 'x' 
                        && xOnce)))
                    {
                        if (expression[position] == 'x') xOnce = false;
                        value += expression[position];
                        position++;
                    }
                    currLexem = new Lexem(LexemType.Number, position, value);
                }
                else
                {
                    switch (expression[position])
                    {
                        case '+':
                        case '*':
                        case '/':
                        case '%':
                            if (prevLexem == null 
                                || (prevLexem.Type != LexemType.Number && prevLexem.Type != LexemType.ClosingBracket))
                                throw (new BadOperator(position));
                            currLexem = new Lexem(LexemType.Binary, position, expression[position]);
                            break;
                        case '!':
                            currLexem = new Lexem(LexemType.Unary, position, expression[position]);
                            break;
                        case '-':
                            if (prevLexem == null 
                                || prevLexem.Type == LexemType.OpenBracket)
                                currLexem = new Lexem(LexemType.Unary, position, expression[position]);
                            else if (prevLexem.Type == LexemType.Number)
                                currLexem = new Lexem(LexemType.Binary, position, expression[position]);
                            else throw (new BadOperator(position));
                            break;
                        case '(':
                            currLexem = new Lexem(LexemType.OpenBracket, position);
                            break;
                        case ')':
                            currLexem = new Lexem(LexemType.ClosingBracket, position);
                            break;
                        default:
                            throw (new BadOperator(position));
                    }
                    position++;
                }
            }
        }
        bool endOfExpr()
        {
            return position == expression.Length;
        }
        public ArithmExpr(string str)
        {
            expression = str;
        }
        public int OpenedBrackets { get { return openedBrackets; } }
        public Lexem CurrLexem { get { return CurrLexem; } }
        public Lexem NextLexem
        {
            get
            {
                skipBlanks();
                if (!endOfExpr())
                {
                    readLexem();
                    switch (currLexem.Type)
                    {
                        case LexemType.OpenBracket:
                            openedBrackets++;
                            break;
                        case LexemType.ClosingBracket:
                            openedBrackets--;
                            if (openedBrackets < 0) throw (new BadBracket(position - 1));
                            break;
                    }
                    return currLexem;
                }
                else return null;
            }
        }
    }
}
