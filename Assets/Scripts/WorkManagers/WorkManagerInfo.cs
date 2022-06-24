using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerType
{
    Junior,
    Senior,
    Executive
}

public enum BoostType
{
    Movement,
    Loading
}

[CreateAssetMenu]
public class WorkManagerInfo : ScriptableObject
{
    [Header("Manager Info")]
    public ManagerType ManagerType;
    public Color LevelColor;

    public BoostType BoostType;
    public Sprite BoostIcon;
    public float BoostDuration;
    public string BoostDescription;
    public float BoostValue;
}
