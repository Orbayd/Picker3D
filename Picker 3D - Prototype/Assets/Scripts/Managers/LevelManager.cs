using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LevelManager : ManagerBase
{
    private Picker _picker;

    private List<LevelSection> _activelevels = new List<LevelSection>();
    private Stack<LevelSection> _passivelevels = new Stack<LevelSection>();

    private LevelSection _currentSection;
    private int _currentLevel = 0;

    public LevelSection CurrentSection => _currentSection;
    public int CurrentLevel => _currentLevel;

    public LevelDataBase LevelDb;

    public bool IsDestinationReached()
    {
        return _currentSection.IsEndSection(_picker.MaxExtends());
    }

    public override void Init(GameManager serviceLocator)
    {
        _picker = serviceLocator.Picker;
        //var entity = serviceLocator.SaveManager.Load();
        SaveEntity entity = null;
        if (entity== null || !entity.IsSavedData)
        {
            _currentSection = Get(0);
            _currentSection.Init(LevelDb.GetLevel(0));
            _picker.Init(_currentSection.StartPosition());
        }
        else
        {
            _currentSection = Get(0, entity.LevelEntity.Position, Quaternion.identity);
            _currentLevel = entity.LevelEntity.CurrentLevel;
            _currentSection.Init(entity.LevelEntity);
            _currentSection.ChangeMaterial(LevelDb.GetLevel(entity.LevelEntity.LevelIndex - 1).BoardMaterial);
            _picker.Init(entity.PlayerEntity.Position);
        }
    }
    public override void UpdateState(IGameState gameState)
    {
        CurrentSection.UpdateState(gameState);
    }

    public bool IsLevelCompleted()
    {
        return _currentSection.IsCompleted;
    }
    public void Restart()
    {
        _currentSection.OnComplete();
        _currentSection.Init(LevelDb.GetLevel(_currentLevel));
        _picker.Init(_currentSection.StartPosition());
    }

    public void LoadNextLevel()
    {
        _currentSection.OnComplete();
        ++_currentLevel;
        var nextLevel = Get(_currentLevel);

        Attach(_currentSection, nextLevel);
        Free(_currentSection);
        _currentSection = nextLevel;
        _currentSection.Init(LevelDb.GetLevel(_currentLevel));
 
    }
 
    public void Free(LevelSection levelSection)
    {
        this.DelayedAction(() =>
        {
            _activelevels.Remove(levelSection);
            levelSection.gameObject.SetActive(false);
            _passivelevels.Push(levelSection);
        }, 2);
    }

    public LevelSection Get(int index)
    {
        return Get(index, Vector3.zero, Quaternion.identity);
    }

    public LevelSection Get(int index, Vector3 position, Quaternion rotation)
    {
        var nextLevel = _passivelevels.Count > 0 ? _passivelevels.Pop() :
                        Instantiate(LevelDb.LevelPrefab.GetComponent<LevelSection>(), Vector3.zero, Quaternion.identity);
        _activelevels.Add(nextLevel);

        nextLevel.transform.position = position;
        nextLevel.transform.rotation = rotation;

        return nextLevel;
    }

    public void Attach(LevelSection from, LevelSection to)
    {
        var spawnPosition = from.EndPosition();
        spawnPosition.z += to.MinForwardExtend();

        to.transform.position = spawnPosition;
    }
}
