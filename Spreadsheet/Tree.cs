using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spreadsheet
{
    class Tree<ExpT> where ExpT : Expression
    {
        ExpT expression;
        TreeNode root;
        void addBinary(Lexem current)
        {
            BinaryNode bNode = new BinaryNode(current.Value, current.Position, expression.OpenedBrackets);
            BinaryNode parent = root as BinaryNode, child = parent;
            if (root == null || root.Priority == 0 || root.Priority >= bNode.Priority)
            {
                bNode.Left = root;
                root = bNode;
            }
            else
            {
                while (child.GetType() == typeof(BinaryNode) && child.Priority < bNode.Priority)
                {
                    parent = child;
                    child = (BinaryNode)parent.Right;
                }
                bNode.Left = child;
                parent.Right = bNode;
            }
        }
        void setRightNode(TreeNode node)
        {
            if (root == null)
            {
                root = node;
                return;
            }
            TreeNode p = root;
            while (true)
            {
                if (p.GetType() == typeof(UnaryNode))
                {
                    UnaryNode uNode = (UnaryNode)p;
                    if (uNode.Child == null)
                    {
                        uNode.Child = node;
                        break;
                    }
                    p = uNode.Child;
                    continue;
                }
                else if (p.GetType() == typeof(BinaryNode))
                {
                    BinaryNode bNode = (BinaryNode)p;
                    if (bNode.Right == null)
                    {
                        bNode.Right = node;
                        break;
                    }
                    p = bNode.Right;
                    continue;
                }
                else throw (new BadOperator(p.Position));
            }
        }
        void parse()
        {
            Lexem current = null;
            while ((current = expression.NextLexem) != null)
            {
                switch(current.Type)
                {
                    case LexemType.Unary:
                        setRightNode(new UnaryNode(current.Value, current.Position));
                        break;
                    case LexemType.Binary:
                        addBinary(current);
                        break;
                    case LexemType.Const:
                        setRightNode(new LeafNode(current.Value, current.Position));
                        break;
                }
            }
        }
        public object calculate(ExpT expr)
        {
            expression = expr;
            this.parse();
            if (root != null) return root.calculate();
            else return null;
        }
    }
}
