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

    protected GameManager _locator;
    public GameStateBase(GameManager locator)
    {
        _locator = locator;
    }
}

public class GamePlayState : GameStateBase
{
    public GamePlayState(GameManager locator) : base(locator)
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
        //var input = Input.GetAxisRaw("Horizontal");
        //Debug.Log($"Input {input}");
#if UNITY_ANDROID
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
           _locator.Picker.SetInput(Mathf.Clamp(Input.GetTouch(0).deltaPosition.x  * 0.1f,-2,2));
        }
#endif
#if UNITY_EDITOR
        _locator.Picker.SetInput(Input.GetAxis("Horizontal"));
#endif

    }

    public override void OnExit()
    {
        _locator.Picker.Stop();
    }
}
public class StartState : GameStateBase
{
    public StartState(GameManager locator) : base(locator)
    {
        
    }
    public override void OnEnter()
    {
        _locator.LevelManager.UpdateState(this);
        _locator.UIManager.UpdateState(this);
    }

    public override void OnExecute()
    {
#if UNITY_ANDROID
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            _locator.ChangeGameState(new GamePlayState(_locator));
        }
#endif
#if UNITY_EDITOR
        var input = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(input) > 0.1f)
        {
            _locator.ChangeGameState(new GamePlayState(_locator));
        }
#endif
    }
    public override void OnExit()
    {
     
    }
}

public class GameOverState : GameStateBase
{ 
    public GameOverState(GameManager locator) : base(locator)
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
        _locator.UIManager.UpdateScore();
    }

    public override void OnExit()
    {
    }
}

