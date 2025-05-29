using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObj;
    public GameObject quitMenu;
    public GameObject mainMenu;
    public GameObject yes_no;
    public Animator animatorController; 
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObj.SetActive(false);
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
        pauseMenuObj.SetActive(true);
        Time.timeScale = 0; // takes this number, times it by time.  
        Cursor.visible = true; // cursor not visible 
        Cursor.lockState = CursorLockMode.None; // keeps cursors in the middle of the screen
        isPaused = true;
    }


    public void unpauseGame()
    {
        
        closeQuitConfirm();
        closeMainMenuConfirm();
        Time.timeScale = 1; // takes this number, times it by time. 
        Cursor.visible = false; // cursor not visible 
        Cursor.lockState = CursorLockMode.Locked; // keeps cursors in the middle of the screen
        pauseMenuObj.SetActive(false);
        isPaused = false;
    }

    public void openQuitConfirm()
    {
        quitMenu.SetActive(true);
        pauseMenuObj.SetActive(false);

    }

    public void closeQuitConfirm()
    {
        quitMenu.SetActive(false);
        pauseMenuObj.SetActive(true);
        yes_no.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void openMainMenuConfirm()
    {
        mainMenu.SetActive(true);
        pauseMenuObj.SetActive(false);
    }

    public void closeMainMenuConfirm()
    {
        mainMenu.SetActive(false);
        pauseMenuObj.SetActive(true);
        yes_no.SetActive(false);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1; // takes this number, times it by time. 
    }

    public void hoverYes()
    {
       
        yes_no.SetActive(true);
        animatorController.Play("yes animation"); 
    }

    public void hoverNo()
    {
        yes_no.SetActive(true);
        animatorController.Play("no animation");
    }

    public void hoverExit()
    {
        
        yes_no.SetActive(false);
    }

}
