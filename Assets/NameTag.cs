using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameTag : MonoBehaviour
{
    public RectTransform nameTag;
    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowMouse();
    }
    private void FollowMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; 
        transform.position = cam.ScreenToWorldPoint(mousePos);
    }

    public void UpdateNametag(Item item)
    {
        nameTag.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
        nameTag.sizeDelta = item.nameTagSize;
        nameTag.localPosition = new Vector3(item.nameTagSize.x/2, -1.5f, 0);
    }
}
