using System;
using System.Runtime.Serialization;

namespace EosSharp.Core.Exceptions
{
    /// <summary>
    /// Generic Api exception
    /// </summary>
    [Serializable]
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }

        public ApiException()
        {

        }

        /* The above code is deserializing the exception object. */
        public ApiException(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                return;

            StatusCode = info.GetInt32("StatusCode");
            Content = info.GetString("Content");
        }

        /// <summary>
        /// > The function is called when the object is serialized
        /// </summary>
        /// <param name="SerializationInfo">This is a container for all the data that needs to be
        /// serialized.</param>
        /// <param name="StreamingContext">This is a structure that contains information about the source and
        /// destination of a given serialized stream, and provides an additional caller-defined context.</param>
        /// <returns>
        /// The status code and the content of the response.
        /// </returns>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                return;

            base.GetObjectData(info, context);
            info.AddValue("StatusCode", StatusCode);
            info.AddValue("Content", Content);
        }
    }
}
