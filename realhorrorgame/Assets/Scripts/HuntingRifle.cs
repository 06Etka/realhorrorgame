using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingRifle : MonoBehaviour
{
    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] float fireRate;
    float timeSinceFired;

    private void Update()
    {
        if(Input.GetKeyDown(fireKey) && Time.time > timeSinceFired)
        {
            timeSinceFired = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        
    }
}
