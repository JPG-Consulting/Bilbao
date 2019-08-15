using System;
using System.Runtime.Serialization;

namespace Bilbao
{
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class BilbaoException : Exception
    {
        public BilbaoException() 
            : base()
        { }
 
        public BilbaoException(String message) 
            : base(message)
        { }

        public BilbaoException(String message, Exception innerException) 
            : base(message, innerException)
        { }

        protected BilbaoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
