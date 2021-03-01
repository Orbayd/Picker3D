
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	//[SerializeField]
	//private GameObject m_Original;

	[SerializeField]
	private GameObject[] _originals;

	//protected Stack<GameObject> m_FreeInstances = new Stack<GameObject>();

	protected Dictionary<ProbType, Stack<GameObject>> _freeInstances = new Dictionary<ProbType, Stack<GameObject>>();


    public void Init(ProbType type, int initialSize)
    {
        var probPrefab = _originals.FirstOrDefault(x => x.GetComponent<Probs>().ProbType == type);
        if (probPrefab != null)
        {
            var stack = new Stack<GameObject>(initialSize);

            for (int i = 0; i < initialSize; ++i)
            {
                GameObject obj = Object.Instantiate(probPrefab);
                obj.SetActive(false);
                stack.Push(obj);
            
            }
            if (_freeInstances.ContainsKey(type))
            {
                _freeInstances[type] = stack;
            }
            else
            {
                _freeInstances.Add(type, stack);
            }
        }
        
    }

    public GameObject Get(ProbType type)
    {
        return Get(type, Vector3.zero, Quaternion.identity);
    }
    //TODO Refactor this Later, Code Duplication
    public GameObject Get(ProbType type,Vector3 pos, Quaternion quat)
    {
        var probPrefab = _originals.FirstOrDefault(x => x.GetComponent<Probs>().ProbType == type);
        if (_freeInstances.ContainsKey(type))
        {
            var stack = _freeInstances[type];
            GameObject ret = stack.Count > 0 ? stack.Pop() : Object.Instantiate(probPrefab);

            ret.SetActive(true);
            ret.transform.position = pos;
            ret.transform.rotation = quat;
            return ret;

        }
        else
        {
            GameObject ret = Object.Instantiate(probPrefab);
            ret.SetActive(true);
            ret.transform.position = pos;
            ret.transform.rotation = quat;
            return ret;
        }

       
    }

    public void Free(GameObject obj)
    {
        var prob = obj.GetComponent<Probs>();
        obj.transform.SetParent(null);
        obj.SetActive(false);
        if (_freeInstances.ContainsKey(prob.ProbType))
        {
            var stack = _freeInstances[prob.ProbType];
            stack.Push(obj);
        }
        else
        {
            var stack = new Stack<GameObject>();
            stack.Push(obj);
            _freeInstances.Add(prob.ProbType, stack);

        }
    }
}
