using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        currentBuildIndex += 1;
        SceneManager.LoadScene(currentBuildIndex);
    }
}
