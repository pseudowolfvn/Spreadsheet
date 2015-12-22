using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spreadsheet
{
    class Operator
    {
        int priority;
        public int Priority { get { return priority; } }
        ArithmOps op;
        public bool isArithm()
        {
            if (op == ArithmOps.UnaryMinus
                || op == ArithmOps.UnaryPlus
                || op == ArithmOps.Dec
                || op == ArithmOps.Inc
                || op == ArithmOps.Minus
                || op == ArithmOps.Plus
                || op == ArithmOps.Multiply
                || op == ArithmOps.Div
                || op == ArithmOps.Mod
                || op == ArithmOps.Pow)
                return true;
            else return false;
        }
        public bool isLogic()
        {
            if (op == ArithmOps.Not
                || op == ArithmOps.Or
                || op == ArithmOps.And
                || op == ArithmOps.Equal
                || op == ArithmOps.Greater
                || op == ArithmOps.Less)
                return true;
            else return false;
        }
        public bool isMixed()
        {
            if (op == ArithmOps.Equal
                || op == ArithmOps.Greater
                || op == ArithmOps.Less)
                return true;
            else return false;
        }
        public bool isUnary()
        {
            if (op == ArithmOps.UnaryMinus
                || op == ArithmOps.UnaryPlus
                || op == ArithmOps.Dec
                || op == ArithmOps.Inc
                || op == ArithmOps.Not)
                return true;
            else return false;
        }
        public dynamic use(params dynamic[] list)
        {
            if (isArithm())
            {
                BigInteger result, x, y;
                if (list[0].GetType() == typeof(BigInteger) && (list.Length < 2 || list[1].GetType() == typeof(BigInteger)))
                {
                    result = (BigInteger)list[0];
                    if (!isUnary())
                    {
                        x = (BigInteger)list[0];
                        y = (BigInteger)list[1];
                    }
                    else
                    {
                        x = (BigInteger)0;
                        y = (BigInteger)list[0];
                    }
                    switch (op)
                    {
                        case ArithmOps.Plus:
                            for (int i = 1; i < list.Length; i++)
                                result += list[i];
                            break;
                        case ArithmOps.Minus:
                            for (int i = 1; i < list.Length; i++)
                                result -= list[i];
                            break;
                        case ArithmOps.UnaryMinus:
                            result = -result;
                            break;
                        case ArithmOps.Multiply:
                            for (int i = 1; i < list.Length; i++)
                                result *= list[i];
                            break;
                        case ArithmOps.Div:
                            for (int i = 1; i < list.Length; i++)
                                    result /= list[i];
                            break;
                        case ArithmOps.Mod:
                            for (int i = 1; i < list.Length; i++)
                                result %= list[i];
                            break;
                        case ArithmOps.Pow:
                            result = 1;
                            for (int i = 1; i < list.Length; i++)
                                for (BigInteger exp = 0; exp < list[i]; exp++)
                                    result *= list[0];
                            break;
                        case ArithmOps.Dec:
                            result = --result;
                            break;
                        case ArithmOps.Inc:
                            result = ++result;
                            break;
                    }
                    return result;
                }
            }
            else if (isLogic())
            {
                bool result = false; 
                if (list[0].GetType() == typeof(bool) && (list.Length < 2 || list[1].GetType() == typeof(bool)))
                {
                    bool x, y;
                    x = (bool)list[0];
                    if (!isUnary())
                        y = (bool)list[1];
                    else y = false;
                    switch (op)
                    {
                        case ArithmOps.And:
                            result = x && y;
                            break;
                        case ArithmOps.Or:
                            result = x || y;
                            break;
                        case ArithmOps.Not:
                            result = !x;
                            break;
                        case ArithmOps.Equal:
                            result = x == y;
                            break;
                    }
                }
                else if (isMixed() && list[0].GetType() == typeof(BigInteger) && list.Length > 1 && list[1].GetType() == typeof(BigInteger))
                {
                    BigInteger x, y;
                    if (list[0].GetType() == typeof(BigInteger) && list.Length > 1 && list[1].GetType() == typeof(BigInteger))
                    {
                        x = (BigInteger)list[0];
                        y = (BigInteger)list[1];
                        switch (op)
                        {
                            case ArithmOps.Greater:
                                result = x > y;
                                break;
                            case ArithmOps.Less:
                                result = x < y;
                                break;
                            case ArithmOps.Equal:
                                result = x == y;
                                break;
                        }
                    }
                }
                else
                {
                    throw new InvalidCastException();
                }
                return result;
            }
            return null;
        }
        public Operator(string str, int brackets)
        {
            switch (str)
            {
                case "+":
                    op = ArithmOps.Plus;
                    break;
                case "-":
                    op = ArithmOps.Minus;
                    break;
                case "+U":
                    op = ArithmOps.UnaryPlus;
                    break;
                case "-U":
                    op = ArithmOps.UnaryMinus;
                    break;
                case "*":
                    op = ArithmOps.Multiply;
                    break;
                case "/": case "div":
                    op = ArithmOps.Div;
                    break;
                case "%": case "mod":
                    op = ArithmOps.Mod;
                    break;
                case "^":
                    op = ArithmOps.Pow;
                    break;
                case "decU":
                    op = ArithmOps.Dec;
                    break;
                case "incU":
                    op = ArithmOps.Inc;
                    break;
                case ">":
                    op = ArithmOps.Greater;
                    break;
                case "<":
                    op = ArithmOps.Less;
                    break;
                case "=":
                    op = ArithmOps.Equal;
                    break;
                case "and":
                    op = ArithmOps.And;
                    break;
                case "or":
                    op = ArithmOps.Or;
                    break;
                case "notU":
                    op = ArithmOps.Not;
                    break;
            }
            priority = brackets * 20;
            switch(op)
            {
                case ArithmOps.Equal:
                    priority += 1;
                    break;
                case ArithmOps.Greater:
                    priority += 2;
                    break;
                case ArithmOps.Less:
                    priority += 3;
                    break;
                case ArithmOps.And:
                    priority += 4;
                    break;
                case ArithmOps.Or:
                    priority += 5;
                    break;
                case ArithmOps.Not:
                    priority += 6;
                    break;
                case ArithmOps.Minus: case ArithmOps.Plus:
                    priority += 11;
                    break;
                case ArithmOps.Multiply: case ArithmOps.Div: case ArithmOps.Mod:
                    priority += 12;
                    break;
                case ArithmOps.Pow:
                    priority += 13;
                    break;
                case ArithmOps.UnaryMinus: case ArithmOps.UnaryPlus: case ArithmOps.Dec: case ArithmOps.Inc:
                    priority += 14;
                    break;
            }
        }
    }
}
