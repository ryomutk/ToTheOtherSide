#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;
public class SecotorEditor : OdinMenuEditorWindow
{
    const string scriptablesFolder = "Assets/Addressables/Scriptables/";



    [MenuItem("Window/DataEditor/SectorEditor")]
    static void OpenWindow()
    {
        var w = GetWindow<SecotorEditor>("SectorEditor");
        w.Show();
    }



    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;

        var task = Addressables.LoadAssetsAsync<SectorData>("SectorData", (x) => LoadSectorOnTree(tree, x));
        task.Completed += (x) => { OnFinishLoad(); };

        return tree;
    }

    void OnFinishLoad()
    {
        var items = base.MenuTree.MenuItems;
        items = items.OrderBy(x => (x.Value as SectorData).sectorNum).ToList();
        base.MenuTree.MenuItems.Clear();
        foreach (var item in items)
        {
            base.MenuTree.MenuItems.Add(item);
        }
    }

    void LoadSectorOnTree(OdinMenuTree tree, SectorData data)
    {
        tree.Add(data.name, data);
        OdinTreeBuilder.BuildTree<SectorStepData>(data.name + "/", data.rootStep, tree);
    }

    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected;
        try
        {
            selected = this.MenuTree.Selection;
        }
        catch (System.NullReferenceException)
        {
            return;
        }

        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;


        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("NewSector")))
        {
            ScriptableObjectCreator.ShowDialog<SectorData>(scriptablesFolder, obj =>
             {
                 if (selected != null)
                 {
                     var parent = selected.SelectedValue as SectorData;
                 }

                 ScriptableObjectCreator.ShowDialog<SectorStepData>(scriptablesFolder, root =>
                 {
                     root.isRoot = true;
                     obj.SetRoot(root);
                     base.TrySelectMenuItemWithObject(obj);
                 });
             });
        }

        /*
        if (SirenixEditorGUI.ToolbarButton(new GUIContent("SetBehaviour")))
        {
            if (selected.SelectedValue is SectorStepData data)
            {
                ScriptableObjectCreator.ShowDialog<SectorStep.StepBehaviourBase>("Assets/Addressables/Scriptables/", obj =>
                 {
                     data.behaviour = obj;
                 }
                );
            }
        }
        */

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("NewStep")))
        {
            if (selected.SelectedValue != null && selected.SelectedValue is SectorStepData stepData)
            {
                ScriptableObjectCreator.ShowDialog<SectorStepData>(scriptablesFolder, obj =>
                 {

                 });
            }

        }

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete")))
        {
            if (selected.SelectedValue != null)
            {
                var obj = selected.SelectedValue as SectorData;
                //自分の子を自分の親に託す
                var path = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.DeleteAsset(path);
            }
        }

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("ReconnectSectors")))
        {
            for (int i = 0; i < base.MenuTree.MenuItems.Count - 1; i++)
            {
                var parentItem = base.MenuTree.MenuItems.Find(x => ((SectorData)x.Value).sectorNum == i);
                var childItem = base.MenuTree.MenuItems.Find(x => ((SectorData)x.Value).sectorNum == i + 1);
                Debug.Log(parentItem);
                Debug.Log(childItem);
                if(childItem != null && parentItem!= null)
                {
                    (childItem.Value as SectorData).rootStep.SetParent(((SectorData)childItem.Value).lastStep);
                }
            }
        }

        if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
        {
            ForceMenuTreeRebuild();
        }

        SirenixEditorGUI.EndHorizontalToolbar();
    }
}
#endif