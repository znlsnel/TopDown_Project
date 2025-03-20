using UnityEngine;

public enum UIState
{
    Home,
    Game,
    GameOver,
}

public class UIManager : MonoBehaviour
{
    HomeUI homeUI;
    GameUI gameUI;
    GameOverUI gameOverUI;

    private UIState currentUIState;

    private void Awake()
    {
        homeUI = GetComponentInChildren<HomeUI>();
        homeUI.Init(this);
        gameUI = GetComponentInChildren<GameUI>();
        gameUI.Init(this);
        gameOverUI = GetComponentInChildren<GameOverUI>();
        gameOverUI.Init(this);

        ChangeState(UIState.Home);
    }

    public void SetPlayGame()
    {
        ChangeState(UIState.Game);
    }

    public void SetGameOver()
    {
        ChangeState(UIState.GameOver);
    }

    public void ChangeWave(int waveIndex)
    {
        gameUI.UpdateWaveText(waveIndex);
    }

    public void ChangePlayerHP(float currentHp, float maxHp)
    {
        gameUI.UpdateHPSlider(currentHp / maxHp);
    } 
     public void ChangeState(UIState state)
    {
        currentUIState = state;
        homeUI.SetActive(currentUIState);
        gameUI.SetActive(currentUIState);
        gameOverUI.SetActive(currentUIState );    
    } 
}
 