using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausemenu;
    public GameObject quitMenu;
    public GameObject mainMenu;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pausemenu.SetActive(false);
        quitMenu.SetActive(false);
        mainMenu.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            
            if (isPaused == true)
            {
                unpauseGame(); 
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void pauseGame()
    {
        pausemenu.SetActive(true);
        Time.timeScale = 0; // takes this number, times it by time.  
        Cursor.visible = true; // cursor not visible 
        Cursor.lockState = CursorLockMode.None; // keeps cursors in the middle of the screen
        isPaused = true;
    }


    public void unpauseGame()
    {
        pausemenu.SetActive(false);
        Time.timeScale = 1; // takes this number, times it by time. 
        Cursor.visible = false; // cursor not visible 
        Cursor.lockState = CursorLockMode.Locked; // keeps cursors in the middle of the screen
        isPaused = false;
    }

    public void OpenQuitConfirm()
    {
        quitMenu.SetActive(true);
        pausemenu.SetActive(false);

    }

    
}
