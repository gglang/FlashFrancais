using System;
using System.Runtime.Serialization;

namespace FlashFrancais
{
    [Serializable]
    internal class InvalidDataBaseOperationException : Exception
    {
        public InvalidDataBaseOperationException()
        {
        }

        public InvalidDataBaseOperationException(string message) : base(message)
        {
        }

        public InvalidDataBaseOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDataBaseOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}