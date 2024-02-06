using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    [Serializable]
    public class CustomHttpException : Exception
    {
        //Клас, де зберігаються помилки
        public HttpStatusCode StatusCode { get; set; }

        //конструктор
        public CustomHttpException() { }

        //Функція з передачі повідомлення
        public CustomHttpException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        //Функція з передачі повідомлення про інші, внутрішні помилки
        public CustomHttpException(string message, Exception inner, HttpStatusCode statusCode) : base(message, inner)
        {

        }
        //protected CustomHttpException(
        //  System.Runtime.Serialization.SerializationInfo info,
        //  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
