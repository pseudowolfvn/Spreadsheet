using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spreadsheet
{
    abstract class Operator<TResult, TArgs>
    {
        protected int priority;
        abstract public int Priority { get; }
        abstract public TResult use(params TArgs[] list);
    }
    class ArithmOperator : Operator<BigInteger, BigInteger>
    {
        public override int Priority { get { return priority; } }
        ArithmOps op;
        public bool isUnary()
        {
            if (op == ArithmOps.UnaryMinus
                || op == ArithmOps.UnaryPlus
                || op == ArithmOps.Dec
                || op == ArithmOps.Inc)
                return true;
            else return false;
        }
        public override BigInteger use(params BigInteger[] list)
        {
            BigInteger result = list[0];
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
        public ArithmOperator(string str, int brackets)
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
            }
            priority = brackets * 20;
            switch(op)
            {
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
    class MixedOperator : Operator<bool, BigInteger>
    {
        public override int Priority { get { return priority; } }
        LogicOps op;
        public override bool use(params BigInteger[] list)
        {
            BigInteger x = list[0], y = list[1];
            bool result = true;
            switch (op)
            {
                case LogicOps.Greater:
                    result = x > y;
                    break;
                case LogicOps.Less:
                    result = x < y;
                    break;
                case LogicOps.Equal:
                    result = x == y;
                    break;
            }
            return result;
        }
        public MixedOperator(string str, int brackets)
        {
            switch (str)
            {
                case ">":
                    op = LogicOps.Greater;
                    break;
                case "<":
                    op = LogicOps.Less;
                    break;
                case "=":
                    op = LogicOps.Equal;
                    break;
            }
            priority = brackets * 20;
            switch (op)
            {
                case LogicOps.Equal:
                    priority += 1;
                    break;
                case LogicOps.And:
                    priority += 2;
                    break;
                case LogicOps.Or:
                    priority += 3;
                    break;
                case LogicOps.Not:
                    priority += 4;
                    break;
            }
        }
    }
    class LogicOperator : Operator<bool, bool>
    {
        public override int Priority { get { return priority; } }
        LogicOps op;
        public bool isUnary()
        {
            if (op == LogicOps.Not)
                return true;
            else return false;
        }
        public override bool use(params bool[] list)
        {
            bool x = list[0], y;
            if (!isUnary()) y = list[1];
            bool result = true;
            switch (op)
            {
                case LogicOps.And:
                    result = x && y;
                    break;
                case LogicOps.Or:
                    result = x || y;
                    break;
                case LogicOps.Not:
                    result = !x;
                    break;
                case LogicOps.Equal:
                    result = x == y;
                    break;
            }
            return result;
        }
        public LogicOperator(string str, int brackets)
        {
            switch (str)
            {
                case "and":
                    op = LogicOps.And;
                    break;
                case "or":
                    op = LogicOps.Or;
                    break;
                case "not":
                    op = LogicOps.Not;
                    break;
                case "=":
                    op = LogicOps.Equal;
                    break;
            }
            priority = brackets * 20;
            switch (op)
            {
                case LogicOps.Equal:
                    priority += 5;
                    break;
                case LogicOps.And:
                    priority += 6;
                    break;
                case LogicOps.Or:
                    priority += 7;
                    break;
                case LogicOps.Not:
                    priority += 8;
                    break;
            }
        }
    }
}
