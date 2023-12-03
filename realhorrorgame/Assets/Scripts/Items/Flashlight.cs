using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Light flashLight;
    [SerializeField] KeyCode flashlightKey = KeyCode.F;
    [SerializeField] float batteryConsumeAmt;
    float battery = 100f;
    bool isFlashlightOpen = false;

    private void Update()
    {
        flashLight.enabled = isFlashlightOpen;
        if (battery <= 0) 
        {
            isFlashlightOpen = false;
            return; 
        }
        if (Input.GetKeyDown(flashlightKey))
        {
            isFlashlightOpen = !isFlashlightOpen;
        }
        if(isFlashlightOpen)
        {
            battery -= batteryConsumeAmt * Time.deltaTime;
        }
    }

}
