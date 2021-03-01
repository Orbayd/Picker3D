using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager
{
    public void Init(GameManager gameManager);
    public void UpdateState(IGameState gameState);
}

public abstract class ManagerBase : MonoBehaviour, IManager
{
    public abstract void Init(GameManager gameManager);
    public virtual void UpdateState(IGameState gameState) { }
}
