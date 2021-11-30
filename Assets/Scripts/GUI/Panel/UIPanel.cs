public interface IUIPanel
{
    PanelName name{get;}
    ITask Show();
    ITask Hide();
}