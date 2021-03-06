using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField] Image fade;
    [SerializeField] float fadeSpeed;
    [SerializeField] GameObject killPlayer;
    [SerializeField] private AudioSource music1;
    [SerializeField] private AudioSource music2;
    [SerializeField] private Transform endLookPos;

    private bool GameOver;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameOver)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a + Time.deltaTime * fadeSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(endLookPos.transform.position - Camera.main.transform.position);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, 1 * Time.deltaTime);

            //GameObject.FindGameObjectWithTag("Player");
            music1.volume = Mathf.Lerp(music1.volume, 0.0f, 0.01f);
            music2.volume = Mathf.Lerp(music2.volume, 0.0f, 0.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameOver = true;
        killPlayer.SetActive(false);
        //music1.Pause();
        //music2.Pause();
        //killSound.Play();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Camera.main.GetComponent<PlayerLook>().enabled = false;
        player.GetComponent<PlayerMove>().enabled = false;
        //Camera.main.transform.LookAt(transform.position);
        Invoke("Load", 1.2f);
    }

    private void Load()
    {
        SceneManager.LoadScene("GameOver");
    }
}
