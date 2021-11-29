using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ShowType
{
    overrap,    //重ねてShow
    swap        //出てるもの全部隠してshow
} 

public enum PanelAction
{
    show,
    hide
}

public class UIManager:Singleton<UIManager>
{
    List<GUIPanel> panels = new List<GUIPanel>();
    List<GUIPanel> safeArea = new List<GUIPanel>();

    public bool ShowPanel(PanelName panel,ShowType type = ShowType.overrap)
    {
        var target = panels.Find(x => x.name.HasFlag(panel));
        if(target == null)
        {
            return false;
        }


        switch(type)
        {
            case ShowType.overrap:
            break;
            
            case ShowType.swap:
            HideAllPanel();
            break;
        }

        return target.Show();
    }

    public bool HidePanel(PanelName panel)
    {
        var target = panels.Find(x => x.name.HasFlag(panel));

        if(target == null)
        {
            return false;
        }


        
        return target.Hide();
    }

    public void HideAllPanel()
    {
        panels.ForEach(x => x.Hide());
    }

    public void ToggleSafeArea(bool show)
    {
        if(show)
        {
            safeArea.ForEach(x => x.Show());
        }
        else
        {
            safeArea.ForEach(x => x.Hide());
        }
    }


    public void RegisterPanel(GUIPanel panel)
    {
        panels.Add(panel);
    }

    public bool DisregisterPanel(GUIPanel panel)
    {
        return panels.Remove(panel);
    }
}