namespace Sofia.Connect.State
{
    public class ServiceDetailArgs
    {
        public string ContractName { get; private set; }
 
        public ServiceDetailArgs(string contract)
        {
            ContractName = contract;
        }
    }
}