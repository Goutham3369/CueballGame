using UnityEngine;

[System.Serializable]
public class BallSlot
{
    public bool isLocked;
    public BallHolder[] ballHolders = new BallHolder[4];
}

[CreateAssetMenu(fileName = "Level Data ", menuName = "ScriptableObjects/LevelData")]

public class LevelData : ScriptableObject
{
    [Header("Ball Slots")]
    public BallSlot[] ballSlots = new BallSlot[12];
}


[System.Serializable]
public class BallHolder
{
    public enum BallColor
    {
        Cyan = 0,
        SkyBlue = 1,
        Green = 2,
        Black = 3,
        LightPink = 4,
        Red = 5,
        Orange = 6,
        Yellow = 7,
        Purple = 8,
        Gray = 9
    }
    public BallColor selectedColorEnum;
    public Color selectedColor;
}
