using UnityEngine;

[CreateAssetMenu(fileName = "MagicCircle", menuName = "Scriptable Objects/MagicCircle")]
public class MagicCircle : ScriptableObject
{

    public Sprite sprite;
    public Vector3 size;
    public string type;
    public int pointsEarned;
    public MagicCircle nextCircle;

}
