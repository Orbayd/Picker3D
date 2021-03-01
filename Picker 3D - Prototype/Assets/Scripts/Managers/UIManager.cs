using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameOverView GameOverView;

    [SerializeField]
    StartView StartView;

    [SerializeField]
    GamePlayView GamePlayView;


    ServiceLocator _serviceLocator;
    public void Init(ServiceLocator serviceLocator)
    {
        GameOverView.OnNextLevelButtonClicked += OnNextLevelButtonClicked;
        GameOverView.OnRestartButtonClicked += OnRestartButtonClicked;

        _serviceLocator = serviceLocator;
    }

    private void OnRestartButtonClicked()
    {
        _serviceLocator.LevelManager.Restart();
        _serviceLocator.ChangeGameState(new GamePlayState(_serviceLocator));
    }

    private void OnNextLevelButtonClicked()
    {
        _serviceLocator.LevelManager.LoadNextLevel();
        _serviceLocator.ChangeGameState(new GamePlayState(_serviceLocator));
    }

    private void UpdateLevelInfo()
    {
        GamePlayView.SetLevel(_serviceLocator.LevelManager.CurrentLevel);
    }
    private void UpdateGameOverInfo()
    {
        if (_serviceLocator.LevelManager.IsLevelCompleted())
        {
            GameOverView.ShowButton(true);
        }
        else
        {
            GameOverView.ShowButton(false);
        }

    }
    public void UpdateState(IGameState gameState)
    {
        GamePlayView.Show(false);
        GameOverView.Show(false);
        StartView.Show(false);

        if (gameState is  GamePlayState)
        {
            GamePlayView.Show(true);
            UpdateLevelInfo();
        }
        else if (gameState is GameOverState)
        {
            GameOverView.Show(true);
            UpdateGameOverInfo();
        }
        else
        {
            StartView.Show(true);
        }
    }
 
}
