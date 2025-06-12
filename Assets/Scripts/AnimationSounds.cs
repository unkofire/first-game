using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// NOTE: PUBLIC variables can have their values edited on unity, PRIVATE cannot. 

public class AnimationSounds : MonoBehaviour
{



    private PlayerControl player; 


    // Start is called before the first frame update
    void Start()
    {
       
        player = GetComponent<PlayerControl>();
       
    }

    // Update is called once per frame
    void Update()
    {
   


    }

   

    public void FootstepSound()
    {
        player.playFootstep(); 
    }
}
