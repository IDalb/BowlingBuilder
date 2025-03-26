using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemDrawer : MonoBehaviour
{
    [Serializable]
    public struct Item
    {
        public GameObject prefab;
        public Vector3 offset;
        public int amount;
    }

    public GameObject buttonPrefab;
    public Transform buttonsHolder;

    [Space]
    public List<Item> items;

    private void Awake()
    {
        foreach (var item in items)
        {
            var button = Instantiate(buttonPrefab, buttonsHolder);
            button.GetComponent<ItemDrawerItem>().item = item;
            button.GetComponent<ItemDrawerItem>().InstantiateObject();
        }
    }
}
