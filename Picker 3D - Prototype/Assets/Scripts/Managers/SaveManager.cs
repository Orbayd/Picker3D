using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    string _savePath;

    Picker picker;

    LevelManager LevelManager;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void Init(ServiceLocator locator)
    {
        LevelManager = locator.LevelManager;
        picker = locator.Picker;
    }

    public SaveEntity Load()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        if (File.Exists(_savePath))
        {
            var json = File.ReadAllText(_savePath);

            return JsonConvert.DeserializeObject<SaveEntity>(json);

        }
        return new SaveEntity()
        {
            IsSavedData = false
        };
    }

    public void Save()
    {
        var pEntity = new PlayerEntity() { Position = picker.transform.position, Speed = 4 };
        var lEntitiy = new LevelEntity()
        {
            CurrentProbCount = LevelManager.CurrentLevel.CurrentProbCount,
            ExpectedProbCount = LevelManager.CurrentLevel.ExpectedProbCount,
            ActiveProbs = LevelManager.CurrentLevel.ActiveProbs.Select(x => new ProbEntity(){ Position = x.transform.position ,
                                                                                              ProbType = x.GetComponent<Probs>().ProbType }).ToList(),
            LevelIndex = LevelManager.CurrentLevelIndex,
            PrefabIndex = LevelManager.CurrentPrefabIndex,
            Position = LevelManager.CurrentLevel.transform.position
        };

        var sEntity = new SaveEntity()
        {
            CurrentLevel = 0,
            LevelEntity = lEntitiy,
            PlayerEntity = pEntity,
        };

        var json = JsonConvert.SerializeObject(sEntity);

        _savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        File.WriteAllText(_savePath, json);

       
    }
}
