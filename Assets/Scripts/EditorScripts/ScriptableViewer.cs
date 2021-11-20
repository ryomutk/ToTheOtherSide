# if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

//指定したScriptableObjectを列挙して編集できるようにするエディタ拡張
public abstract class ScriptableViewer<T> : OdinMenuEditorWindow
where T : ScriptableObject
{
    protected abstract string targetPath { get; }
    [SerializeField]
    List<DataCell<T>> dataCells = new List<DataCell<T>>();

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(true);
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;

        LoadScriptables(tree);

        return tree;
    }

    
    protected void LoadScriptables(OdinMenuTree tree)
    {
        dataCells.Clear();
        var directorys = System.IO.Directory.GetDirectories(Application.dataPath + "/Resources/Scriptables/" + targetPath);

        foreach (var directory in directorys)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(directory);
            //var res = Resources.LoadAll("Scriptables/" + targetPath + name + "/", typeof(T)).Cast<T>().ToArray();
            //var cell = new DataCell<T>(res,name);
            //dataCells.Add(cell);

            tree.AddAllAssetsAtPath(name,"Assets/Resources/"+"Scriptables/" + targetPath + name + "/");            
        }
    }

    [System.Serializable]
    class DataCell<M>
    {
        string directory;
        [BoxGroup("$directory")]
        [SerializeField,InlineEditor] M[] datas;

        public DataCell(M[] datas,string directory)
        {
            this.directory = directory;
            this.datas = datas;
        }
    }
    
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
}

#endif