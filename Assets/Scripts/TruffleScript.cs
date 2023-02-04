using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TruffleScript : MonoBehaviour
{

    //how much cover it has before it is dug out of ground
    float cover = 1f;

    AudioSource audioSource;
    bool digging = false;

    public UnityEvent<GameObject> collected;


    public ParticleSystem particleSystem;
    public ParticleSystem.EmissionModule em;

    public Rigidbody trufflePrefab;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        em = particleSystem.emission;
    }

    public void Update()
    {
        if (digging)
        {
            cover -= 1 * Time.deltaTime;
            if (cover <= 0)
            {
                Debug.Log("found it!!");
                var go = Instantiate(trufflePrefab, transform.position, transform.rotation);
                go.AddForce(Vector3.up * 4f, ForceMode.Impulse);
                collected.Invoke(gameObject);
                Destroy(gameObject);
            }
            //plays dig if it is not already playing
            PlayDigSound();
        }
    }

    void PlayDigSound()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }


    public void StopDigging()
    {
        digging = false;
        em.rateOverTime = 0f;
    }

    public void StartDigging()
    {
        digging = true;
        em.rateOverTime = 50;
    }


}
