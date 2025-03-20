using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpSlider;


    private void Start()
    {
        UpdateHPSlider(1);
    }

    public void UpdateHPSlider(float ratio)
    {
        hpSlider.value = ratio;
    }
    
    public void UpdateWaveText(int wave)
    {
        waveText.text = wave.ToString();    
    }
     protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}