using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionChange : MonoBehaviour
{
    [SerializeField] private string section;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove.section = section;
        }
    }
}
