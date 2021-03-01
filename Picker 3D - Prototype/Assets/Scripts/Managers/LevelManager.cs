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
    private Stack<LevelSection> _passivelevels = new Stack<LevelSection>();

    private LevelSection _currentLevel;
    public LevelSection CurrentLevel => _currentLevel;
    private int _currentLevelIndex = 0;
    public int CurrentLevelIndex => _currentLevelIndex;

    public int CurrentPrefabIndex => _levelPrefabs.IndexOf(_levelPrefabs.FirstOrDefault(x => x.LevelIndex.Equals(CurrentLevel.LevelIndex)));
    //TODO Refactor this, Currently Prototype
    [SerializeField]
    private Picker Picker;

    public LevelDataBase LevelDb;


    void Start()
    {

      
    }

    public bool IsDestinationReached()
    {
        return _currentLevel.IsEndSection(Picker.MaxExtends());
    }

    internal void Init(ServiceLocator serviceLocator)
    {
        //var entity = serviceLocator.SaveManager.Load();
        SaveEntity entity = null;
        if (entity== null || !entity.IsSavedData)
        {
            _currentLevel = Get(0);
            _currentLevel.Init(LevelDb.GetLevel(0));
            Picker.Init(_currentLevel.StartPosition());
        }
        else
        {
           
            _currentLevel = Get(0, entity.LevelEntity.Position, Quaternion.identity);
            _currentLevelIndex = entity.LevelEntity.LevelIndex;
            _currentLevel.Init(entity.LevelEntity);
            _currentLevel.ChangeMaterial(LevelDb.GetLevel(entity.LevelEntity.LevelIndex).BoardMaterial);
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
        _currentLevel = Get(_currentLevelIndex);
        _currentLevel.Init(LevelDb.GetLevel(_currentLevelIndex));

        Picker.Init(_currentLevel.StartPosition());
    }

    public void LoadNextLevel()
    {

        ++_currentLevelIndex;
        var nextLevel = Get(_currentLevelIndex);

        Attach(_currentLevel, nextLevel);
        Free(_currentLevel);
        _currentLevel = nextLevel;
        _currentLevel.Init(LevelDb.GetLevel(_currentLevelIndex));
        if (_currentLevelIndex < _levelPrefabs.Count-1)
        {

        }
        else 
        {

            // UnityEngine.Random.state = new UnityEngine.Random.State();
            //UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
            //var randInt = UnityEngine.Random.Range(1, 100);
            //var randomIndex = (int)randInt % (_levelPrefabs.Count());
            //var nextLevel = Get(randomIndex);
            //Attach(_currentLevel, nextLevel);
            //Free(_currentLevel);
            //_currentLevel = nextLevel;
            //_currentLevel.Init();

            //CreateRandomLevel();       
        }
    }
    //private void CreateLevel(int index)
    //{
    //     CreateLevel(index, Vector3.zero, Quaternion.identity);
    //}
    //private void CreateLevel(int index,Vector3 position, Quaternion rotation)
    //{     
    //    _currentLevelIndex = Mathf.Clamp(index,0,_levelPrefabs.Count-1);
    //    if (!_activelevels.Contains(_currentLevel))
    //    {
    //        var level = _levelPrefabs[_currentLevelIndex];
    //        _currentLevel = Instantiate(level, position, rotation);
    //        _activelevels.Add(_currentLevel);
    //       // _currentLevel.Init();
    //    }
    //    else
    //    {
    //       // _currentLevel.Init();
    //        _currentLevel.transform.position = position;
    //        _currentLevel.transform.rotation = rotation;
    //    }   
    //}


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
        //var nextLevel = _passivelevels.Count > 0 ? _passivelevels.Pop() :
        //                Instantiate(LevelDb.LevelPrefab.GetComponent<LevelSection>(), Vector3.zero, Quaternion.identity);
        //_activelevels.Add(nextLevel);

        //return nextLevel;

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
