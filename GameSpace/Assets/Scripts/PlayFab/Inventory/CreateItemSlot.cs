using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class CreateItemSlot : MonoBehaviour
{
    public ItemContainer itemContainer;
    public RectTransform containTarget;
    MyGameSettingInstaller.Items[] refItems;
    [Inject]
    public void SettingPlayer(MyGameSettingInstaller.Items[] items_)
    {
        refItems = items_; // can use 
    }
    private void Start()
    {
        for (int i = 0; i < PlayFabController.singleton.items.Count; i++)
        {
            ItemContainer it = Instantiate(itemContainer);
            it.GetComponent<RectTransform>().SetParent(containTarget);
            it.GetComponent<RectTransform>().localPosition = Vector3.zero;
            it.itemName.text = PlayFabController.singleton.items[i].itemName;
            it.itemUses.text = PlayFabController.singleton.items[i].uses;
            for (int j = 0; j < refItems.Length; j++)
            {
               if(refItems[j].itemName == PlayFabController.singleton.items[i].itemName)
                {
                    it.itemIcon.sprite = refItems[j].itemSprite;
                    it.description.text = refItems[j].itemDescription;
                }
            }
        }
    }
}
