#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine.AddressableAssets;
using UnityEditor;
using UnityEngine;


//TreeStructuredDataの構築&ViewをするWindow
//初めのデータにはrootというファイル名を付けること。
public abstract class TreeStructuredDataEditor<T> : OdinMenuEditorWindow
where T : ScriptableTreeStructureDataBase
{

    protected abstract string targetLabel { get; }


    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;


        LoadScriptables(tree);

        return tree;
    }



    protected void LoadScriptables(OdinMenuTree tree)
    {
        var indexer = Addressables.LoadAssetsAsync<T>(targetLabel, (x) => AppendData(tree, x));

    }

    void AppendData(OdinMenuTree tree, T data)
    {
        if (data.isRoot)
        {
            OdinTreeBuilder.BuildTree<T>(targetLabel, data, tree);
        }
    }




    protected override void OnBeginDrawEditors()
    {
        var selected = this.MenuTree.Selection;
        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;


        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("Add content")))
        {
            ScriptableObjectCreator.ShowDialog<T>("Assets/Resources/Scriptables/" + targetLabel, obj =>
             {
                 if (selected != null)
                 {
                     var parent = selected.SelectedValue as T;

                     obj.SetParent(parent);
                     base.TrySelectMenuItemWithObject(obj);
                 }
             });
        }

        if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delete")))
        {
            if (selected.SelectedValue != null)
            {
                var obj = selected.SelectedValue as T;
                //自分の子を自分の親に託す
                if (obj.rawParent != null)
                {
                    foreach (var child in obj.rawChildren)
                    {
                        child.SetParent(obj.rawParent);
                    }

                    obj.SetParent(null);
                }
                else
                {
                    foreach (var child in obj.rawChildren)
                    {
                        child.SetParent(null);
                    }
                }


                var path = AssetDatabase.GetAssetPath(obj);
                AssetDatabase.DeleteAsset(path);
            }
        }


        //みなしごを作る
        if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create New Root")))
        {
            ScriptableObjectCreator.ShowDialog<T>("Assets/Resources/Scriptables/" + targetLabel, obj =>
             {
                 obj.isRoot = true;
                 base.TrySelectMenuItemWithObject(obj);
             });
        }

        if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
        {
            Debug.Log("refleshed");
            ForceMenuTreeRebuild();
        }

        SirenixEditorGUI.EndHorizontalToolbar();
    }

}
#endif


/*
void BuildWindow()
{

    foreach (var data in scriptableDict)
    {
        if (data.Value != null)
        {
            foreach (var scriptable in data.Value)
            {
                InspectObjectInDropDown(scriptable);
            }
        }
    }
}
*/