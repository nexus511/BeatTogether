using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatTogether.Models.Interfaces;

namespace BeatTogether.Providers.Interfaces
{
    /// <summary>
    /// Class providing access to the server selection and the list of
    /// selectable servers.
    /// </summary>
    public interface IServerDetailProvider
    {
        /// <summary>
        /// This event will be triggered after the selection of the active
        /// server has been changed.
        /// </summary>
        event EventHandler<IServerDetails> ServerSelectionChanged;

        /// <summary>
        /// This event will be triggered when the list of available servers
        /// has been updated.
        ///
        /// This is the case, when servers have been added or removed from
        /// thge list using the appropriate methods.
        /// </summary>
        event EventHandler<List<IServerDetails>> ServerListChanged;

        /// <summary>
        /// Contains the list of all configured servers that can be selected
        /// by the user.
        /// </summary>
        List<IServerDetails> Servers { get; }

        /// <summary>
        /// The server that has been currently selected by the user.
        /// </summary>
        IServerDetails SelectedServer { get; }

        /// <summary>
        /// API call to change the selection for the current server.
        ///
        /// This call will also update the UI selection, so use this to
        /// change the selection from an external source.
        /// </summary>
        /// <param name="server">The server to be set as active.</param>
        void SetSelectedServer(IServerDetails server);

        /// <summary>
        /// Adds a new server to the list of available servers.
        /// 
        /// The server will only be added to the temporary server list and
        /// not stored to the configuration file.
        /// 
        /// It will be removed on the restart of the game.
        /// </summary>
        /// <param name="server">The server to be added.</param>
        /// <param name="select">Selects the server.</param>
        /// <returns>True, if the server has been added.</returns>
        bool AddTemporaryServer(IServerDetails server, bool select = false);

        /// <summary>
        /// Removes a temporary server from the list of servers.
        /// 
        /// This does only allow to add or remove temporary servers.
        /// </summary>
        /// <param name="server">The server to be removed.</param>
        /// <returns>True, if the server has been removed.</returns>
        bool RemoveTemporaryServer(IServerDetails server);
    }
}
