namespace AnalyticSoftware
{
    public class DataResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
