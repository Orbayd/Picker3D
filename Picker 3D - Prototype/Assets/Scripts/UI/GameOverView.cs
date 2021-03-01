using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : ViewBase
{
    [SerializeField]
    private Button RestartButton;

    [SerializeField]
    private Button NextLevelButton;

    public event Action OnRestartButtonClicked;
    public event Action OnNextLevelButtonClicked;

    private void Start()
    {
        RestartButton.onClick.AddListener(() => OnRestartButtonClicked?.Invoke());
        NextLevelButton.onClick.AddListener(() => OnNextLevelButtonClicked?.Invoke());
    }

    public void ShowButton(bool IsSuccess)
    {
        NextLevelButton.gameObject.SetActive(false);
        RestartButton.gameObject.SetActive(false);
        if (IsSuccess)
        {
            NextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            RestartButton.gameObject.SetActive(true);
        }
    } 
}
