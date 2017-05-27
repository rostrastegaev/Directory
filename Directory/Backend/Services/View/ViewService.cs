using System.Collections.Generic;
using System.IO;

namespace Backend
{
    public class ViewService : IViewService
    {
        private Dictionary<string, string> _files;
        private string _rootFolder;

        public ViewService(string rootFolder)
        {
            _rootFolder = rootFolder;
            _files = new Dictionary<string, string>();
        }

        public void Init()
        {
            DirectoryInfo directory = new DirectoryInfo(_rootFolder);
            ReadDirectory(directory, "views");
        }

        public Stream GetView(string url)
        {
            return File.OpenRead(_files[url]);
        }

        private void ReadDirectory(DirectoryInfo directory, string urlPart)
        {
            foreach (var file in directory.EnumerateFiles())
            {
                if (!file.Extension.Equals(".html"))
                {
                    continue;
                }
                _files.Add($"{urlPart}/{file.Name.Replace(".html", string.Empty)}", file.FullName);
                foreach (var childDirectory in directory.EnumerateDirectories())
                {
                    ReadDirectory(childDirectory, $"{urlPart}/{childDirectory.Name}");
                }
            }
        }
    }
}
