using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }
    [SerializeField] AudioSource audioPlayerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAudio(Transform _spawnPos, bool isAttached, AudioClip _clip, bool _useIsPlaying, float _timeToRemove)
    {
        AudioSource audioPlayer = Instantiate(audioPlayerPrefab, _spawnPos.position, Quaternion.identity);
        if (isAttached)
        {
            audioPlayer.transform.parent = _spawnPos;
        }
        if(_useIsPlaying)
        {
            if(!audioPlayer.isPlaying)
            {
                audioPlayer.PlayOneShot(_clip);
            }
        } else
        {
            audioPlayer.PlayOneShot(_clip);
        }
        Destroy(audioPlayer, _timeToRemove);
    }
}
