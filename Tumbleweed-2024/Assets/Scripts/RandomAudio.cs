using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    float minTime = 10f;
    float maxTime = 30f;
    float randTime;
    float countDown;

    public AudioSource AmbSounds;
    public AudioClip[] randClips;

    private void Start()
    {
        randTime = Random.Range(minTime, maxTime);
        countDown = randTime;
    }


    // Update is called once per frame
    void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
            if(countDown <= 0)
            {
                AmbSounds.PlayOneShot(randClips[Random.Range(0, randClips.Length)]);
                countDown = Random.Range(minTime, maxTime);
            }
        }
    }
}
