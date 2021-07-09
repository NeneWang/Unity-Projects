// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System;

namespace Naninovel
{
    public abstract class IDEParameterPayloadAttribute : Attribute
    {
        public readonly string Value;
        public readonly int NamedIndex;

        protected IDEParameterPayloadAttribute (string value, int namedIndex = -1)
        {
            Value = value;
            NamedIndex = namedIndex;
        }
    }
}
