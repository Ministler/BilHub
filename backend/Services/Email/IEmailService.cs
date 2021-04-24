namespace backend.Services.Email
{
    public interface IEmailService
    {
        void Send(Message message);
    }
}