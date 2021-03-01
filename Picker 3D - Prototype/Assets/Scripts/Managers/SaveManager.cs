using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : ManagerBase
{
    string _savePath;

    Picker picker;

    LevelManager LevelManager;

    public override void Init(GameManager locator)
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
            CurrentProbCount = LevelManager.CurrentSection.CurrentProbCount,
            ExpectedProbCount = LevelManager.CurrentSection.ExpectedProbCount,
            ActiveProbs = LevelManager.CurrentSection.ActiveProbs.Select(x => new ProbEntity(){
                                                                        Position = x.transform.position ,
                                                                        Velocity = x.GetComponent<Rigidbody>().velocity,
                                                                        AngularVelocity = x.GetComponent<Rigidbody>().angularVelocity,
                                                                        ProbType = x.GetComponent<Probs>().ProbType ,
                                                                        }).ToList(),
            CurrentLevel = LevelManager.CurrentLevel,
            LevelIndex = LevelManager.CurrentSection.LevelIndex,
            Position = LevelManager.CurrentSection.transform.position
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
