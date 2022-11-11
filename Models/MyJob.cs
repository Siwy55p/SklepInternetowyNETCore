namespace partner_aluro.Models
{
    public class MyJob
    {
        public MyJob(Type type, string expression)
        {
            //Common.Logs($"My job at " + DateTime.Now.ToString("dd-mm-yyyy hh:mm:ss"), "My Job" + DateTime.Now.ToString("hhmmss"));

            Type = type;
            Expression = expression;
        }

        public Type Type { get; } 
        public string Expression { get; }
    }
}
