using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelEntity 
{
    public int CurrentLevel { get; set; }
    public int LevelIndex { get; set; }
    public int ExpectedProbCount { get; set; }
    public int CurrentProbCount { get; set; }
    public Vector3 Position { get; set; }
    public List<ProbEntity> ActiveProbs { get; set; } = new List<ProbEntity>();
}
[System.Serializable]
public class ProbEntity
{
    public Vector3 Position { get; set; }
    public ProbType ProbType { get; set; }
    public Vector3 Velocity { get; set; }
    public Vector3 AngularVelocity { get; set; }

}
