using UnityEngine;

[System.Serializable]
public class OrderIngredient
{
    public string id;
    public string name;
    public string[] flavors;

    public OrderIngredient(ItemObject source)
    {
        id = source.id;
        name = source.name;
        flavors = (string[])source.flavors.Clone();
    }
}
