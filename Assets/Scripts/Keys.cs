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

    [SerializeField] private AudioSource unlockSource;

    [SerializeField] private Transform monsterRunToBarsPos;

    [SerializeField] private Text keysText;

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

    private void Update()
    {
        keysText.color = new Color(keysText.color.r, keysText.color.g, keysText.color.b, keysText.color.a - Time.deltaTime * 0.2f);
    }

    public void AddKey(Color given)
    {
        Color newColor = new Color(given.r * .7f, given.g * .7f, given.b * .7f, 1);
        keys[keysCollected].color = newColor;
        keysCollected ++;
        keysText.text = "Keys: " + keysCollected + "/?";
        keysText.color = new Color(keysText.color.r, keysText.color.g, keysText.color.b, 0.9f);
        if (keysCollected == 5)
        {
            floorLocks.SetActive(true);
            Lock[] locks = GameObject.FindObjectsOfType<Lock>();
            foreach (Lock l in locks)
            {
                keysText.text = "Keys: 5/5";
                l.FoundAllKeys();
                Invoke("MonsterHear", 1.0f);
                unlockSource.Play();
            }
            bars.SetActive(false);
        }
    }

    public static int GetKeys()
    {
        return keysCollected;
    }

    private void MonsterHear()
    {
        GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>().HeardNoise(monsterRunToBarsPos.position);
    }
}
