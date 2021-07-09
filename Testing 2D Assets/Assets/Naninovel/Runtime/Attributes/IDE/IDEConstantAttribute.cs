// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System;

namespace Naninovel
{
    /// <summary>
    /// Can be applied to a command parameter to associate specified constant value range.
    /// Used by the IDE Metadata utility to provide autocomplete information to the IDE extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class IDEConstantAttribute : IDEParameterPayloadAttribute
    {
        public readonly Type EnumType;

        /// <param name="enumType">An enum type to extract constant values from.</param>
        /// <param name="namedIndex">When applied to named parameter, specify index of the associated value (0 is for name and 1 for value).</param>
        public IDEConstantAttribute (Type enumType, int namedIndex = -1) : base(enumType.Name, namedIndex)
        {
            if (!enumType.IsEnum) throw new ArgumentException("Only enum types are supported.");
            EnumType = enumType;
        }
    }
}
