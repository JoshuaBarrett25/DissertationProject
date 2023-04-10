using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Preset", menuName = "Scripts/Gameplay/Preset", order =1)]
public class Preset : ScriptableObject
{
    public Gradient ambient;
    public Gradient directional;
    public Gradient fog;
}
