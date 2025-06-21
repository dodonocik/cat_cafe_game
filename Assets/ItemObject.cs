using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName="Item" )]
public class ItemObject : ScriptableObject
{
    public string id;
    public new string name;
    public string type;
    public string[] flavors;
    public Sprite sprite;
    public Vector2 nameTagSize = new Vector2(5.188f, 1.2381f);
    public Sprite pourMinigameSprite;
    public Color pourSpriteColor = new Color(1, 1, 1, 1);
}
