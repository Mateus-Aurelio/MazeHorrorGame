using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] GameObject locked;
    [SerializeField] GameObject unlocked;
    [SerializeField] Color color;

    void Start()
    {
        LockLock();
    }

    void Update()
    {
        
    }

    public void FoundKey(Color given)
    {
        if (given == color)
        {
            UnlockLock();
        }
    }

    public void FoundAllKeys()
    {
        locked.SetActive(false);
        unlocked.SetActive(false);
    }

    void LockLock()
    {
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    void UnlockLock()
    {
        locked.SetActive(false);
        unlocked.SetActive(true);
    }
}
