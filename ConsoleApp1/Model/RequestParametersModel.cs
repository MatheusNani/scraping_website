namespace ConsoleApp1.Model
{
    public class RequestParametersModel
    {
        /// <summary>
        /// Api base address
        /// </summary>
        public string APIBaseAddress { get; set; }
        /// <summary>
        /// Api method name
        /// </summary>
        public string APIMethodAddress { get; set; }
        /// <summary>
        /// Method: Post, Get, Put, Delete
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// List of name and values to header parameters
        /// </summary>
        public List<KeyValuePair<string, string>> Headers { get; set; } = new List<KeyValuePair<string, string>>();

    }
}
