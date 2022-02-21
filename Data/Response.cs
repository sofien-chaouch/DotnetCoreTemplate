using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace PlatformService{
    public class Response<T>{


        public Response(T data) {
            Message = "success";
            Data = data;
            StatusCode = 200;
        }
        public Response( T data, String message, bool errors, int statusCode ){
            Data = data;
            Message = message;
            Errors = errors;
            StatusCode = statusCode;
        }
        public T Data { get; set; }
        public int StatusCode{ get; set; }
        public bool Errors { get; set; }
        public string Message { get; set; }
        public JObject headers{ get; set;  }

    }

    public class CustomLog{
        public DateTime _dateTime{ get; set; }
        public String Message{ get; set; }
        public String Type{ get; set; }
        public LogLevel loglevel{ get; set; }
        public CustomLog(String message , string type , LogLevel _loglevel){
            _dateTime = DateTime.Now;
            Message = message;
            Type = type;
            loglevel = _loglevel;
        }
        public CustomLog(String message , string type ){
            _dateTime = DateTime.Now;
            Message = message;
            Type = type;
            loglevel = LogLevel.Error ;
        }
        public String GetLog( ){
            return this.loglevel.ToString() + "  " + this._dateTime.ToString() + "  " + this.Message + " " + this.Type ;
        }
    }
}
