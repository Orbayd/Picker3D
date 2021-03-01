using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSectionCreator))]
public class LevelDataBaseCreator : Editor
{
    LevelSectionCreator _selectedObject;


    void OnEnable()
    {
        _selectedObject = Selection.activeGameObject.GetComponent<LevelSectionCreator>();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Create Database"))
        {
            CreateLevelDatabase();
        }
       
    }

    private void CreateLevelDatabase()
    {
        Debug.Log("Level Db Created");

        var levelSection = _selectedObject.GetComponent<LevelSection>();
        var probs = _selectedObject.GetComponentsInChildren<Probs>();
        LevelSectionDatabase lvlDb = ScriptableObject.CreateInstance<LevelSectionDatabase>();

        lvlDb.ProbPositions = probs.Select(x=> x.transform.position).ToArray();
        lvlDb.ProbType = probs.Select(x => x.ProbType).ToArray();
        lvlDb.Level = levelSection.LevelIndex;
        lvlDb.ExpectedProb = levelSection.ExpectedProbCount;

       
        AssetDatabase.CreateAsset(lvlDb, $"Assets/Resources/ScriptableObjects/LevelSectionDatabase_{lvlDb.Level}.asset");
        AssetDatabase.SaveAssets();
    }

}
