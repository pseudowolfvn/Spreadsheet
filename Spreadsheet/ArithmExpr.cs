using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spreadsheet
{
    enum ArithmOps { Plus, Minus, Multiply, Div, Mod, UnaryMinus, UnaryPlus, Dec, Inc, Pow, Not, Or, And, Equal, Greater, Less };
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
                    string func = "";
                    if (Char.IsLower(expression[position]))
                        func = new String(expression.Substring(position).TakeWhile(n => Char.IsLower(n)).ToArray());
                    else if (Char.IsUpper(expression[position]))
                        func = new String(expression.Substring(position).TakeWhile(n => (Char.IsUpper(n) || Char.IsDigit(n))).ToArray());
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
                        case "<":
                        case ">":
                        case "=":
                        case "and":
                        case "or":
                            if (prevLexem == null 
                                || (prevLexem.Type != LexemType.Const 
                                    && prevLexem.Type != LexemType.ClosingBracket 
                                    && prevLexem.Type != LexemType.Var))
                                throw (new BadOperator(position));
                            currLexem = new Lexem(LexemType.Binary, position, op);
                            break;
                        case "+":
                        case "-":
                        case "inc":
                        case "dec":
                        case "not":
                            if (prevLexem != null 
                                && (prevLexem.Type == LexemType.Const 
                                    || prevLexem.Type == LexemType.ClosingBracket
                                    || prevLexem.Type == LexemType.Var))
                                    currLexem = new Lexem(LexemType.Binary, position, op);
                            else currLexem = new Lexem(LexemType.Unary, position, op + "U");
                            break;
                        case "false":
                        case "true":
                            currLexem = new Lexem(LexemType.Const, position, op);
                            break;
                        case "(":
                            currLexem = new Lexem(LexemType.OpenBracket, position);
                            break;
                        case ")":
                            currLexem = new Lexem(LexemType.ClosingBracket, position);
                            break;
                        default:
                            if (Char.IsUpper(op[0])) // add deeper error handling
                                currLexem = new Lexem(LexemType.Var, position, op);
                            else throw (new BadOperator(position));
                            break;
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
        private void EraseState()
        {
            position = 0;
            openedBrackets = 0;
        }
        private void LoadState(int prior, int brackets)
        {
            position = prior;
            openedBrackets = brackets;
        }
        public override List<Lexem> AllVars
        {
            get
            {
                List<Lexem> result = new List<Lexem>();
                int oldPosition = position, oldBrackets = openedBrackets;
                EraseState();
                Lexem currLexem;
                while ((currLexem = this.NextLexem) != null)
                    if (currLexem.Type == LexemType.Var)
                        result.Add(currLexem);
                position = oldPosition;
                LoadState(oldPosition, oldBrackets);
                return result.GroupBy(x => x.Value).Select(distinct => distinct.First()).ToList();
            }
        }
    }
}
