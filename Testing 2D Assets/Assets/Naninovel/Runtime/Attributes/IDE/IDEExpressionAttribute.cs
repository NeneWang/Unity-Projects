// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System;

namespace Naninovel
{
    /// <summary>
    /// Can be applied to a command parameter for expression functions.
    /// Used by the IDE Metadata utility to provide autocomplete information to the IDE extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class IDEExpressionAttribute : IDEParameterPayloadAttribute
    {
        public IDEExpressionAttribute (int namedIndex = -1) : base("", namedIndex) { }
    }
}
