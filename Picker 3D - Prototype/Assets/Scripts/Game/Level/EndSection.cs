using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    LevelSection _levelSection;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Probs>())
        {
            _levelSection.CollectProbe(collision.gameObject.GetComponent<Probs>());

            this.DelayedAction(() =>
            {
                GameManager.Instance.Pooler.Free(collision.gameObject);
               // Destroy(collision.gameObject,0);
            }, 2);
            //Destroy(collision.gameObject,2);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Probs>())
        {
            _levelSection.CollectProbe(collision.gameObject.GetComponent<Probs>());

           
        }
    }
}
