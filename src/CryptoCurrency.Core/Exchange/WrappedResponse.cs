namespace CryptoCurrency.Core.Exchange
{
    public enum WrappedResponseStatusCode
    {
        Ok = 0,
        ApiError = 2,
        FatalError = 3
    }

    public class WrappedResponse<T>
    {
        public WrappedResponseStatusCode StatusCode { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public T Data { get; set; }
    }
}
