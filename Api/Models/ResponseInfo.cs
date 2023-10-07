namespace Api.Models
{
    public class ResponseInfo<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public DateTime OperationDate
        {
            get
            {
                return DateTime.Now;
            }
            private set { }
        }
    }
}
