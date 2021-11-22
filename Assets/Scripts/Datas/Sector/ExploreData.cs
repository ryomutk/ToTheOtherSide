using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEditor;

//探索情報を保持。
[CreateAssetMenu]
public class ExploreData : ScriptableObject, IPermanentData
{
    [FoldoutGroup("System"), Button]
    void ClearData()
    {
        stepdatas.Clear();
    }


    public bool UpdateData(ISalvageData rawData)
    {
        if (rawData is SessionData data)
        {
            EditorUtility.SetDirty(this);
            int nowID = default;
            StepStateData nowStateData = null;

            foreach (var arg in data.eventsOccoured)
            {
                switch (arg.type)
                {
                    case ExploreObjType.Step:
                        var tmp = (StepExArg)arg;
                        nowID = tmp.step;
                        var state = GetStateData(nowID);
                        state.state = tmp.state;
                        break;

                    case ExploreObjType.Interact:
                        var tmpAction = (StepActionArg)arg;
                        if (tmpAction.actionType == StepActionType.enter)
                        {
                            nowID = tmpAction.stepId;
                            nowStateData = GetStateData(nowID);
                        }
                        else if (tmpAction.actionType == StepActionType.cleared)
                        {
                            nowStateData.clearedCount++;
                        }

                        break;

                    case ExploreObjType.Item:
                        var itemArg = (ItemExArg)arg;
                        nowStateData.NoticeDiscover(itemArg.itemID);
                        break;
                }
            }

            AssetDatabase.SaveAssets();

            return true;
        }

        return false;
    }

    StepStateData GetStateData(int id)
    {
        var stepstate = stepdatas.Find(x => x.target == id);
        if (stepstate == null)
        {
            stepstate = new StepStateData(id);
            stepdatas.Add(stepstate);
        }

        return stepstate;
    }

    [SerializeField, ReadOnly] List<StepStateData> stepdatas;
    public StepState GetState(int id)
    {
        var stepState = stepdatas.Find(x => x.target == id);
        if (stepState != null)
        {
            return stepState.state;
        }

        return StepState.unfound;
    }

    public int[] GetFoundSteps()
    {
        var founds = stepdatas.FindAll(x => x.state >= StepState.found);
        var ids = new int[founds.Count];
        for (int i = 0; i < founds.Count; i++)
        {
            ids[i] = founds[i].target;
        }

        return ids;
    }


    [Serializable]
    class StepStateData : ISalvageData
    {
        public ReadOnlyCollection<ItemID> foundItems { get { return _foundItems.AsReadOnly(); } }
        public StepState state
        {
            get { return _state; }
            set
            {
                if (value == StepState.traveled)
                {
                    _state = StepState.revealed;
                }
                else
                {
                    _state = value;
                }
            }
        }

        public int clearedCount { get { return _count; } set { _count = value; } }
        [SerializeField] StepState _state;
        [SerializeField] List<ItemID> _foundItems = new List<ItemID>();
        [SerializeField] int _count;
        [SerializeField] int _target;
        public int target { get { return _target; } private set { _target = value; } }

        public StepStateData(int id)
        {
            this.target = id;
        }

        public bool NoticeDiscover(ItemID id)
        {
            if (!_foundItems.Contains(id))
            {
                _foundItems.Add(id);
                return true;
            }
            return false;
        }
    }
}