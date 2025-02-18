namespace HotelManagement.Domain.Interfaces
{
    public interface IEmail 
    {
        void SendEmail(string to, string subject, string body);
    }
}
