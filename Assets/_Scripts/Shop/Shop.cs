using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject ItemPrefab;
    public GameObject StatParent;
    public GameObject ImageItem;

    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject itemToBuy = Instantiate(ItemPrefab, this.transform);
            itemToBuy.GetComponent<ItemToBuyUI>().m_gStatUIParent = StatParent;
            itemToBuy.GetComponent<ItemToBuyUI>().m_gImageItem = ImageItem;
            itemToBuy.GetComponent<ItemToBuyUI>().Init();

        }
    }
}
