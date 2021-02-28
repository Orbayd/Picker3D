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

    [SerializeField]
    private Transform ProbHolder;

    [Space(2)][Header("Info")]
    [SerializeField]
    public int LevelIndex;
    public int ExpectedProbCount = 2;

    [SerializeField]
    public int CurrentProbCount = 0;
    public bool IsCompleted  => CurrentProbCount >= ExpectedProbCount;

    public List<GameObject> ActiveProbs = new List<GameObject>();
    public void Start()
    {
        //Init();
    }
    public void Init()
    {
        this.gameObject.SetActive(true);
        var poolManager = FindObjectOfType<PoolManager>();
        var probPositions = ProbHolder.GetComponentsInChildren<Transform>().Where(x => !x.transform.Equals(ProbHolder.transform));
        ActiveProbs.Clear();
        foreach (var probPos in probPositions)
        {
            var prob = poolManager.Get(probPos.transform.position, Quaternion.identity);
            ActiveProbs.Add(prob);
            
            prob.GetComponent<Rigidbody>().velocity = Vector3.zero;
            prob.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        CurrentProbCount = 0;
    }
    public void Init(LevelEntity entity)
    {
        CurrentProbCount = entity.CurrentProbCount;
        ExpectedProbCount = entity.ExpectedProbCount;
        //LevelIndex = entity.LevelIndex;

        foreach (var probEntity in entity.ActiveProbs)
        {
            var prob = ServiceLocator.Instance.PoolManager.Get(probEntity.Position, Quaternion.identity);
            ActiveProbs.Add(prob);
            prob.GetComponent<Rigidbody>().velocity = Vector3.zero;
            prob.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public void OnComplete()
    {
        foreach (var prob in ActiveProbs)
        {   
            ServiceLocator.Instance.PoolManager.Free(prob.gameObject);
        }
        ActiveProbs.Clear();
        this.gameObject.SetActive(false);
    }
    public bool IsEndSection(Vector3 player)
    {
        return Mathf.Abs( StopSection.position.z - player.z) < 0.1f;
    }
    
    public Vector3 EndPosition()
    {
        var bounds = EndSection.GetComponent<MeshRenderer>().bounds.extents.z;
        //Debug.Log($"[Info] Extends z {bounds}");
        var endPos = EndSection.transform.position;
        endPos.z += bounds;
        return endPos;
    }
    public float MinExtend()
    {
        var bounds = StartSection.GetComponent<MeshRenderer>().bounds.extents.z;
        var startPos = StartSection.position;
        startPos.z -= bounds;
        return Vector3.Distance(startPos, transform.position);
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
        this.DelayedAction(() =>
        {
            ActiveProbs.Remove(prob.gameObject);
            ServiceLocator.Instance.PoolManager.Free(prob.gameObject);
            //Destroy(prob.gameObject);

        }, 0.3f);
        CurrentProbCount++;
        //Debug.Log($"[INFO] Current Prob {CurrentProbCount}");
    }
}
