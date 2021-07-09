// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using UniRx.Async;

namespace Naninovel.Commands
{
    /// <summary>
    /// Assigns result of a [script expression](/guide/script-expressions.md) to a [custom variable](/guide/custom-variables.md).
    /// </summary>
    /// <remarks>
    /// If a variable with the provided name doesn't exist, it will be automatically created.
    /// <br/><br/>
    /// It's possible to define multiple set expressions in one line by separating them with `;`. The expressions will be executed in sequence by the order of declaration.
    /// <br/><br/>
    /// In case variable name starts with `T_` or `t_` it's considered a reference to a value stored in 'Script' [managed text](/guide/managed-text.md) document. 
    /// Such variables can't be assigned and mostly used for referencing localizable text values.
    /// </remarks>
    [CommandAlias("set")]
    public class SetCustomVariable : Command, Command.IForceWait
    {
        /// <summary>
        /// Set expression. 
        /// <br/><br/>
        /// The expression should be in the following format: `VariableName=ExpressionBody`, where `VariableName` is the name of the custom 
        /// variable to assign and `ExpressionBody` is a [script expression](/guide/script-expressions.md), the result of which should be assigned to the variable.
        /// <br/><br/>
        /// It's also possible to use increment and decrement unary operators, eg: `@set foo++`, `@set foo--`.
        /// </summary>
        [ParameterAlias(NamelessParameterAlias), RequiredParameter, IDEExpression]
        public StringParameter Expression;

        protected ICustomVariableManager VariableManager => Engine.GetService<ICustomVariableManager>();
        protected IStateManager StateManager => Engine.GetService<IStateManager>();

        private const string assignmentLiteral = "=";
        private const string incrementLiteral = "++";
        private const string decrementLiteral = "--";
        private const string separatorLiteral = ";";

        public override async UniTask ExecuteAsync (CancellationToken cancellationToken = default)
        {
            var saveStatePending = false;
            var expressions = Expression.Value.Split(separatorLiteral[0]);
            for (int i = 0; i < expressions.Length; i++)
            {
                var expression = expressions[i];
                if (string.IsNullOrEmpty(expression)) continue;

                if (expression.EndsWithFast(incrementLiteral))
                    expression = expression.Replace(incrementLiteral, $"={expression.GetBefore(incrementLiteral)}+1");
                else if (expression.EndsWithFast(decrementLiteral))
                    expression = expression.Replace(decrementLiteral, $"={expression.GetBefore(decrementLiteral)}-1");

                var variableName = expression.GetBefore(assignmentLiteral)?.TrimFull();
                var expressionBody = expression.GetAfterFirst(assignmentLiteral)?.TrimFull();
                if (string.IsNullOrWhiteSpace(variableName) || string.IsNullOrWhiteSpace(expressionBody))
                {
                    LogErrorMsg("Failed to extract variable name and expression body. Make sure the expression starts with a variable name followed by assignment operator `=`.");
                    continue;
                }

                var result = ExpressionEvaluator.Evaluate<string>(expressionBody, LogErrorMsg);
                if (result is null) continue;

                VariableManager.SetVariableValue(variableName, result);
                saveStatePending = saveStatePending || CustomVariablesConfiguration.IsGlobalVariable(variableName);
            }

            if (saveStatePending)
                await StateManager.SaveGlobalAsync();
        }

        private void LogErrorMsg (string desc = null) => LogErrorWithPosition($"Failed to evaluate set expression `{Expression}`. {desc ?? string.Empty}");
    }
}
