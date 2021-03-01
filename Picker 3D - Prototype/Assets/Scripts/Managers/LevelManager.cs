using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Picker Picker;

    private List<LevelSection> _activelevels = new List<LevelSection>();
    private Stack<LevelSection> _passivelevels = new Stack<LevelSection>();

    private LevelSection _currentSection;
    private int _currentLevel = 0;

    public LevelSection CurrentSection => _currentSection;
    public int CurrentLevel => _currentLevel;

    //TODO Refactor this, Currently Prototype
   

    public LevelDataBase LevelDb;


    void Start()
    {

      
    }

    public bool IsDestinationReached()
    {
        return _currentSection.IsEndSection(Picker.MaxExtends());
    }

    internal void Init(ServiceLocator serviceLocator)
    {
        var entity = serviceLocator.SaveManager.Load();
        //SaveEntity entity = null;
        if (entity== null || !entity.IsSavedData)
        {
            _currentSection = Get(0);
            _currentSection.Init(LevelDb.GetLevel(0));
            Picker.Init(_currentSection.StartPosition());
        }
        else
        {
            _currentSection = Get(0, entity.LevelEntity.Position, Quaternion.identity);
            _currentLevel = entity.LevelEntity.CurrentLevel;
            _currentSection.Init(entity.LevelEntity);
            _currentSection.ChangeMaterial(LevelDb.GetLevel(entity.LevelEntity.LevelIndex - 1).BoardMaterial);
            Picker.Init(entity.PlayerEntity.Position);
        }
    }

    public bool IsLevelCompleted()
    {
        return _currentSection.IsCompleted;
    }
    public void Restart()
    {
        _currentSection.OnComplete();
        _currentSection = Get(_currentLevel);
        _currentSection.Init(LevelDb.GetLevel(_currentLevel));

        Picker.Init(_currentSection.StartPosition());
    }

    public void LoadNextLevel()
    {
        ++_currentLevel;
        var nextLevel = Get(_currentLevel);

        Attach(_currentSection, nextLevel);
        Free(_currentSection);
        _currentSection = nextLevel;
        _currentSection.Init(LevelDb.GetLevel(_currentLevel));
 
    }
 
    public void Free(LevelSection levelSection)
    {
        _activelevels.Remove(levelSection);
        _passivelevels.Push(levelSection);
        this.DelayedAction(() =>
        {
            levelSection.gameObject.SetActive(false);
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
        spawnPosition.z += to.MinExtend();

        to.transform.position = spawnPosition;
    }
}
