using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Game/Color Palette")]
public class ColorPalette : ScriptableObject
{
    public Color playerColor = Color.blue;
    public Color enemyColor = Color.red;
}
