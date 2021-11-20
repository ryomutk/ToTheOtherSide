using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatedPanel : GUIPanel
{
    Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("Enter");
    }

    public override bool Hide()
    {
        if (isActive)
        {
            base.Hide();
            animator.SetTrigger("Exit");
            
            return true;
        }
        return false;
    }
}