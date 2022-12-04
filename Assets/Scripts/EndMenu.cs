using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] analysisScreens;
    public int anaysisScreenToShow;
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowAnalysis()
    {
        analysisScreens[anaysisScreenToShow].SetActive(true);
    }
}
