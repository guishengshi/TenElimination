using UnityEngine;
using System.Collections;

public class Helper {
    private static string mPrefabPathInResources = "Prefabs/";
    private static string mSpritePathInResources = "Pictures/";
    private static string mEffectPathInResources = "Effect/"; 
    
    public static Object GetPrefab (string name, System.Type type) {
        return Resources.Load(mPrefabPathInResources + name, type);
    }

    public static GameObject GetPrefabTypeOfGameObject(string name) {
        GameObject g = Resources.Load <GameObject> (mPrefabPathInResources + name);
        GameObject newG = GameObject.Instantiate(g);
        newG.transform.localScale = Vector3.one;
        newG.transform.localPosition = Vector3.zero;
        return newG;
    }

    public static void SetParent(Transform child, Transform parent) {
        child.SetParent(parent);
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
    }

    public static Sprite GetSprite(string name) {
        Sprite s = Resources.Load <Sprite> (mSpritePathInResources + name);
        return s;
    }

    public static GameObject GetEffect(string name) {
        GameObject g = Resources.Load<GameObject>(mEffectPathInResources + name);
        GameObject newG = GameObject.Instantiate(g);
        newG.transform.localScale = Vector3.one;
        newG.transform.localPosition = new Vector3(0, 0, -3);
        return newG;
    }

    public static bool CheckNetworkConnecting() {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            return false;
        }
        return true;
    }
}
