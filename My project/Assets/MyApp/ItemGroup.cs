using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGroup : MonoBehaviour
{
    [SerializeField] private ItemValue item1;
    [SerializeField] private ItemValue item2;

    public void OnCheck(ItemValue itemValue)
    {
        if (item1 == itemValue)
        {
            item2.gameObject.SetActive(false);
        }

        if (item2 == itemValue)
        {
            item1.gameObject.SetActive(false);
        }
    }
}
