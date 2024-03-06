using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public record ExpressionToTextParameters(Func<double, string>? DoubleConvertion);

    public sealed class TextExpressionBodyVisitor : ExpressionVisitor
    {
        private readonly StringBuilder _builder;
        private readonly ExpressionToTextParameters _parameters;

        public TextExpressionBodyVisitor(StringBuilder builder, ExpressionToTextParameters parameters)
        {
            _builder = builder;
            _parameters = parameters;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Math) &&
                node.Method.Name == nameof(Math.Pow))
            {
                _builder.Append("(");
                Visit(node.Arguments[0]);
                _builder.Append(" ^ ");
                Visit(node.Arguments[1]);
                _builder.Append(")");
            }
            else if (node.Method.DeclaringType == typeof(Math) &&
                     node.Method.Name == nameof(Math.Exp))
            {
                _builder.Append("exp(");
                Visit(node.Arguments[0]);
                _builder.Append(")");
            }
            else
            {
                throw new ArgumentException($"Unknown method {node.Method}");
            }

            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            _builder.Append(node.Name);
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);

            _builder.Append(GetOperator(node.NodeType));

            Visit(node.Right);

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            WriteConstantValue(node.Value);

            return node;
        }

        private void WriteConstantValue(object obj)
        {
            switch (obj)
            {
                case string str:
                    _builder.Append('"');
                    _builder.Append(str);
                    _builder.Append('"');
                    break;
                case double doubleValue:
                    _builder.Append(_parameters.DoubleConvertion != null ? _parameters.DoubleConvertion(doubleValue) : doubleValue);
                    break;
                default:
                    _builder.Append(obj);
                    break;
            }
        }

        private static string GetOperator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Add:
                    return " + ";
                case ExpressionType.Subtract:
                    return " - ";
                case ExpressionType.Multiply:
                    return " * ";
                default:
                    return "???";
            }
        }
    }
}
