using System;

namespace BeatTogether.Models
{
    /// <summary>
    /// Class holding custom enumerations.
    /// </summary>
    public class ConfigFlags
    {
        /// <summary>
        /// Enumeration defining supported server features.
        /// </summary>
        [Flags]
        public enum ServerFeatures : int
        {
            None = 0,
            QuickPlay = 1,
            CreateGame = 2,
            JoinGame = 4
        }
    }
}
