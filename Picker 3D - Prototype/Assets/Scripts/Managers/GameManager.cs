using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    ProbPooler _pooler;

    [SerializeField]
    CameraManager _cameraManager;

    [SerializeField]
    LevelManager _levelManager;

    [SerializeField]
    SaveManager _saveManager;

    [SerializeField]
    UIManager _uiManager;

    public ProbPooler Pooler => _pooler;
    public CameraManager CameraManager => _cameraManager;
    public LevelManager LevelManager => _levelManager;
    public SaveManager SaveManager => _saveManager;
    public UIManager UIManager => _uiManager;

    public Picker Picker;

    public static GameManager Instance;

    GameStateBase _gameState;
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;

        _saveManager.Init(this);
        _uiManager.Init(this);
        _cameraManager.Init(this);
        _levelManager.Init(this);

        _gameState = new StartState(this);
        _gameState.OnEnter();

        this.DelayedAction(() =>
        {
            

        }, 2.0f);
       
       
    }
    private void Update()
    {
        _gameState?.OnExecute();
    }

    
    public void ChangeGameState(GameStateBase gamestate)
    {
        _gameState.OnExit();
        _gameState = gamestate;
        _gameState.OnEnter();
    }
    private void OnDisable()
    {
       
    }

    private void OnApplicationQuit()
    {
        _saveManager.Save();
    }
    private void OnApplicationPause()
    {
        _saveManager.Save();
    }
}
