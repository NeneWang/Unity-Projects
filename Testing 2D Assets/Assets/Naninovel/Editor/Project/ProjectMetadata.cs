// Copyright 2017-2021 Elringus (Artyom Sovetnikov). All rights reserved.

using System;
using System.Collections.Generic;

namespace Naninovel
{
    [Serializable]
    public class ProjectMetadata
    {
        public List<ProjectConstant> constants;
        public List<CommandMetadata> commands;
        public List<ProjectResource> resources;
        public List<ProjectActor> actors;
        public List<string> predefinedVariables;
        public List<string> expressionFunctions;

        [Serializable]
        public class ProjectConstant
        {
            public string name;
            public string[] values;
        }

        [Serializable]
        public class ProjectResource
        {
            public string name;
            public string pathPrefix;
        }

        [Serializable]
        public class ProjectActor
        {
            public string id;
            public string pathPrefix;
            public string displayName;
            public List<string> appearances;
        }

        [Serializable]
        public class CommandMetadata
        {
            public string id;
            public string alias;
            public bool localizable;
            public string summary;
            public string remarks;
            public string examples;
            public List<ParameterMetadata> @params;
        }

        [Serializable]
        public class ParameterMetadata
        {
            public string id;
            public string alias;
            public bool nameless;
            public bool required;
            public bool localizable;
            public DataType dataType;
            public string defaultValue;
            public string summary;
            public List<ParameterPayload> payload;
        }

        [Serializable]
        public class DataType
        {
            public string kind;
            public string contentType;
        }

        [Serializable]
        public class ParameterPayload
        {
            public string key;
            public string value;
            public int namedId;
        }
    }
}
