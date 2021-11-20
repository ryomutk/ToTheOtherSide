#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Sirenix.Utilities.Editor;
using UnityEngine;


public class ItemDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Window/DataEditor/ItemDataEditor")]
    static void OpenWindow()
    {
        var w = GetWindow<ItemDataEditor>("ItemDataEditor");
        w.Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;

        var itemDatas = Addressables.LoadAssetsAsync<ItemData>("Item", (x) => AddData(tree, x));

        return tree;
    }

    void AddData(OdinMenuTree tree, ItemData item)
    {
        tree.Add(item.type.ToString() + "/" + item.name, item, item.thumbNail);
    }

    protected override void OnBeginDrawEditors()
    {
        if (this.MenuTree != null)
        {
            var selected = this.MenuTree.Selection;

            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;


            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("NewItem")))
            {
                ScriptableObjectCreator.ShowDialog<ItemData>("Assets/Addressables/Scriptables/ItemData/", obj =>
                 {
                     if (selected != null)
                     {
                         var parent = selected.SelectedValue as SectorData;
                         base.TrySelectMenuItemWithObject(obj);
                     }
                 });
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

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                ForceMenuTreeRebuild();
            }

            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
#endif