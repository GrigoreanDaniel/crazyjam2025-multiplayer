using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializePrefabs()
    {
        Debug.Log("Initializing Prefabs...");

        GameObject[] prefabs = Resources.LoadAll<GameObject>("InstantiateOnLoad/");

        foreach (GameObject prefab in prefabs)
        {
            Object.Instantiate(prefab);

        }

        Debug.Log("Initializing Prefabs done.");
    }

}
