using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelSectionDatabase", order = 1)]
public class LevelSectionDatabase : ScriptableObject
{
    public int Level;
    public int ExpectedProb;

    public Vector3[] ProbPositions;
    public ProbType[] ProbType;

    public Material BoardMaterial;

}
