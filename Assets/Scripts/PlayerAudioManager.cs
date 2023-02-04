using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{

    public AudioClip[] oinkSounds;
    public AudioClip[] rootSounds;
    public AudioClip[] sniffSounds;
    //the audio source to play the sounds
    AudioSource audioSource;

    //read the player state from this to play the right sounds
    public PlayerMovement playerMovement;

    float oinkDelay = 2f;
    float oinkTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
        switch (playerMovement.playerState)
        {
            case PlayerState.idle:
                PlayOinkSounds();
                break;
            case PlayerState.sniffing:
                PlaySniffingSounds();
                break;
            case PlayerState.rooting:
                PlayRootingSounds();
                break;
            default:
                break;
        }
    }




    void PlaySniffingSounds()
    {
        if (!audioSource.isPlaying)
        {
            int soundToPlay = Random.Range(0, sniffSounds.Length);
            audioSource.PlayOneShot(sniffSounds[soundToPlay]);
        }
    }

    void PlayRootingSounds()
    {
        if (!audioSource.isPlaying)
        {
            int soundToPlay = Random.Range(0, rootSounds.Length);
            audioSource.PlayOneShot(rootSounds[soundToPlay]);
        }
    }

    void PlayOinkSounds()
    {
        oinkTimer += Time.deltaTime;
        if (!audioSource.isPlaying && oinkTimer > oinkDelay)
        {
            int soundToPlay = Random.Range(0, oinkSounds.Length);
            audioSource.PlayOneShot(oinkSounds[soundToPlay]);
            oinkDelay = Random.Range(1f, 3f);
            oinkTimer = 0f;
        }
    }







}
