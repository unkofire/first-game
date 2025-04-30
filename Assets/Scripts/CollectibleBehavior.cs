using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    private GameManager GM; 
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>(); // equal game manager to GM variable. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
        GM.Collected();
    }
}
