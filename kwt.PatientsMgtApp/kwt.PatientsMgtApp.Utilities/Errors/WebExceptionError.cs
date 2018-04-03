using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace kwt.PatientsMgtApp.Utilities.Errors
{
    public class PatientsMgtException : Exception
    {
        public int Code { get; set; }

        public string Severity { get; set; }

        public string Title { get; set; }

        public PatientsMgtException(int code, string severity, string title, string message)
            : base(message)
        {
            Code = code;
            Severity = severity;
            Title = title;
        }
    }

    [DataContract]
    public class WebExceptionError
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public List<ContentMessage> messages { get; set; }

        public WebExceptionError(int exceptionCode, List<ContentMessage> contentMessages)
        {
            Code = exceptionCode;
            messages = contentMessages;
        }
    }

    [DataContract]
    public class ContentMessage
    {
        [DataMember]
        public string text { get; set; }
        [DataMember]
        public String severity { get; set; }
        [DataMember]
        public String title { get; set; }
    }
}
