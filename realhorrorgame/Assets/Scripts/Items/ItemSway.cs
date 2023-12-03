using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSway : MonoBehaviour
{
    public float mouseSwayAmount = 0.02f;
    public float maxSwayAmount = 0.04f;
    public float swaySpeed = 2.0f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate sway amount based on input
        float deltaX = (-mouseX * mouseSwayAmount);
        float deltaY = (-mouseY * mouseSwayAmount);

        // Limit sway amount
        deltaX = Mathf.Clamp(deltaX, -maxSwayAmount, maxSwayAmount);
        deltaY = Mathf.Clamp(deltaY, -maxSwayAmount, maxSwayAmount);

        // Calculate new position with sway
        Vector3 swayPosition = new Vector3(deltaX, deltaY, 0);
        Vector3 newPosition = Vector3.Lerp(transform.localPosition, initialPosition + swayPosition, Time.deltaTime * swaySpeed);

        // Apply the new position
        transform.localPosition = newPosition;
    }
}
