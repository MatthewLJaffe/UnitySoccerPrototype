using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class EndScreenControls : MonoBehaviour
{
    
    public UnityEvent onAnalysis;
    public UnityEvent onEndMenu;
    private bool _endBegin = true;
    private bool _AnalysisBegin;
    private int menuChoice = 0;
    private int picChoice = 0;
    [SerializeField] private TextMeshProUGUI retryText;
    [SerializeField] private TextMeshProUGUI analysisText;
    [SerializeField] private TextMeshProUGUI quitText;
    public GameObject[] Images;
    private void Update()
    {
        if (_endBegin) {
            onEndMenu.Invoke();
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                menuChoice += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                menuChoice -= 1;
            }
            
            menuChoice = menuChoice % 4;
            if (menuChoice == 0) {
                retryText.fontStyle = (FontStyles)FontStyle.Bold;
                analysisText.fontStyle = (FontStyles)FontStyle.Normal;
                quitText.fontStyle = (FontStyles)FontStyle.Normal;
                if (Input.GetButtonDown("Jump")) {
                    SceneManager.LoadScene(0);
                }
            }
            if (menuChoice == 1) {
                retryText.fontStyle = (FontStyles)FontStyle.Normal;
                analysisText.fontStyle = (FontStyles)FontStyle.Bold;
                quitText.fontStyle = (FontStyles)FontStyle.Normal;
                
                if (Input.GetButtonDown("Jump")) {
                    onAnalysis.Invoke();
                    _endBegin = false;
                    _AnalysisBegin = true;
                }
            }
            if (menuChoice == 2) {
                retryText.fontStyle = (FontStyles)FontStyle.Normal;
                analysisText.fontStyle = (FontStyles)FontStyle.Normal;
                quitText.fontStyle = (FontStyles)FontStyle.Bold;
                if(Input.GetButtonDown("Jump")) {
                    Application.Quit();
                }
            }
        }

        
        if (_AnalysisBegin) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                Images[picChoice].SetActive(false);
                picChoice += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                Images[picChoice].SetActive(false);
                picChoice -= 1;
            }
            picChoice = picChoice % 3;
            Images[picChoice].SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.N)) {
                _AnalysisBegin = false;
                _endBegin = true;
                onEndMenu.Invoke();
            }
        }
    }
}
