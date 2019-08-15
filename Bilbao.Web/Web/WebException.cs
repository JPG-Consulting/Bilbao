using System;
using System.Runtime.Serialization;

namespace Bilbao.Web
{
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class WebException : BilbaoException
    {
        public WebException()
            : base()
        { }

        public WebException(String message)
            : base(message)
        { }

        public WebException(String message, Exception innerException)
            : base(message, innerException)
        { }

        protected WebException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
