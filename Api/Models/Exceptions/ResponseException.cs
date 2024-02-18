using Api.Auxiliary;

namespace Api.Models.Exceptions
{
    [Serializable]
    public class ResponseException : Exception
    {
        public ResponseCode ErrorCode { get; set; } = ResponseCode.UncknownError;

        // ############# BASE #############
        public ResponseException() : base() { }
        public ResponseException(string? message) : base(message) { }
        public ResponseException(string? message, Exception? innerException) : base(message, innerException) { }
        // ########## ########## ##########

        public ResponseException(ResponseCode errorCode)
        {
            ErrorCode = errorCode;
        }

        public ResponseException(string? message, ResponseCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ResponseException(Exception? innerException) : base(innerException?.Message, innerException) { }
        public ResponseException(Exception? innerException, ResponseCode errorCode) : base(innerException?.Message, innerException)
        {
            ErrorCode = errorCode;
        }

        public ResponseException(string? message, Exception? innerException, ResponseCode errorCode) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public ResponseInfo<T> ResolveResponseInfo<T>(ResponseInfo<T> responseInfo)
        {
            responseInfo ??= new ResponseInfo<T>();

            responseInfo.Success = false;
            responseInfo.Code = ErrorCode;
            responseInfo.Message = Message;

            return responseInfo;
        }

        public ResponseInfo ResolveResponseInfo(ResponseInfo responseInfo)
        {
            responseInfo ??= new ResponseInfo();

            responseInfo.Success = false;
            responseInfo.Code = ErrorCode;
            responseInfo.Message = Message;

            return responseInfo;
        }
    }
}
