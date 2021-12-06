using UnityEngine;
using UnityEngine.UI;

public class IslandSessionAddon : IslandMapAddon, IEventListener<SessionEventArg>, IEventListener<SelectableArg>
{
    public override PanelName name { get { return PanelName.SessionAddon; } }
    SessionInfo sessionInfo = new SessionInfo();
    [SerializeField] UIPanel summaryPanel;
    ILoadSalvageData<SessionData> summaryLoader;
    [SerializeField] Button submitButton;

    class SessionInfo
    {
        public Vector2? towardCoords = null;
        public string selectedBot = null;
        public bool filled { get { return towardCoords != null && selectedBot != null; } }
    }

    protected override void Start()
    {
        base.Start();
        if (summaryPanel is ILoadSalvageData<SessionData> loader)
        {
            summaryLoader = loader;
        }
        else
        {
#if DEBUG
            Debug.LogError("your panel is not loader");
#endif
        }

        submitButton.onClick.AddListener(()=>OnSubmit());
    }

    void OnSubmit()
    {
        if (sessionInfo != null && sessionInfo.filled)
        {
            var motCord = DataProvider.nowGameData.currentMOTHERCoordinate;
            SessionUtility.RequestSummary(sessionInfo.selectedBot,Vector2Int.RoundToInt(motCord),sessionInfo.towardCoords.Value-motCord);
        }
        else
        {
            #if DEBUG
            Debug.LogWarning("");
            #endif
        }
    }

    public ITask OnNotice(SessionEventArg arg)
    {
        if (arg.state == SessionState.summary)
        {
            summaryLoader.Load(arg.data);
        }

        return SmallTask.nullTask;
    }

    public ITask OnNotice(SelectableArg arg)
    {
        if (showing)
        {
            if (arg.data is Island island)
            {
                sessionInfo.towardCoords = DataProvider.nowGameData.map.GetCoordinate(island);
            }
            else if (arg.data is ArmBotData.Entity bot)
            {
                sessionInfo.selectedBot = bot.id;
            }
        }

        return SmallTask.nullTask;
    }

    protected override ITask Show()
    {
        EventManager.instance.Register<SessionEventArg>(this,EventName.SessionEvent);
        EventManager.instance.Register<SelectableArg>(this,EventName.SelectableEvent);
        return base.Show();
    }

    protected override ITask Hide()
    {
        EventManager.instance.Disregister<SessionEventArg>(this,EventName.SessionEvent);
        EventManager.instance.Disregister<SelectableArg>(this,EventName.SelectableEvent);
        return base.Hide();
    }
}