using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationSounds))] // requires animation sounds 

public class AnimationController : MonoBehaviour
{

    public Animator anim; // animator control of the player
    public float animBlendSpeed = 0; //How fast animation transitions from one animations to the next.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // changes animations based on where the player is moving. 
    public void AnimationUpdate(int x, int z)
    {
        //NOTE: This is for blending: aka, this makes the animations transition from one to another without snapping
        float animX = (anim.GetFloat("right and left"));
        float animZ = (anim.GetFloat("fwd and bkwd"));
        if (animX < x) // if we are trying to walk right, and we were walking left
        {
            anim.SetFloat("right and left", animX + animBlendSpeed * Time.deltaTime);
        }
        else if (animX > x) // if we are trying to walk left, and we were walking right
        {
            anim.SetFloat("right and left", animX - animBlendSpeed * Time.deltaTime);
        }
        if (Mathf.Abs(animX - x) < 0.1 && x == 0) // Fixes the weird jittering problem. 
        {
            anim.SetFloat("right and left", 0f);
        }


        if (animZ < z) // if we are trying to walk forward, and we were walking backwards
        {
            anim.SetFloat("fwd and bkwd", animZ + animBlendSpeed * Time.deltaTime);
        }
        else if (animZ > z) // if we are trying to walk backwards, and we were walking forwards
        {
            anim.SetFloat("fwd and bkwd", animZ - animBlendSpeed * Time.deltaTime);
        }
        if (Mathf.Abs(animZ - z) < 0.1 && z == 0) // prevents jittering 
        {
            anim.SetFloat("fwd and bkwd", 0f);
        }
    }
    // checks if player is grounded to stop playing the jump animation or to start playing the jump animation WHEN we jump. 
    public void UpdateAnimationGrounded(bool isGrounded)
    {
        anim.SetBool("grounded", isGrounded); // "grounded" is the name in the animation parameter
    }
}
