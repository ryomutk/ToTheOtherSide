using UnityEngine;

//Animatorで表示するタイプに使う
[RequireComponent(typeof(Animator))]
public class UIAnimator : MonoBehaviour, IUIRenderer
{
    const string showKey = "Enter";
    const string hideKey = "Exit";

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //トリガーを出したアニメーションが終わってるか確認するための簡単な
    class AnimationTriggerHandler
    {
        public AnimationTriggerHandler(Animator target, string trigger)
        {
            prevHash = target.GetCurrentAnimatorStateInfo(0).fullPathHash;
            targetHash = target.GetCurrentAnimatorStateInfo(0).fullPathHash;
            this.target = target;
            target.SetTrigger(trigger);
        }
        Animator target;
        int prevHash;
        int targetHash;

        bool playing = false;

        public bool CheckFinished()
        {
            if(target==null){return true;}

            var state = target.GetCurrentAnimatorStateInfo(0);
            if(!playing&&targetHash == state.fullPathHash)
            {
                //最初のPlayingのフラグを立てる
                playing = true;
            }
            else if(playing && (targetHash != state.fullPathHash || state.normalizedTime >= 1))
            {
                //Playingのフラグが立ってるときは、hashが目的と違う場合or normalizedTimeが終了を示していたらtrue
                target = null; //どうせトラッシュされるだろうけど一応ちゃんと参照を切っておく
                return true;
            }

            return false;
        }
    }


    public ITask Draw()
    {
        var player = new AnimationTriggerHandler(animator,showKey);


        return new TaskBase(player.CheckFinished);
    }

    public ITask Hide()
    {
        var player = new AnimationTriggerHandler(animator,hideKey);
        
        return new TaskBase(player.CheckFinished);
    }
}