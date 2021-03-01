using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposiveProb : Probs
{
    public int ExplosiveCount = 5;
    public ProbType ExplosiveProbType;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Picker>())
        {
            for (int i = 0; i < ExplosiveCount; i++)
            {
                ServiceLocator.Instance.LevelManager.CurrentSection.CreateProb(ExplosiveProbType, this.transform.position,Quaternion.identity);
            }

            ServiceLocator.Instance.LevelManager.CurrentSection.FreeProbe(this);
        }
    }
}
