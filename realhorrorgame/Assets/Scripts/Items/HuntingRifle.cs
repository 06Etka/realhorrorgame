using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingRifle : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] int maxAmmo;
    int ammo;
    [SerializeField] int maxMag;
    int mag;
    [SerializeField] float reloadTime;
    bool isReloading;
    [SerializeField] float fireRate;
    float timeSinceFired;
    bool isSafe = true;

    [Header("Keybinds")]
    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;
    [SerializeField] KeyCode safeKey = KeyCode.B;

    [Header("Sounds")]
    [SerializeField] AudioClip[] shootingSounds;
    [SerializeField] AudioClip[] safeOnSounds;
    [SerializeField] AudioClip[] safeOffSounds;

    private void Update()
    {
        if(Input.GetKeyDown(safeKey))
        {
            isSafe = !isSafe;
            if (isSafe)
            {
                int randomSafeOnIndex = Random.Range(0, safeOnSounds.Length);
                AudioPlayer.Instance.PlayAudio(transform, true, safeOnSounds[randomSafeOnIndex], false, 1f);
            } else if (!isSafe)
            {
                int randomSafeOffIndex = Random.Range(0, safeOffSounds.Length);
                AudioPlayer.Instance.PlayAudio(transform, true, safeOffSounds[randomSafeOffIndex], false, 1f);
            }
        }
        if(Input.GetKeyDown(reloadKey))
        {
            StartCoroutine(Reload());
        }
        if (isSafe)
        {
            return;
        }
        if(ammo <= 0)
        {
            return;
        }
        if (Input.GetKeyDown(fireKey) && Time.time > timeSinceFired)
        {
            timeSinceFired = Time.time + fireRate;
            Shoot();
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSecondsRealtime(reloadTime);
        ammo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        ammo--;
        int randShootingSoundIndex = Random.Range(0, shootingSounds.Length);
        AudioPlayer.Instance.PlayAudio(transform, true, shootingSounds[randShootingSoundIndex], false, 1f);
    }
}
