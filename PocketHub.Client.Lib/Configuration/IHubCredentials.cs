namespace PocketHub.Client.Lib.Configuration
{
    /// <summary>
    /// Contains required values for connecting to a hub.
    /// </summary>
    public interface IHubCredentials
    {
        /// <summary>
        /// HTTP or HTTPS url of hub server
        /// </summary>
        string   HubServerURL    { get; }

        /// <summary>
        /// Login name of a user account in hub.
        /// </summary>
        string   LoginName       { get; }

        /// <summary>
        /// Login password of a user account in hub.
        /// </summary>
        string   LoginPassword   { get; }

        /// <summary>
        /// Common key shared between connecting clients and hub server
        /// </summary>
        string   SharedKey       { get; }
    }
}
