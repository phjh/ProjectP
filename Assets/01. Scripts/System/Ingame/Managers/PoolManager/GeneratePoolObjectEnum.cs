using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class GeneratePoolObjectEnum : Editor
{
    public static PoolingListSO SO;

    [MenuItem("Tools/GenerateEnum")]
    static void GenerateEnum()
    {
        SO = GameManager.Instance.PoolingSO;

        StringBuilder sb = new StringBuilder();
        List<PoolingPair> list1 = SO.PoolObjectLists;
        sb.Append("public enum PoolObjectListEnum \n{\n");
        sb.Append($"\tNone = 0,\n");
        foreach (var pair in list1)
        {
            sb.Append($"\t{pair.name},\n");
        }
        sb.Append("}\n\n");

        List<EffectPoolingPair> list2 = SO.PoolEffectLists;
        sb.Append("public enum PoolEffectListEnum \n{\n");
        sb.Append($"\tNone = 0,\n");
        foreach (var pair in list2)
        {
            sb.Append($"\t{pair.name},\n");
        }
        sb.Append("}\n\n");

        List<UIPoolingPair> list3 = SO.PoolUILists;
        sb.Append("public enum PoolUIListEnum \n{\n");
        sb.Append($"\tNone = 0,\n");
        foreach (var pair in list3)
        {
            sb.Append($"\t{pair.name},\n");
        }
        sb.Append("}\n\n");

        string enumLocation = Application.dataPath + "\\01. Scripts\\System\\Ingame\\Managers\\PoolManager\\PoolObjectEnum.cs";
        Equals(enumLocation, sb);
        File.WriteAllText(enumLocation, sb.ToString());

        Debug.Log("Generate Enum has been Successful");
    }

}

#endif