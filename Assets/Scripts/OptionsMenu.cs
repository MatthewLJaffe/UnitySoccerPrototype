using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private bool resetValues;

    private void Awake()
    {
        if (resetValues)
        {
            gameSettings.volume = gameSettings.defaultVolume;
            gameSettings.speed = gameSettings.defaultSpeed;
            speedSlider.value = gameSettings.speed;
            volumeSlider.value = gameSettings.volume;
        }
    }

    public void UpdateGameSettings()
    {
        gameSettings.volume = volumeSlider.value;
        gameSettings.speed = speedSlider.value;
    }

    public void AddToVolume(float value)
    {
        volumeSlider.value += value;
        UpdateGameSettings();
    }

    public void AddToSpeed(float value)
    {
        speedSlider.value += value;
        UpdateGameSettings();
    }
}
