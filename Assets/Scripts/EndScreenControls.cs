using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenControls : MonoBehaviour
{
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
