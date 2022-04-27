using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    private bool killed;

    [SerializeField] private AudioSource killSound;
    [SerializeField] private AudioSource music1;
    [SerializeField] private AudioSource music2;

    [SerializeField] Image fade;
    [SerializeField] float fadeSpeed;

    [SerializeField] private Monster monster;
    private float timer;

    void Update()
    {
        if (killed)
        {
            timer += Time.deltaTime;
            if (timer > 1.1f)
            {
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a + Time.deltaTime * fadeSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !killed)
        {
            killed = true;
            music1.Pause();
            music2.Pause();
            killSound.Play();

            Vector3 newCamPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.75f, Camera.main.transform.position.z);
            Camera.main.transform.position = newCamPos;
            monster.transform.LookAt(new Vector3(other.transform.position.x, monster.transform.position.y, other.transform.position.z));
            //Camera.main.transform.position = transform.position + Vector3.left * 3;
            Camera.main.transform.LookAt(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
            //Camera.main.transform.Translate(Vector3.back * 0.1f);

            monster.KilledPlayer();
            Camera.main.GetComponent<PlayerLook>().enabled = false;
            other.GetComponent<PlayerMove>().PlayerDied();
            Invoke("Restart", 5.5f);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
