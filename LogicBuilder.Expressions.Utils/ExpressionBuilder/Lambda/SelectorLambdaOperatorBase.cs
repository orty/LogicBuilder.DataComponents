﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public abstract class SelectorLambdaOperatorBase
    {
        public SelectorLambdaOperatorBase(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart selectorBody, string selectorParameterName)
        {
            SourceOperand = sourceOperand;
            SelectorBody = selectorBody;
            SelectorParameterName = selectorParameterName;
            Parameters = parameters;
        }

        public SelectorLambdaOperatorBase(IExpressionPart sourceOperand)
        {
            SourceOperand = sourceOperand;
        }

        public IExpressionPart SourceOperand { get; }
        public IExpressionPart SelectorBody { get; }
        public string SelectorParameterName { get; }
        public IDictionary<string, ParameterExpression> Parameters { get; }

        public Expression Build() => Build(SourceOperand.Build());

        protected abstract Expression Build(Expression operandExpression);

        protected Expression[] GetParameters(Expression operandExpression)
        {
            if (SelectorBody == null)
                return new Expression[0];

            return new Expression[]
            {
                GetLambdaOperatorHelper(operandExpression.GetUnderlyingElementType()).Build()
            };
        }

        protected LambdaExpression GetSelector(Expression operandExpression)
            => (LambdaExpression)GetLambdaOperatorHelper(operandExpression.GetUnderlyingElementType()).Build();

        protected SelectorLambdaOperatorHelper GetLambdaOperatorHelper(Type elementType)
            => new SelectorLambdaOperatorHelper
            (
                Parameters,
                SelectorBody,
                elementType,
                SelectorParameterName
            );
    }
}
