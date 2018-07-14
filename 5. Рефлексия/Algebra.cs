// Вставьте сюда финальное содержимое файла Algebra.cs
using System;
using System.Linq.Expressions;
 
namespace Reflection.Differentiation
{
    public static class Algebra
    {
        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function)
        {
            return Expression.Lambda<Func<double, double>>(DifferentiateBody(function.Body), function.Parameters);
        }
 
        private static Expression DifferentiateBody(Expression body)
        {
            if (body is ConstantExpression)
                return Expression.Constant(0.0);
            if (body is ParameterExpression)
                return Expression.Constant(1.0);
 
            if (body is BinaryExpression operation)
            {
                var param1 = operation.Left;
                var param2 = operation.Right;
                
                if (body.NodeType == ExpressionType.Add)
                    return Expression.Add(DifferentiateBody(param1), DifferentiateBody(param2));
 
                if (body.NodeType == ExpressionType.Multiply)
                    return Expression.Add(Expression.Multiply(DifferentiateBody(param1), param2), 
                        Expression.Multiply(DifferentiateBody(param2), param1));
            }
 
            if (body is MethodCallExpression methodCall)
            {
                var param = methodCall.Arguments[0];
                var newMethod = body; 
 
                if (methodCall.Method.Name == "Sin")
                    newMethod = Expression.Call(typeof(Math).GetMethod("Cos", new[] { typeof(double) }), param);
 
                if (methodCall.Method.Name == "Cos")
                    newMethod = Expression.Negate(
                        Expression.Call(typeof(Math).GetMethod("Sin", new[] { typeof(double) }), param));
 
                return Expression.Multiply(newMethod, DifferentiateBody(param));
            }          
            throw new ArgumentException();
        }
    }
}