using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState 
{
    public void OnEnter();
    public void OnExecute();
    public void OnExit();
}

public abstract class GameStateBase : IGameState
{
    public abstract void OnEnter();
    public abstract void OnExecute();
    public abstract void OnExit();

    protected ServiceLocator _locator;
    public GameStateBase(ServiceLocator locator)
    {
        _locator = locator;
    }
}

public class GamePlayState : GameStateBase
{
    public GamePlayState(ServiceLocator locator) : base(locator)
    {
        _locator.LevelManager.UpdateState(this);
        _locator.UIManager.UpdateState(this);
    }
    public override void OnEnter()
    {
        _locator.Picker.Move();
    }

    public override void OnExecute()
    {
        if (_locator.LevelManager.IsDestinationReached())
        {
            _locator.Picker.Stop();
            _locator.ChangeGameState(new GameOverState(_locator));
        }

        _locator.Picker.SetInput(Input.GetAxis("Horizontal"));
    }

    public override void OnExit()
    {
        _locator.Picker.Stop();
    }
}
public class StartState : GameStateBase
{
    public StartState(ServiceLocator locator) : base(locator)
    {
        
    }
    public override void OnEnter()
    {
        _locator.LevelManager.UpdateState(this);
        _locator.UIManager.UpdateState(this);
    }

    public override void OnExecute()
    {
        var input = Input.GetAxis("Horizontal");
        if (Mathf.Abs(input)> 0.1f)
        {
            _locator.ChangeGameState(new GamePlayState(_locator));
        }
    }

    public override void OnExit()
    {
     
    }
}

public class GameOverState : GameStateBase
{ 
    public GameOverState(ServiceLocator locator) : base(locator)
    {

    }
    public override void OnEnter()
    {
        _locator.LevelManager.UpdateState(this);
        _locator.DelayedAction(() =>
        {
            _locator.UIManager.UpdateState(this);
            
            
        }, 4);
    }
    public override void OnExecute()
    {
        
    }

    public override void OnExit()
    {
    }
}

