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
        abstract public BigInteger calculate();
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
        public override BigInteger calculate()
        {
            if (right == null) throw (new BadOperator(position));
            BigInteger tmp = right.calculate();
            return op.use(left.calculate(), tmp);
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
        public override BigInteger calculate()
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
        BigInteger value;
        string binding = "";
        public override BigInteger calculate()
        {
            if (binding != "")
            {
                int index = 0;
                while (index < binding.Length && Char.IsUpper(binding[index])) ++index;
                string column = binding.Substring(0, index),
                    row = binding.Substring(index);
                return BigInteger.Parse((App.Current.MainWindow as MainWindow).Data[row, column].Value);
            }
            return value;
        }
        public LeafNode(string val, int pos) : base(pos)
        {
            if (Char.IsUpper(val[0])) binding = val;
            else value = BigInteger.Parse(val);
        }
        public override int Priority { get { return 0; } }
    }
}
