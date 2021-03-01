using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelDatabase", order = 1)]
public class LevelDataBase : ScriptableObject
{
    public GameObject LevelPrefab;

    public List<LevelSectionDatabase> Levels = new List<LevelSectionDatabase>();

    public LevelSectionDatabase GetLevel(int level)
    {
        var levelSection = Levels.FirstOrDefault(x => x.Level == level + 1);
        if (levelSection == null)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            var randInt = Random.Range(1, 100);
            var randomIndex = (int)randInt % (Levels.Count());

            levelSection = Levels[randomIndex];
        }

        return levelSection;
    }

}
