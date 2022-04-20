using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverseFade : MonoBehaviour
{
    [SerializeField] Image fade;

    void Update()
    {
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a - Time.deltaTime * 0.2f);
    }
}
