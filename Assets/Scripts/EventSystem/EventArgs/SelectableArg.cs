public class SelectableArg:SalvageEventArg
{
    public ISalvageData data{get;private set;}

    public SelectableArg(ISalvageData data)
    {
        this.data = data;
    }
}