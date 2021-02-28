using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField]
    PoolManager _poolManager;

    [SerializeField]
    CameraManager _cameraManager;

    [SerializeField]
    LevelManager _levelManager;

    [SerializeField]
    SaveManager _saveManager;

    public StartView StartView;

    public GameOverView GameOverView;

    public PoolManager PoolManager => _poolManager;
    public CameraManager CameraManager => _cameraManager;
    public LevelManager LevelManager => _levelManager;
    public SaveManager SaveManager => _saveManager;

    public Picker Picker;

    public static ServiceLocator Instance;

    GameStateBase _gameState;
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;

        _saveManager.Init(this);
        
        _cameraManager.Init(this);
        _levelManager.Init(this);


        _gameState = new StartState(this);
        _gameState.OnEnter();
       
    }
    private void Update()
    {
        _gameState.OnExecute();
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
}
