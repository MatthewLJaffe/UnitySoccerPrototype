using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void AdvanceScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int idx)
    {
        SceneManager.LoadScene(idx);
    }
}
