using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keys : MonoBehaviour
{
    public static int keysCollected = 0;
    [SerializeField] private Image key1;
    [SerializeField] private Image key2;
    [SerializeField] private Image key3;
    [SerializeField] private Image key4;
    [SerializeField] private Image key5;
    [SerializeField] private static List<Image> keys = new List<Image>();
    [SerializeField] private GameObject floorLocks;
    [SerializeField] private GameObject bars;

    private void Start()
    {
        keysCollected = 0;
        keys.Add(key1);
        keys.Add(key2);
        keys.Add(key3);
        keys.Add(key4);
        keys.Add(key5);
        floorLocks.SetActive(false);
    }

    public void AddKey(Color given)
    {
        Color newColor = new Color(given.r, given.g, given.b, 1);
        keys[keysCollected].color = newColor;
        keysCollected ++;
        if (keysCollected == 5)
        {
            floorLocks.SetActive(true);
            Lock[] locks = GameObject.FindObjectsOfType<Lock>();
            foreach (Lock l in locks)
            {
                l.FoundAllKeys();
            }
            bars.SetActive(false);
        }
    }

    public static int GetKeys()
    {
        return keysCollected;
    }
}
