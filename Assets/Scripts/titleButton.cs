using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleButton : MonoBehaviour
{
    public GameObject handImage; 

    // Start is called before the first frame update
    void Start()
    {
        handImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void hover()
    {
        print("mouse Enter");
        handImage.SetActive(true);
        handImage.GetComponent<RectTransform>().position = new Vector3(this.GetComponent<RectTransform>().position.x -236, this.GetComponent<RectTransform>().position.y + 2, 0); // -236 and 2 is offset from image to button
    }

    public void hoverExit()
    {
        print("mouse Exit");
        handImage.SetActive(false);
    } 



}
