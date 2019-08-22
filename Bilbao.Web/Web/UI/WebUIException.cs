using System;
using System.Runtime.Serialization;

namespace Bilbao.Web.UI
{
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class WebUIException : WebException
    {
        public WebUIException()
            : base()
        { }

        public WebUIException(String message)
            : base(message)
        { }

        public WebUIException(String message, Exception innerException)
            : base(message, innerException)
        { }

        protected WebUIException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
