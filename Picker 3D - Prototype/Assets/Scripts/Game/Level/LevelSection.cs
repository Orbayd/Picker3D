using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSection : MonoBehaviour
{
    [Header("Bounds")]
    [SerializeField]
    private Transform EndSection;
    [SerializeField]
    private Transform StartSection;
    [SerializeField]
    private Transform StopSection;

    [Space(2)][Header("Info")]
    [SerializeField]
    public int LevelIndex;
    public int ExpectedProbCount = 2;

    [SerializeField]
    public int CurrentProbCount = 0;
    public bool IsCompleted  => CurrentProbCount >= ExpectedProbCount;

    public List<Probs> ActiveProbs = new List<Probs>();
    
    public void Init(LevelSectionDatabase levelSectionDb)
    {
        this.gameObject.SetActive(true);
        var poolManager = FindObjectOfType<ProbPooler>();
        var probPositions = levelSectionDb.ProbPositions;
        ExpectedProbCount = levelSectionDb.ExpectedProb;
        LevelIndex = levelSectionDb.Level;
        ChangeMaterial(levelSectionDb.BoardMaterial);
        ActiveProbs.Clear();

        for (int i = 0; i < probPositions.Length; i++)
        {
            var probGo = poolManager.Get(levelSectionDb.ProbType[i], this.transform.position + probPositions[i], Quaternion.identity);
            var prob = probGo.GetComponent<Probs>();
            prob.Init();
            ActiveProbs.Add(prob);
        }
        CurrentProbCount = 0;
    }

    public void Init(LevelEntity entity)
    {
        CurrentProbCount = entity.CurrentProbCount;
        ExpectedProbCount = entity.ExpectedProbCount;
        LevelIndex = entity.LevelIndex;
        
        foreach (var probEntity in entity.ActiveProbs)
        {
            var probGo = GameManager.Instance.Pooler.Get(probEntity.ProbType,probEntity.Position, Quaternion.identity);
            var prob = probGo.GetComponent<Probs>();
            ActiveProbs.Add(prob);
            prob.Init(probEntity.Velocity, probEntity.AngularVelocity);
        }
    }

    public void OnComplete()
    {
        foreach (var prob in ActiveProbs)
        {   
            GameManager.Instance.Pooler.Free(prob.gameObject);
        }
        ActiveProbs.Clear();
        
    }
    public void ChangeMaterial(Material mat)
    {
        if (mat == null)
        {
            return;
        }
        var meshRenderers = GetComponentsInChildren<MeshRenderer>().Where(x => !x.GetComponent<EndSection>());
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = mat;
        }
    }

    public void UpdateState(IGameState gameState)
    {
        if (gameState is GamePlayState)
        {
            foreach (var prob in ActiveProbs)
            {
                prob.Begin();
            }
        }
    }

    public bool IsEndSection(Vector3 player)
    {
        return Mathf.Abs( StopSection.position.z - player.z) < 0.1f;
    }
    
    public Vector3 EndPosition()
    {
        var bounds = EndSection.GetComponent<MeshRenderer>().bounds.extents.z;
        var endPos = EndSection.transform.position;
        endPos.z += bounds;
        return endPos;
    }
    public float MinForwardExtend()
    {
        var bounds = StartSection.GetComponent<MeshRenderer>().bounds.extents.z;
        var startPos = StartSection.position;
        startPos.z -= bounds;
        return Vector3.Distance(startPos, transform.position);
    }
    /// <summary>
    /// Extends of Level Sector
    /// </summary>
    /// <returns>Vector2 , x = min , y = max</returns>
    public Vector2 RightExtends()
    {
        var bounds = StartSection.GetComponent<MeshRenderer>().bounds;

        return new Vector2(bounds.min.x, bounds.max.x);
    }
    public Vector3 StartPosition()
    {
        var bounds = StartSection.GetComponent<MeshRenderer>().bounds.max;
        var startPosition = StartSection.transform.position;
        startPosition.y = bounds.y;
        return startPosition;
    }
    public void CollectProbe(Probs prob)
    {
        FreeProbe(prob);
        CurrentProbCount++;
    }
    public void FreeProbe(Probs prob)
    {
        ActiveProbs.Remove(prob);
        GameManager.Instance.Pooler.Free(prob.gameObject);
    }

    public GameObject CreateProb(ProbType type ,Vector3 position,Quaternion rotation)
    {
        var prob = GameManager.Instance.Pooler.Get(type, position, rotation);
        ActiveProbs.Add(prob.GetComponent<Probs>());
        return prob;
       
    }
}
