using System;
using System.Runtime.Serialization;

namespace Bilbao.Data
{
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class DataException : BilbaoException
    {
        public DataException() 
            : base()
        { }

        public DataException(String message) 
            : base(message)
        { }

        public DataException(String message, Exception innerException) 
            : base(message, innerException)
        { }

        protected DataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
