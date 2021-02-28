using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SaveEntity
{
    public PlayerEntity PlayerEntity { get; set; }
    public LevelEntity LevelEntity { get; set; }

    public int CurrentLevel { get; set; }

    [JsonIgnore]
    public bool IsSavedData { get; set; } = true;
    
}
