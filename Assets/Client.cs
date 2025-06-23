using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Client", menuName = "Scriptable Objects/Client")]
public class Client : ScriptableObject
{
    public int id;
    public new string name;
    public string[] liked_flavors;
    public string[] disliked_flavors;
    public float tip_score = 1.0f;

    public Sprite sprite;
}
