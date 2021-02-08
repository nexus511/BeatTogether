using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BeatTogether.Models;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace BeatTogether.Configuration
{
    internal class PluginConfiguration
    {
        public static PluginConfiguration Instance { get; set; }

        public virtual string SelectedServer { get; set; } = "BeatTogether";

        [NonNullable, UseConverter(typeof(CollectionConverter<ServerConfig, List<ServerConfig>>))]
        public virtual List<ServerConfig> Servers { get; set; } = new List<ServerConfig>()
        {
            new ServerConfig()
            {
                ServerName = "BeatTogether",
                HostName = "btogether.xn--9o8hpe.ws",
                StatusUri = "http://btogether.xn--9o8hpe.ws/status"
            }
        };

        public virtual void OnReload()
        {
        }

        public virtual void Changed()
        {
        }

        public virtual void CopyFrom(PluginConfiguration other)
        {
            SelectedServer = other.SelectedServer;
            Servers = other.Servers;
        }
    }
}
