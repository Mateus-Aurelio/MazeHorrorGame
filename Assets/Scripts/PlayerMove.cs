using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public static string section = "Bot";
    [SerializeField] private CharacterController controller;
    private float speed;

    [SerializeField] private List<AudioClip> steps = new List<AudioClip>();
    [SerializeField] private AudioClip gravelStep;
    private AudioSource source;
    private float footstepTimer;
    private float footstepTimerMax;
    private bool onGravel;

    private bool sprinting;
    private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float minStaminaToRun;
    [SerializeField] private float staminaGainRate;
    [SerializeField] private float staminaLoseRate;

    [SerializeField] private Slider staminaSlider;

    [SerializeField] private Image fill;
    [SerializeField] private Image background;
    [SerializeField] private Image backgroundNoRun;

    [SerializeField] private Color fillDefault;
    [SerializeField] private Color fillNoRun;
    [SerializeField] private Color fillHidden;
    [SerializeField] private Color backgroundDefault;
    [SerializeField] private Color backgroundHidden;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] AudioSource drone;

    private void Start()
    {
        staminaSlider.maxValue = maxStamina;
        stamina = maxStamina;
        fill.color = fillHidden;
        background.color = backgroundHidden;
        backgroundNoRun.color = backgroundHidden;
        source = GetComponent<AudioSource>();
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift)) && stamina > minStaminaToRun)
        {
            sprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || stamina == 0)
        {
            sprinting = false;
        }
        staminaSlider.value = stamina;
        if (sprinting)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                speed = Mathf.Lerp(speed, 9.5f, 0.04f);
                stamina -= Time.deltaTime * staminaLoseRate;
            }
            else
            {
                speed = Mathf.Lerp(speed, 6f, 0.04f);
                stamina -= Time.deltaTime * staminaLoseRate;
            }
        }
        else
        {
            speed = Mathf.Lerp(speed, 3f, 0.04f);
            stamina += Time.deltaTime * staminaGainRate;
        }
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        if (stamina == maxStamina)
        {
            fill.color = Color.Lerp(fill.color, fillHidden, 0.01f);
            background.color = Color.Lerp(background.color, backgroundHidden, 0.01f);
            backgroundNoRun.color = Color.Lerp(backgroundNoRun.color, backgroundHidden, 0.01f);
        }
        else
        {
            if (stamina < minStaminaToRun)
            {
                fill.color = Color.Lerp(fill.color, fillNoRun, 0.01f);
                backgroundNoRun.color = Color.Lerp(backgroundNoRun.color, backgroundDefault, 0.05f);
            }
            else
            {
                fill.color = Color.Lerp(fill.color, fillDefault, 0.01f);
                backgroundNoRun.color = Color.Lerp(backgroundNoRun.color, backgroundHidden, 0.05f);
            }
            background.color = Color.Lerp(background.color, backgroundDefault, 0.05f);
            
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        controller.Move(move * speed * Time.deltaTime);

        //footstepTimerMax = (15 - speed) / 14;
        footstepTimerMax = 1 - (speed / 15);
        if (move.magnitude > 0)
        {
            footstepTimer += Time.deltaTime;
        }
        if (footstepTimer > footstepTimerMax)
        {
            footstepTimer = 0;
            Footstep();
        }

        /*Monster monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
        if (Vector3.Distance(monster.transform.position, transform.position) <= 75 * .15f)
        {
            Debug.Log("Within walk range");
        }
        else if (Vector3.Distance(monster.transform.position, transform.position) <= 75 * .35f)
        {
            Debug.Log("Within run range");
        }
        else
        {
            Debug.Log("out of range");
        }*/

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                drone.volume = 0.2f;
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                drone.volume = 0.1f;
                pauseMenu.SetActive(true);
            }
        }
    }

    private void Footstep()
    {
        float volume = 0.15f;
        if (sprinting)
        {
            volume = 0.35f;
        }

        if (!onGravel)
        {
            source.PlayOneShot(steps[Random.Range(0, steps.Count)], volume);

            Monster monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
            if (Vector3.Distance(monster.transform.position, transform.position) <= 75 * volume)
            {
                monster.HeardNoise(transform.position);
            }
        }
        else
        {
            source.PlayOneShot(gravelStep, volume + 0.1f);
            Monster monster = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
            if (Vector3.Distance(monster.transform.position, transform.position) <= 75 * volume + 0.1f)
            {
                monster.HeardNoise(transform.position);
            }
        }
    }

    public void PlayerDied()
    {
        Color invisible = new Color(0, 0, 0, 0);
        fill.color = invisible;
        background.color = invisible;
        backgroundNoRun.color = invisible;
        Destroy(this);
    }

    public void Gravel(bool given)
    {
        onGravel = given;
    }
}