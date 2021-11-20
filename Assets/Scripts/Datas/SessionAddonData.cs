using System;

public interface ISessionDataAddon
{

}

public class SessionAddonData
{
    public ISalvageData data{get;private set;}
    public DateTime publishedTime{get;private set;}

    public SessionAddonData()
    {
        publishedTime = DateTime.Now;
    }
}