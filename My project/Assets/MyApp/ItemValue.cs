using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemType
{
    SZeni,GZeni
}
public class ItemValue : MonoBehaviour
{
    [SerializeField] private int itemData = 1;
    public ItemType itemType;

    public ItemGroup itemGroup;
    private void Update()
    {
        if (itemType == ItemType.GZeni)
        {
            transform.Rotate(Vector3.forward, 90f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Finish"))
            {
                RoadController.Instance.isDisable = true;
                return;
            }

            if (itemGroup != null)
            {
                itemGroup.OnCheck(this);
            }
            
            gameObject.SetActive(false);
            GamePlayControlller.Instance.OnZeniUpdate(itemType,itemData);
        }
    }
}
