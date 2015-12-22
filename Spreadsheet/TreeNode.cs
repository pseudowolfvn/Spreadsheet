using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Spreadsheet
{
    abstract class TreeNode
    {
        //protected int priority;
        protected int position;
        protected Operator op;
        abstract public dynamic calculate();
        //public int Priority { get { return priority; } }
        public int Position { get { return position; } }
        public virtual int Priority { get { return op.Priority; } }
        public TreeNode(int pos)
        {
            //priority = prior;
            position = pos;
        }
    }
    class BinaryNode : TreeNode
    {
        TreeNode left, right;
        public TreeNode Left { get { return left; } set { left = value; } }
        public TreeNode Right { get { return right; } set { right = value; } }
        public override dynamic calculate()
        {
            if (right == null) throw new BadOperator(position);
            try
            {
                dynamic tmp = right.calculate();
                return op.use(left.calculate(), tmp);
            }
            catch (InvalidCastException e)
            {
                throw new BadArgs(position);
            }
            catch (DivideByZeroException e)
            {
                throw new BadDiv(position);
            }
        }
        public BinaryNode(string str, int pos, int brackets, TreeNode addedLeft = null, TreeNode addedRight = null) 
            : base(pos)
        {
            left = addedLeft;
            right = addedRight;
            op = new Operator(str, brackets);
        }
    }
    class UnaryNode : TreeNode
    {
        TreeNode child;
        public TreeNode Child { get { return child; } set { child = value; } }
        public override dynamic calculate()
        {
            return op.use(child.calculate());
        }
        public UnaryNode(string str, int pos, int brackets, TreeNode addedChild = null) : base (pos)
        {
            op = new Operator(str, brackets);
            child = addedChild;
        }
    }
    class LeafNode : TreeNode
    {
        string value;
        string binding = "";
        public override dynamic calculate()
        {
            string var = "";
            if (binding != "")
                var = (App.Current.MainWindow as MainWindow).DataBase[binding].Value;
            else var = value;
            BigInteger resultBigInteger;
            bool resultBool;
            if (BigInteger.TryParse(var, out resultBigInteger)) return resultBigInteger;
            else if (System.Boolean.TryParse(var, out resultBool)) return resultBool;
            else throw new BadArgs(position);
        }
        public LeafNode(string val, int pos) : base(pos)
        {
            if (Char.IsUpper(val[0])) binding = val;
            else value = val;
        }
        public override int Priority { get { return 0; } }
    }
}
