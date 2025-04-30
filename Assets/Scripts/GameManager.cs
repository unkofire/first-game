using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public CollectibleBehavior[] collectibles; // array
    private int maxNumCollectibles;
    private int currentNumCollectibles;
    public TMP_Text collectibleTxt;
    public GameObject goal; 
    
    void Start()
    {
        collectibles = FindObjectsOfType<CollectibleBehavior>(); // OfType functions always have <type> before parenthesis. Only use FindObjectsofType in start function to prevent lag.
        Debug.Log("Number of coins found: " + collectibles.Length);
        maxNumCollectibles = collectibles.Length;
        collectibleTxt.text = currentNumCollectibles + "/" + maxNumCollectibles;
        goal.SetActive(false); // turns off goal game object 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collected()
    {
        currentNumCollectibles += 1;
        collectibleTxt.text = currentNumCollectibles + "/" + maxNumCollectibles; //update text  to show collected coins 

        if (currentNumCollectibles >= maxNumCollectibles) 
        {
            goal.SetActive(true); 
        }
    }
}
