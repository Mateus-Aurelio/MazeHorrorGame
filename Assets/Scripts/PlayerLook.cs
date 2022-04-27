using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;
    [SerializeField] Transform flashlight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //mouseSensitivity = 300;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        float moveMult = 0.5f;
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            moveMult = 1.0f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveMult = 2.25f;
            }
        }

        flashlight.localRotation = Quaternion.Lerp(
            flashlight.localRotation,
            Quaternion.Euler(
            Mathf.Sin(Time.time * 3) * moveMult - 1.5f, // up down
            Mathf.Sin(Time.time * 1.5f ) * moveMult, // left right
            0), 0.03f);
    }
}
