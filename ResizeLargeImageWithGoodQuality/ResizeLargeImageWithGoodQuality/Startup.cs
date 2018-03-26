using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResizeLargeImageWithGoodQuality.Startup))]
namespace ResizeLargeImageWithGoodQuality
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
