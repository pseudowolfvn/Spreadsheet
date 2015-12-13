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
        public bool isUnary()
        {
            if (op == ArithmOps.UnaryMinus
                || op == ArithmOps.UnaryPlus
                || op == ArithmOps.Dec
                || op == ArithmOps.Inc)
                return true;
            else return false;
        }
        public BigInteger use(params BigInteger[] list)
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
            }
            priority = brackets * 10;
            switch(op)
            {
                case ArithmOps.Minus: case ArithmOps.Plus:
                    priority += 1;
                    break;
                case ArithmOps.Multiply: case ArithmOps.Div: case ArithmOps.Mod:
                    priority += 2;
                    break;
                case ArithmOps.Pow:
                    priority += 3;
                    break;
                case ArithmOps.UnaryMinus: case ArithmOps.UnaryPlus: case ArithmOps.Dec: case ArithmOps.Inc:
                    priority += 4;
                    break;
            }
        }
    }
}
