public class  SystemEventArg:SalvageEventArg
{
    public GameState state{get;}
    public SystemEventArg(GameState state)
    {
        this.state = state;
    }
}