namespace crds_angular.Services.Interfaces
{
    public interface ITextCommunicationService
    {
        void SendTextMessage(string toPhoneNumber, string body);
    }
}