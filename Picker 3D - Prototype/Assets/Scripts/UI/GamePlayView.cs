using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayView : ViewBase
{
    [SerializeField]
     Text LevelText;
    // Start is called before the first frame update

    public void SetLevel(int level)
    {
        LevelText.text = $"Level {level}";
    }

}
