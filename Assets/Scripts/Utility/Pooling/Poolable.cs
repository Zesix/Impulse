using UnityEngine;

public class Poolable : MonoBehaviour
{
    // Used to map this object to a prefab in the PoolData class
    public string Key;
    public bool IsPooled;
}
