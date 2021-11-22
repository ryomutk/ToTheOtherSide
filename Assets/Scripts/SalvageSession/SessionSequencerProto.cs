using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Linq;
/*
public class SessionSequencerProto:ISessionSequencer
{
    //今回かかわったすべての人
    List<SectorStep> exploredSteps = new List<SectorStep>();
    ExploreData exData;
    DataIndexer sectorIndexer;
    ArmBotData.Entity botEntity;
    SalvageEvent<ExploreArg> exploreEvent;
    //各セクターのルートステップが保存されてるやつ
    List<SectorStep> rootSteps = new List<SectorStep>();

    public SessionSequencerProto(ExploreData exploreData, DataIndexer sectorIndexer, ArmBotData.Entity botEntity, SalvageEvent<ExploreArg> exploreEvent)
    {
        this.exData = exploreData;
        this.sectorIndexer = sectorIndexer;
        this.botEntity = botEntity;
        this.exploreEvent = exploreEvent;
    }

    public void BuildSession()
    {

        SectorStep nowStep = null;

        while (botEntity.CheckMovility())
        {
            nowStep = SelectStep(nowStep);
            InteractLoop(nowStep);
        }

        //nowStep = SelectUpward(nowStep);
    }

    bool InteractLoop(SectorStep targetStep)
    {
        targetStep.ResetDurability();

        targetStep.Enter(botEntity);
        while (targetStep.durability > 0)
        {
            targetStep.Interact(botEntity);
            if (!botEntity.CheckMovility())
            {
                break;
            }
        }
        targetStep.Exit(botEntity);

        //探索を終了していない
        if (targetStep.durability > 0)
        {
            return false;
        }
        else
        {
            OnClear(targetStep);
            return true;
        }
    }

    
        SectorStep SelectUpward(SectorStep lastStep)
        {
            var step = lastStep.parent;

            return step;
        }
    
    SectorStep SelectStep(SectorStep lastStep)
    {
        SectorStep step = null;

        if (lastStep == null)
        {
            List<ISalvageData> orderedSectors = sectorIndexer.entity.OrderBy(x => ((SectorData)x).sectorNum).ToList();

            for (int i = 0; i < orderedSectors.Count; i++)
            {
                var localRoot = (orderedSectors[i] as SectorData).rootStep;
                var rootStep = new SectorStep(localRoot, exploreEvent);
                rootSteps.Add(rootStep);
            }

            step = rootSteps[0];
        }
        else if (lastStep.children.Length > 1)
        {
            //複数行き先がある場合
            var acc = 0;
            for (int i = 0; i < lastStep.children.Length; i++)
            {
                //見たものは逐一記録
                OnFound(lastStep.children[i]);
                
                //ACCはtraveledなども考慮してくれる
                var tmp = lastStep.children[i].GetAccessabliity(botEntity);

                //ここで値を入れるので、負しかない場合はいかない
                if (acc < tmp)
                {
                    acc = tmp;
                    step = lastStep.children[i];
                }

            }
        }
        else if (lastStep.children.Length == 1)
        {
            //一人しかいないなら
            if (lastStep.children[0].GetAccessabliity(botEntity) > 0)
            {
                //行けたら行く
                step = lastStep.children[0];
            }
        }
        else
        {
            var nowSector = sectorIndexer.entity.Find(x => (x as SectorData).sectorNum == lastStep.alignSector) as SectorData;
            Debug.Log("Reached To Sector Buttom");
            if (nowSector.lastStep.iD == lastStep.iD)
            {
                try
                {
                    step = rootSteps[lastStep.alignSector + 1];
                }
                catch (System.IndexOutOfRangeException)
                {
                    //最下ステップの場合
                    throw new System.NotImplementedException();
                }
            }
        }


        //下れないときは
        if (step == null)
        {
            //上る
            step = lastStep.parent;

            //おそらくどこかのRootStepで上に上がろうとしている。
            //上に上がるのは、現状ちょっとした一部屋分岐のためだけにあるので、
            //これが起こるのは何かがおかしい。
            if (step == null)
            {
                throw new System.Exception("ろとうにまよった");
            }
        }

        //選ばれた人を記録
        OnFound(step);
        return step;
    }


    //出会ったとき
    void OnFound(SectorStep step)
    {
        //まだ初期化されていない人は初期化
        if (step.state == StepState.undefined)
        {
            var state = exData.GetState(step.iD);
            var changed = false;

            //そいつが未発見の奴なら
            if (state == StepState.unfound)
            {
                state = StepState.found;
                changed = true;
            }

            if (state == StepState.undefined)
            {
                throw new System.Exception("Step State of: " + step.iD + " is undefined!");
            }

            //かかわった人間に追加
            exploredSteps.Add(step);
            step.SetState(state, changed);
        }
    }

    //たん靴終了時
    void OnClear(SectorStep step)
    {
        step.SetState(StepState.traveled);
    }
}
*/