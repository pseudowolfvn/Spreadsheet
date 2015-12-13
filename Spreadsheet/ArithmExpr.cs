using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spreadsheet
{
    enum ArithmOps { Plus, Minus, Multiply, Div, Mod, UnaryMinus, UnaryPlus, Dec, Inc, Pow };
    class ArithmExpr : Expression
    {
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
                    var func = new String(expression.Substring(position).TakeWhile(n => Char.IsLower(n)).ToArray());
                    position += (func.Length > 0) ? func.Length - 1: 0;
                    string op = (func.Length > 0) ? func : expression[position].ToString();
                    switch (op)
                    {
                        case "*":
                        case "/":
                        case "div":
                        case "%":
                        case "mod":
                        case "^":
                            if (prevLexem == null 
                                || (prevLexem.Type != LexemType.Const && prevLexem.Type != LexemType.ClosingBracket))
                                throw (new BadOperator(position));
                            currLexem = new Lexem(LexemType.Binary, position, op);
                            break;
                        case "+":
                        case "-":
                        case "inc":
                        case "dec":
                            //if (prevLexem == null 
                            //    || prevLexem.Type == LexemType.OpenBracket || prevLexem.Type == LexemType.Binary)
                            //    currLexem = new Lexem(LexemType.Unary, position, op + "U");
                            //else if (prevLexem.Type == LexemType.Const)
                            //    currLexem = new Lexem(LexemType.Binary, position, op);
                            //else throw (new BadOperator(position));
                            if (prevLexem != null 
                                && (prevLexem.Type == LexemType.Const || prevLexem.Type == LexemType.ClosingBracket))
                                    currLexem = new Lexem(LexemType.Binary, position, op);
                            else currLexem = new Lexem(LexemType.Unary, position, op + "U");
                            break;
                        case "(":
                            currLexem = new Lexem(LexemType.OpenBracket, position);
                            break;
                        case ")":
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
