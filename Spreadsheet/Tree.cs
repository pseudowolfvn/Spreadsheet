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
            //BinaryNode parent = root as BinaryNode;
            TreeNode parent = root, child = parent;
            if (root == null || root.Priority == 0 || root.Priority >= bNode.Priority)
            {
                bNode.Left = root;
                root = bNode;
            }
            else
            {
                while (child != null && child.Priority < bNode.Priority)
                {
                    if (child.GetType() == typeof(BinaryNode))
                    {
                        parent = child as BinaryNode;
                        child = (parent as BinaryNode).Right;
                    }
                    else if (child.GetType() == typeof(UnaryNode))
                    {
                        parent = child as UnaryNode;
                        child = (parent as UnaryNode).Child;
                    }
                    else break;
                }
                bNode.Left = child;
                if (parent.GetType() == typeof(BinaryNode)) (parent as BinaryNode).Right = bNode;
                else if (parent.GetType() == typeof(UnaryNode)) (parent as UnaryNode).Child = bNode;
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
                    UnaryNode uNode = p as UnaryNode;
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
                    BinaryNode bNode = p as BinaryNode;
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
                        setRightNode(new UnaryNode(current.Value, current.Position, this.expression.OpenedBrackets));
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

