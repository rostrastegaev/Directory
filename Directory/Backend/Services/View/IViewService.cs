using System.IO;

namespace Backend
{
    public interface IViewService
    {
        Stream GetView(string url);
    }
}
