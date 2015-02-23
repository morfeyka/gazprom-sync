namespace Sofia.Scheduling.data
{
    public interface IDataConverter
    {
        string ConverterName { get; }
        bool CanBeUsedFor(string tag);
        double Convert(double val,string key);
    }
}