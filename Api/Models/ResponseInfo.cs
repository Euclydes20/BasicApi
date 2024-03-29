﻿using Api.Auxiliary;

namespace Api.Models
{
    public class ResponseInfo<T> : ResponseInfo
    {
        public T? Data { get; set; }
    }

    public class ResponseInfo
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public ResponseCode Code { get; set; } = ResponseCode.Success;
        public DateTime OperationDate
        {
            get
            {
                return DateTime.Now;
            }
            private set { }
        }

        private DateTime OperationRequestDate { get; set; } = DateTime.Now;
        public double OperationMilliseconds
        {
            get
            {
                return (DateTime.Now - OperationRequestDate).TotalMilliseconds;
            }
            private set { }
        }
    }
}
