using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections;

//とりあえず今話題のTimelineで動きを実装。
[RequireComponent(typeof(PlayableDirector), typeof(SignalReceiver))]
public class TimelinePanel : GUIPanel
{
    PlayableDirector director;

    protected override void Start()
    {
        base.Start();
        director = GetComponent<PlayableDirector>();
    }

    public void Pause()
    {
        director.Pause();
    }

    public override bool Show()
    {
        if (!isActive)
        {
            base.Show();
            StartCoroutine(ShowField());
            return true;
        }

        return false;
    }

    protected override IEnumerator ShowField()
    {
        yield return StartCoroutine(base.ShowField());
        gameObject.SetActive(true);
        director.time = 0;
        director.Play();
    }

    public override bool Hide()
    {
        if (director.state == PlayState.Paused && isActive)
        {
            base.Hide();
            director.Resume();

            /*
            if (field != null)
            {
                //これ消える前に消えるのでは？
                //多分Directorの終了を待ったほうがいい
                field.Unload();
            }
            */
            
            return true;
        }

        return false;
    }
}