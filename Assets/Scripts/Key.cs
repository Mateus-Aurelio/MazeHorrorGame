using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float bobSpeed;
    [SerializeField] private Color color;
    [SerializeField] private GameObject prefab;

    void Start()
    {

    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * bobSpeed + 3, transform.position.z);
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectKey();
        }
    }

    private void CollectKey()
    {
        Monster monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
        if (Vector3.Distance(monster.transform.position, transform.position) <= 20)
        {
            monster.HeardNoise(transform.position);
        }
        Lock[] locks = GameObject.FindObjectsOfType<Lock>();
        foreach (Lock l in locks)
        {
            l.FoundKey(color);
        }
        GameObject.FindObjectOfType<Keys>().AddKey(color);
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
