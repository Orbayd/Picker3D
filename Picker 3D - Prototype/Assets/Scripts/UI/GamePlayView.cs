using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayView : ViewBase
{
    [SerializeField]
     Text LevelText;

    [SerializeField]
    Text ScoreText;
    // Start is called before the first frame update

    public void SetLevel(int level)
    {
        LevelText.text = $"Level {level + 1}";
    }

    public void SetScore(int current , int expected)
    {
        ScoreText.text = $"{current} / {expected}";
    }

}
