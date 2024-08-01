namespace AnalyticSoftware.Utilities
{
    public class DatasResponse<T>
    {
        public T[] Datas { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
