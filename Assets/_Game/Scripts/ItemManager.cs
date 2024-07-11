using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> itemObjects;

    public static ItemManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this); }
    }
    private void Start()
    {
        // Deactivate all items on start
        foreach (var itemObject in itemObjects)
        {
            IItem item = itemObject.GetComponent<IItem>();
            if (item != null)
            {
                item.Deactivate();
                itemObject.SetActive(false);
            }
        }
        ActivateItemByName("NukeWaste");
        ActivateItemByName("DualPistol");
    }

    public void ActivateItemByName(string itemName)
    {
        foreach (var itemObject in itemObjects)
        {
            IItem item = itemObject.GetComponent<IItem>();
            if (item != null && item.ItemName == itemName)
            {
                itemObject.SetActive(true);
                item.Activate();
                break;
            }
        }
    }
}