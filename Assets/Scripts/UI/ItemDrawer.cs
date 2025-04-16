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
        
        public int initialAmount; 
        private int amount;
        public int GetAmount() { return amount; }
        public void SetAmount(int newAmount) { amount = newAmount; }
    }

    public GameObject buttonPrefab;
    public Transform buttonsHolder;

    [Space]
    public List<Item> items;
    private List<ItemDrawerItem> drawerItems;

    private void Awake()
    {
        drawerItems = new List<ItemDrawerItem>();

        foreach (var item in items)
        {
            var button = Instantiate(buttonPrefab, buttonsHolder);
            ItemDrawerItem drawerItem = button.GetComponent<ItemDrawerItem>();
            
            drawerItems.Add(drawerItem);
            drawerItem.item = item;
            drawerItem.ResetAmount();
        }
    }

    public void ResetShapes() {    
        foreach (var go in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            if (go.layer == LayerMask.NameToLayer("ConstructionBlock"))
                Destroy(go.gameObject);
        
        foreach (var item in drawerItems)
            item.ResetAmount();
    }
}
