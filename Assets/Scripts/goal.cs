using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // used to help teleport player to next level 

public class goal : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
            currentBuildIndex += 1;
            SceneManager.LoadScene(currentBuildIndex);
        }
    }
}

