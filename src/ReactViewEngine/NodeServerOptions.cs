namespace ReactViewEngine
{
    public class NodeServerOptions
    {
        public bool IsDevelopment { get; set; }
        /// <summary>
        /// The root of the app package, where package.json lives
        /// </summary>
        public string PackageRoot { get; set; }
        public string PathToNpm { get; set; }
        public int Port { get; set; }
    }
}
