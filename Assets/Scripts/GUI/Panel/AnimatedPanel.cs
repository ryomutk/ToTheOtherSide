using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIAnimator))]
public class AnimatedPanel : GUIPanel
{
    UIAnimator uiAnimator;
    protected override void Start()
    {
        base.Start();
        uiAnimator = GetComponent<UIAnimator>();
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
        
        var task = uiAnimator.Draw();

        yield return new WaitUntil(()=>task.compleated);
    }

    public override bool Hide()
    {
        if (isActive)
        {
            base.Hide();
            uiAnimator.Hide();
            
            return true;
        }
        return false;
    }
}