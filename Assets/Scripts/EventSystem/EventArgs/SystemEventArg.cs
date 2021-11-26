public class  SystemEventArg:SalvageEventArg
{
    public GameState state{get;}
    public SystemEventArg(GameState stete)
    {
        this.state = state;
    }
}