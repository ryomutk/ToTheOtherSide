public class SessionEventArg:ISalvageEventArg
{
    public SessionEventArg(SessionState state,SessionData data)
    {
        this.state = state;
        this.data = data;
    }

    public SessionState state{get;}

    //!!実験中!!
    //現在の対象のSessionData。写して保持しないこと(DataManagerの管理外なので)
    public SessionData data{get;}
}

public enum SessionState
{
    request,
    summary,
    start,
    compleate
}