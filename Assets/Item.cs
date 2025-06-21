using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemObject itemObject;
    public string itemId;
    public string itemName;
    public string type;
    public string[] flavors;
    private Sprite objectSprite;
    public GameObject sprite;
    public Vector2 nameTagSize = new Vector2(5.188f, 1.2381f);
    public Sprite pourMinigameSprite;
    public Color pourSpriteColor = new Color(1, 1, 1, 1);
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        setFields();
    }

    public void setItemObject(ItemObject itemObject)
    {
        this.itemObject = itemObject;
        setFields();
    }
    private void setFields()
    {
        this.itemId = itemObject.id;
        this.itemName = itemObject.name;
        this.type =itemObject.type;
        this.flavors =itemObject.flavors;
        this.objectSprite = itemObject.sprite;
        this.nameTagSize = itemObject.nameTagSize;
        this.pourMinigameSprite = itemObject.pourMinigameSprite;
        this.pourSpriteColor = itemObject.pourSpriteColor;

        SpriteRenderer sr = this.sprite.GetComponent<SpriteRenderer>();
        if (sr != null && this.objectSprite != null)
        {
            sr.sprite = this.objectSprite;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
