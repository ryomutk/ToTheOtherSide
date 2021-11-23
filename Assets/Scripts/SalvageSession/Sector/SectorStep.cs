using UnityEngine;
using System;

public class SectorStep
{
    public int id{get;set;}
    public int resourceLv{get;set;}
    public int radius{get;set;}
}

/*
//ゲーム内で使われるSectorのStepインスタンス。
//Dataから生成される。
//ここ意外とSectorStepDataの依存をなくす。
public class SectorStep
{
    [Serializable]
    public class StepBehaviourBase : ISalvageData
    {
        public int searchPbonus { get { return _searchPbonus; } set { _searchPbonus = value; } }
        //なんか作用させたいときあれば
        //botのsearchPに足される
        protected int _searchPbonus = 0;
        [SerializeField] protected int baseAccesability = 20;
        [SerializeField] InterctBehaviour interactor;




        //ここで返した値が大きいものほど行きやすくなる
        //負の場合はいけない。
        public virtual int GetAccessability(ArmBotData.Entity bot, SectorStep step)
        {
            var rand = SessionConfig.instance.accRandomizer;

            if (baseAccesability > 0)
            {
                var acc = baseAccesability + UnityEngine.Random.Range(-rand, rand);

                //いけるいけないは反転しない
                if (acc <= 0)
                {
                    acc = 1;
                }

                return acc;
            }
            else
            {
                return baseAccesability;
            }
        }

        //そのステップに入った時の処理。
        public virtual void OnEnter(ArmBotData.Entity bot, SectorStep step)
        {

        }

        /// <summary>
        /// Interact時の処理
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="step"></param>
        /// <returns>進んだdepth</returns>
        public virtual void OnInteract(ArmBotData.Entity bot, SectorStep step)
        {
            step.durability -= (int)(bot.searchP * SessionConfig.instance.GetMultiplier(step.state) + searchPbonus);
            if (step.durability < 0)
            {
                step.durability = 0;
            }
            bot.genki -= SessionConfig.instance.interactFuel;
            interactor.OnInteract(bot, step);
        }

        //出口処理
        /// <summary>
        /// 必ずしも探索が終わっているわけではないので注意
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="step"></param>
        public virtual void OnExit(ArmBotData.Entity bot, SectorStep step)
        {

        }
    }

    public string name { get; private set; }
    public int iD { get; }
    public StepBehaviourBase behaviour { get; }
    public StepState state { get; private set; }
    public SectorStep parent { get; set; }
    public SectorStep[] children { get; private set; }
    public int alignSector { get; }
    SalvageEvent<ExploreArg> exploreEvent;



    public SectorStep(StepConfig data, SalvageEvent<ExploreArg> exploreEvent)
    {
        throw new NotImplementedException();

        this.exploreEvent = exploreEvent;
        this.behaviour = data.behaviour;

        this.iD = data.iD;

        var rawChildren = data.GetChildren();
        this.children = new SectorStep[rawChildren.Length];
        for (int i = 0; i < rawChildren.Length; i++)
        {
            this.children[i] = new SectorStep(rawChildren[i], exploreEvent, this);
        }
    }

    protected SectorStep(SectorStepData data, SalvageEvent<ExploreArg> exploreEvent, SectorStep parent)
    {
        throw new NotImplementedException();

        this.parent = parent;
        this.exploreEvent = exploreEvent;
        this.behaviour = data.behaviour;
        this.alignSector = data.alignSector;


        this.iD = data.iD;

        var rawChildren = data.GetChildren();
        this.children = new SectorStep[rawChildren.Length];
        for (int i = 0; i < rawChildren.Length; i++)
        {
            this.children[i] = new SectorStep(rawChildren[i], exploreEvent, this);
        }
    }

    public int GetAccessabliity(ArmBotData.Entity bot)
    {
        var acc = behaviour.GetAccessability(bot, this);
        var arg = new StepActionArg(StepActionType.confirm, 0, iD);
        exploreEvent.Notice(arg);
        if (acc < 0)
        {
            SetState(StepState.unavalable);
        }

        return acc;
    }

    public void SetState(StepState state, bool changed = true)
    {
        //単なる初期化か変化か
        //unfound -> foundの時だけ初期化でもtrue(これに対応するためだけのもの。よくないね)
        if (changed)
        {
            exploreEvent.Notice(new StepExArg(iD, state));
        }
        this.state = state;
    }

    public void Enter(ArmBotData.Entity bot)
    {
        var arg = new StepActionArg(StepActionType.enter, 0, iD);
        exploreEvent.Notice(arg);
        behaviour.OnEnter(bot, this);
    }

    /// <summary>
    /// いんてらくと！！
    /// </summary>
    /// <param name="bot"></param>
    /// <returns>進んだdepth</returns>
    public float Interact(ArmBotData.Entity bot)
    {
        throw new NotImplementedException();
        
        behaviour.OnInteract(bot, this);

        var arg = new StepActionArg(StepActionType.interact, deltaDepth, iD);
        exploreEvent.Notice(arg);

        return deltaDepth;
        
    }

    public void Exit(ArmBotData.Entity bot)
    {
        StepActionArg arg;

        arg = new StepActionArg(StepActionType.leave, 0, iD);
        exploreEvent.Notice(arg);

        behaviour.OnExit(bot, this);
    }
}
*/