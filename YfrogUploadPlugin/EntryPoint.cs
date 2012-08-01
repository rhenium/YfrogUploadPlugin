using System;
using System.ComponentModel.Composition;
using System.Reflection;
using Acuerdo.Plugin;
using Inscribe.Plugin;

namespace YfrogUploader
{
    [Export(typeof(IPlugin))]
    public class EntryPoint : IPlugin
    {
        public string Name
        {
            get { return "yfrog Upload plugin"; }
        }

        public Version Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public void Loaded()
        {
            UploaderManager.RegisterUploader(new YfrogUploader());
        }

        public IConfigurator ConfigurationInterface
        {
            get { return null; }
        }
    }
}
