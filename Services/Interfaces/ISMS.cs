namespace partner_aluro.Services.Interfaces
{
    public interface ISMS
    {

        string sendSMS(string apiKey, string numbers ,string message, string sender);
    }
}
