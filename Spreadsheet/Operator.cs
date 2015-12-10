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
            }
            return result;
        }
        public Operator(string str, int prior)
        {
            
        }
    }
}
