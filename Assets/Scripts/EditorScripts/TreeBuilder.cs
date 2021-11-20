#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;

/// <summary>
/// ScriptableTreeStructureDataBaseのTreeを組んでくれる人。
/// </summary>
public static class OdinTreeBuilder
{
        //Tree構造を作る。
    //枝分かれがあった場合、先に登録されているChildが
    //便宜上MainStreamということにされる
    public static void BuildTree<T>(string basePath, T rootData, OdinMenuTree tree)
    where T:ScriptableTreeStructureDataBase
    {
        var targetData = rootData;

        if (rootData == null)
        {
            return;
        }

        tree.Add(basePath + targetData.name, targetData);

        while (targetData.rawChildren.Count != 0)
        {
            //枝分かれを見つけたら
            if (targetData.rawChildren.Count > 1)
            {
                //Main以外をSolveBranchに任せる
                SolveBranch(basePath + targetData.name + "/", targetData, tree);
            }

            //Index0のメインストリームをroot木に追加
            targetData = targetData.rawChildren[0] as T;
            tree.Add(basePath + targetData.name, targetData);
        }
    }

        //或る枝分かれから下を書く。
    //ネストが深い場合、再帰的にも呼び出される。
    static void SolveBranch<T>(string branchPath, T branchRoot, OdinMenuTree tree)
    where T:ScriptableTreeStructureDataBase
    {
        //0はメインストリームなので、それ以外のサブストリームを
        //branchRoot下の"○○route"下に○○で始まるストリームとしてつくる。

        for (int i = 1; i < branchRoot.rawChildren.Count; i++)
        {
            var subBranchRoot = branchRoot.rawChildren[i];
            var subStreamName = branchPath + "[route " + subBranchRoot.name + "]";
            var targetData = subBranchRoot as T;

            tree.Add(subStreamName + "/" + targetData.name, subBranchRoot);

            while (targetData.rawChildren.Count != 0)
            {
                //枝分かれを見つけたら
                if (targetData.rawChildren.Count > 1)
                {
                    //Main以外をSolveBranchに任せる
                    SolveBranch(subStreamName + "/" + targetData.name + "/", targetData, tree);
                }

                //Index0のメインストリームをroot木に追加
                targetData = targetData.rawChildren[0] as T;
                tree.Add(subStreamName + "/" + targetData.name, targetData);
            }
        }
    }

}

#endif