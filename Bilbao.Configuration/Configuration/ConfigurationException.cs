using System;
using System.Runtime.Serialization;

namespace Bilbao.Configuration
{
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public class ConfigurationException : BilbaoException
    {
        public ConfigurationException() 
            : base()
        { }

        public ConfigurationException(String message) 
            : base(message)
        { }

        public ConfigurationException(String message, Exception innerException) 
            : base(message, innerException)
        { }

        protected ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
