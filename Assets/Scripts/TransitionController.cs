using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class TransitionController : MonoBehaviour
{
    public UnityEvent onCountDownStart;
    public UnityEvent onOptionStart;
    public UnityEvent onMenuStart;
    public UnityEvent onGameStart;
    public static Action gameStart = delegate { };
    private bool _gameBegin;
    private bool _optionBegin;
    private bool _menuBegin = true;
    [SerializeField] private TextMeshProUGUI text;
    public Slider speedSlider;
    public Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI speedSliderText;
    [SerializeField] private TextMeshProUGUI volumeSliderText;
    [SerializeField] private TextMeshProUGUI menuOptionText;
    [SerializeField] private TextMeshProUGUI menuStartText;
    [SerializeField] private TextMeshProUGUI menuQuitText;
    private int sliderChoice = 0;
    private int menuChoice = 0;

    private void Update()
    {
        //check for pressing space key
        // if (!_gameBegin && Input.GetButtonDown("Jump"))
        // {
        //     onCountDownStart.Invoke();
        //     _gameBegin = true;
        //     StartCoroutine(CountDown());
        // }
        
        if (_menuBegin) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                menuChoice += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                menuChoice -= 1;
            }
            menuChoice = menuChoice % 3;
            if (menuChoice == 0) {
                menuOptionText.fontStyle = (FontStyles)FontStyle.Bold;
                menuStartText.fontStyle = (FontStyles)FontStyle.Normal;
                menuQuitText.fontStyle = (FontStyles)FontStyle.Normal;
                // option menu
                if (Input.GetButtonDown("Jump")) {
                    _menuBegin = false;
                    _optionBegin = true;
                    onOptionStart.Invoke();
                }
            }
            if (menuChoice == 1) {
                menuOptionText.fontStyle = (FontStyles)FontStyle.Normal;
                menuStartText.fontStyle = (FontStyles)FontStyle.Bold;
                menuQuitText.fontStyle = (FontStyles)FontStyle.Normal;
                
                if (!_gameBegin && Input.GetButtonDown("Jump")) {
                    _menuBegin = false;
                    onCountDownStart.Invoke();
                    _gameBegin = true;
                    StartCoroutine(CountDown());
                }
            }
            if (menuChoice == 2) {
                menuOptionText.fontStyle = (FontStyles)FontStyle.Normal;
                menuStartText.fontStyle = (FontStyles)FontStyle.Normal;
                menuQuitText.fontStyle = (FontStyles)FontStyle.Bold;
                if(Input.GetButtonDown("Jump")) {
                    Application.Quit();
                }
            }
        }

        // back to main menu
        if (Input.GetKeyDown(KeyCode.B)) {
            onMenuStart.Invoke();
            _optionBegin = false;
            _menuBegin = true;
        }

        if (_optionBegin) {
            Slider slider;
            if (sliderChoice == 0) {
                slider = speedSlider;
                speedSliderText.fontStyle = (FontStyles)FontStyle.Bold;
                volumeSliderText.fontStyle = (FontStyles)FontStyle.Normal;
            }
            else {
                slider = volumeSlider;
                speedSliderText.fontStyle = (FontStyles)FontStyle.Normal;
                volumeSliderText.fontStyle = (FontStyles)FontStyle.Bold;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                sliderChoice += 1;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                sliderChoice -= 1;
            }
            sliderChoice = sliderChoice % 2;

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                slider.value += 1;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                slider.value -= 1;
            }

        }
    }

    private IEnumerator CountDown()
    {
        text.text = "3";
        yield return new WaitForSeconds(1);
        text.text = "2";
        yield return new WaitForSeconds(1);
        text.text = "1";
        yield return new WaitForSeconds(1);
        onGameStart.Invoke();
        gameStart.Invoke();
    }
}
