using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spreadsheet
{
    enum ArithmOps { Plus, Minus, Multiply, Div, Mod };
    class ArithmExpr : Expression
    {
        public override Type valueType()
        {
            return typeof(BigInteger);
        }
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
                    string value = "";
                    while (!endOfExpr() 
                        && (Char.IsDigit(expression[position])))
                    {
                        value += expression[position];
                        position++;
                    }
                    currLexem = new Lexem(LexemType.Const, position, value);
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
                                || (prevLexem.Type != LexemType.Const && prevLexem.Type != LexemType.ClosingBracket))
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
                            else if (prevLexem.Type == LexemType.Const)
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
        public override Lexem NextLexem
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
