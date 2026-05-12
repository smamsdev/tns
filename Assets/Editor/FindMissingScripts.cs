using UnityEngine;
using UnityEditor;

public class FindMissingScripts
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void Find()
    {
        Debug.Log("Searching for missing scripts...");

        GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in gos)
        {
            if (EditorUtility.IsPersistent(go))
                continue;

            Component[] components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.Log(
                        "Missing script found on: " + GetFullPath(go),
                        go
                    );
                }
            }
        }
    }

    static string GetFullPath(GameObject go)
    {
        string path = go.name;

        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = go.name + "/" + path;
        }

        return path;
    }
}