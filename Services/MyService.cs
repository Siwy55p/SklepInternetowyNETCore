using partner_aluro.Services.Interfaces;
using System.ServiceModel;

namespace partner_aluro.Services
{
    public class MyService : IMyService
    {
        public int Add(int num1, int num2)
        {
            return num1 + num2;
        }
    }
}
