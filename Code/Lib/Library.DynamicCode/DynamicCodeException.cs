﻿using System;
using System.Runtime.Serialization;

namespace Library.DynamicCode
{
    [Serializable]
    public class DynamicCodeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DynamicCodeException()
        {
        }

        public DynamicCodeException(string message) : base(message)
        {
        }

        public DynamicCodeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DynamicCodeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExportAttribute : Attribute
    {
    }

    public class GenerateAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ChecklArgsNulAttribute : Attribute
    {
    }
}