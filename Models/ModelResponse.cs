namespace MyGarden_API.Models
{
    public class ModelResponse<T> : Exception
    {
        public int HTTP { get; set; }

        public string CustomMessage { get; set; }

        public string Title { get; set; }

        public T Value { get; set; }
    }
}
