using System.Diagnostics;
using System;
using System.Linq;
using System.IO;

namespace ReactViewEngine
{
    /// <summary>
    /// Add a way to restart the node express server if it crashes/gets killed
    /// </summary>
    static class NodeInstance
    {
        public static NodeServerOptions Options { get; set; }

        private static bool _started;
        private static Process _expressServer;

        internal static void Start()
        {
            if (Options == null) {
                throw new ArgumentNullException(nameof(Options));
            }

            if (_started) {
                throw new Exception("Node server already started!!!");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = GetPathToNpm(),
                Arguments = $"run start-dev {Options.Port}",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = Options.PackageRoot
            };

            _expressServer = Process.Start(startInfo);

            if (_expressServer == null) {
                throw new Exception("Failed to start node express server");
            }

            _expressServer.OutputDataReceived += _expressServer_OutputDataReceived;
            _expressServer.BeginErrorReadLine();
            _expressServer.BeginOutputReadLine();
        }

        private static void _expressServer_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(string.Format("[Node Express Server] {0}", e.Data));
        }

        private static string GetPathToNpm()
        {
            if (!string.IsNullOrWhiteSpace(Options.PathToNpm)) {
                return Options.PathToNpm;
            }

            string npmPath = null;
            string nodePath = Environment.GetEnvironmentVariable("Path")
                .Split(';')
                .Where(t => t.Contains("node"))
                .FirstOrDefault();

            foreach (var file in Directory.GetFiles(nodePath)) {
                string fileName = Path.GetFileName(file)?.ToLower();
                if (fileName == "npm" || fileName == "npm.cmd") {
                    npmPath = file;
                }
            }

            return Options.PathToNpm = npmPath;
        }
    }
}