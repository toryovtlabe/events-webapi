using Business.Interfaces;

namespace Events.Adapters
{
    public class WebRootAdapter(IWebHostEnvironment env) : IWebRootPath
    {
        public string RootPath => env.WebRootPath;
    }
}
