using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundEvents : MonoBehaviour
{

public AudioSource audio1;
public AudioClip shoot;
public AudioClip recharge;
public AudioClip walkgrass;
public AudioClip walkCave;
public AudioClip walkWater;

[Header("Variables")]
public bool grass;
public bool cave;
public bool water;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public void walkEvent()
    {
      if(grass)
      {
        audio1.clip = walkgrass;
        audio1.Play();
        audio1.volume = 0.8f;
      }

      if(cave)
      {
        //walkCave.Play();
        audio1.clip = walkCave;
        audio1.Play();
        audio1.volume = 1f;
      }

      if(water)
      {
        //walkWater.Play();
        audio1.clip = walkWater;
        audio1.Play();
        audio1.volume = 1f;
      }

    }

public void Shoot()
    {
     audio1.clip  = shoot;
     audio1.Play();
     audio1.volume = 0.25f;
    }

    public void Stop()
    {
     audio1.Stop();
     audio1.volume = 1f;
    }

public void Recharge()
    {
     audio1.clip  = recharge;
     audio1.Play(); 
     audio1.volume = 0.8f;
    }
}
