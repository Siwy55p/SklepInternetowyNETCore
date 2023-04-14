using System.ServiceModel;

namespace partner_aluro.Services.Interfaces
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        int Add(int num1, int num2);
    }
}
