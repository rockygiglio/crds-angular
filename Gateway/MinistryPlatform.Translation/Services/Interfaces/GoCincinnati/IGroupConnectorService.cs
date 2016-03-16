namespace MinistryPlatform.Translation.Services.Interfaces.GoCincinnati
{
    public interface IGroupConnectorService
    {
        int CreateGroupConnector(int registrationId);
        int CreateGroupConnectorRegistration(int groupConnectorId, int registrationId);
    }
}