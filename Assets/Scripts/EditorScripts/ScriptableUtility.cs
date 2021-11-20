using System.Collections.Generic;
using UnityEngine;
/*
public static class ScriptableUtility
{
    public static Dictionary<string, T[]> LoadScriptablesWithDirectory<T>(string loadPath)
    where T : ScriptableObject
    {
        var result = new Dictionary<string, T[]>();

        var directorys = System.IO.Directory.GetDirectories(Application.dataPath + "/Resources/" + loadPath);

        foreach (var directory in directorys)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(directory) + "/";
            var files = Resources.LoadAll<T>(loadPath + name);

            result[name] = new T[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                result[name][i] = files[i];
            }
        }

        return result;
    }

    public static Dictionary<string, Object[]> LoadScriptablesWithDirectory(string loadPath)
    {
        var result = new Dictionary<string, Object[]>();

        var directorys = System.IO.Directory.GetDirectories(Application.dataPath + "/Resources/" + loadPath);

        foreach (var directory in directorys)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(directory) + "/";
            var files = Resources.LoadAll<Object>(loadPath + name);

            result[name] = new Object[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                result[name][i] = files[i];
            }
        }

        return result;
    }
}
*/