namespace PocketHub.Client.Lib.ServiceContracts
{
    public interface IObjectDBClient : IHubClient,
        IObjectDBReader, IObjectDBWriter
    {
    }
}
