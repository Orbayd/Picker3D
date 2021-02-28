using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<LevelSection> _levelPrefabs;

    private List<LevelSection> _activelevels = new List<LevelSection>();
    private List<LevelSection> _passivelevels = new List<LevelSection>();

    private LevelSection _currentLevel;
    public LevelSection CurrentLevel => _currentLevel;
    private int _currentLevelIndex = 0;
    public int CurrentLevelIndex => _currentLevelIndex;

    public int CurrentPrefabIndex => _levelPrefabs.IndexOf(_levelPrefabs.FirstOrDefault(x => x.LevelIndex.Equals(CurrentLevel.LevelIndex)));
    //TODO Refactor this, Currently Prototype
    [SerializeField]
    private Picker Picker;



    void Start()
    {

      
    }

    public bool IsDestinationReached()
    {
        return _currentLevel.IsEndSection(Picker.MaxExtends());
    }

    internal void Init(ServiceLocator serviceLocator)
    {
        var entity = serviceLocator.SaveManager.Load();
        //SaveEntity entity = null;
        if (entity== null || !entity.IsSavedData)
        {
            CreateLevel(0);
            _currentLevel.Init();
            Picker.Init(_currentLevel.StartPosition());
        }
        else
        {
            CreateLevel(entity.LevelEntity.PrefabIndex,entity.LevelEntity.Position,Quaternion.identity);
            _currentLevelIndex = entity.LevelEntity.LevelIndex;
            _currentLevel.Init(entity.LevelEntity);
           
            Picker.Init(entity.PlayerEntity.Position);
        }
    }

    public bool IsLevelCompleted()
    {
        return _currentLevel.IsCompleted;
    }
    public void Restart()
    {
        _currentLevel.OnComplete();
        CreateLevel(_currentLevelIndex, _currentLevel.transform.position,_currentLevel.transform.rotation);
        _currentLevel.Init();
        Picker.Init(_currentLevel.StartPosition());
    }

    public void LoadNextLevel()
    {
       
        if (_currentLevelIndex < _levelPrefabs.Count-1)
        {
            ++_currentLevelIndex;
            var nextLevel = Get(_currentLevelIndex);

            Attach(_currentLevel, nextLevel);
            Free(_currentLevel);
            _currentLevel = nextLevel;
            _currentLevel.Init();
            //var nextLevel = _levelPrefabs[_currentLevelIndex];
            //var spawnPosition =   _currentLevel.EndPosition();
            //spawnPosition.z += nextLevel.MinExtend();

            //_activelevels.Remove(_currentLevel);
            //_passivelevels.Add(_currentLevel);

            //_currentLevel =  Instantiate(nextLevel, spawnPosition, Quaternion.identity);
            //_activelevels.Add(_currentLevel);

        }
        else 
        {

            // UnityEngine.Random.state = new UnityEngine.Random.State();
            UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
            var randInt = UnityEngine.Random.Range(1, 100);
            var randomIndex = (int)randInt % (_levelPrefabs.Count());
            var nextLevel = Get(randomIndex);
            Attach(_currentLevel, nextLevel);
            Free(_currentLevel);
            _currentLevel = nextLevel;
            _currentLevel.Init();

            //CreateRandomLevel();       
        }
    }
    private void CreateLevel(int index)
    {
         CreateLevel(index, Vector3.zero, Quaternion.identity);
    }
    private void CreateLevel(int index,Vector3 position, Quaternion rotation)
    {     
        _currentLevelIndex = Mathf.Clamp(index,0,_levelPrefabs.Count-1);
        if (!_activelevels.Contains(_currentLevel))
        {
            var level = _levelPrefabs[_currentLevelIndex];
            _currentLevel = Instantiate(level, position, rotation);
            _activelevels.Add(_currentLevel);
           // _currentLevel.Init();
        }
        else
        {
           // _currentLevel.Init();
            _currentLevel.transform.position = position;
            _currentLevel.transform.rotation = rotation;
        }   
    }

    private void CreateRandomLevel()
    {
        var randomIndex = UnityEngine.Random.Range(0, _passivelevels.Count()-1);
        var nextLevel = _passivelevels[randomIndex];

        var spawnPosition = _currentLevel.EndPosition();
        spawnPosition.z += nextLevel.MinExtend();

        nextLevel.transform.position = spawnPosition;

        _activelevels.Remove(_currentLevel);
        _passivelevels.Add(_currentLevel);

        _passivelevels.Remove(nextLevel);
        _activelevels.Add(nextLevel);

        _currentLevel = nextLevel;
        _currentLevel.Init();

    }

    public void Free(LevelSection levelSection)
    {
        _activelevels.Remove(levelSection);
        _passivelevels.Add(levelSection);
        this.DelayedAction(() =>
        {
            levelSection.gameObject.SetActive(false);
        }, 2);
    }

    public LevelSection Get(int index)
    {
        var nextLevel = _passivelevels.ElementAtOrDefault(index) ??
                        Instantiate(_levelPrefabs[_currentLevelIndex], Vector3.zero, Quaternion.identity);
        if (_passivelevels.Contains(nextLevel))
        {
            _passivelevels.Remove(nextLevel);
        }
        _activelevels.Add(nextLevel);

        return nextLevel;
    }

    public void Attach(LevelSection from, LevelSection to)
    {
        var spawnPosition = from.EndPosition();
        spawnPosition.z += to.MinExtend();

        to.transform.position = spawnPosition;
    }
}
