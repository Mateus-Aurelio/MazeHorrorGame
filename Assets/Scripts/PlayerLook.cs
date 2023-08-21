using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;
    [SerializeField] Transform flashlight;
    private float yPosOffset;

    private float moveMult = 0.5f;
    private float headBobSpeed = 5;
    private float headBobHeight = 0.15f;

    private float mouseX;
    private float mouseY;

    private Vector3 transformPositionGoal;
    private Vector3 flashlightPositionGoal;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = 300;
        yPosOffset = transform.position.y;
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        moveMult = 0.5f;
        headBobSpeed = 2;
        headBobHeight = 0.1f;
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            headBobSpeed = 5;
            moveMult = 1.0f;
            headBobHeight = 0.2f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveMult = 2.25f;
                headBobSpeed = 10;
                headBobHeight = 0.3f;
            }
        }

        transformPositionGoal = new Vector3(transform.position.x, yPosOffset + headBobHeight * Mathf.Sin(Time.time * headBobSpeed), transform.position.z);
        transform.position = Vector3.Lerp(transform.position, transformPositionGoal, 0.1f);

        flashlightPositionGoal = new Vector3(transform.position.x, yPosOffset, transform.position.z);
        flashlight.transform.position = Vector3.Lerp(flashlight.transform.position, flashlightPositionGoal, 0.1f);

        flashlight.localRotation = Quaternion.Lerp(
            flashlight.localRotation,
            Quaternion.Euler(
            Mathf.Sin(Time.time * 3) * moveMult - 1.5f, // up down
            Mathf.Sin(Time.time * 1.5f ) * moveMult, // left right
            0), 0.03f);
    }
}
