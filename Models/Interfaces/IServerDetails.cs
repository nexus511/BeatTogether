using BeatTogether.Configuration.Interfaces;

namespace BeatTogether.Models.Interfaces
{
    /// <summary>
    /// Shared interface for all classes describing a server configuration.
    /// </summary>
    public interface IServerDetails : IExtendedDetails, IServerConfig
    {
    }
}
