using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianFactory : MonoBehaviour
{

    private static List<GameObject> used = new List<GameObject>();
    private static List<GameObject> free = new List<GameObject>();
    public GameObject GetGuardian()
    {
        if (free.Count == 0)
        {
            GameObject aGameObject = Instantiate(
                Resources.Load("prefabs/Guardian"), 
                new Vector3(0,1,0),
                Quaternion.identity) as GameObject;
            used.Add(aGameObject);
        }
        else
        {
            used.Add(free[0]);
            free.RemoveAt(0);
            used[used.Count - 1].SetActive(true);
            used[used.Count - 1].transform.position = Vector3.zero;
            used[used.Count - 1].transform.localRotation = Quaternion.identity;
        }
        return used[used.Count - 1];
    }

    public void FreeObj(GameObject oj)
    {
        oj.SetActive(false);
        used.Remove(oj);
        free.Add(oj);
    }
}
