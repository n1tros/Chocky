﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Item> _items;
    [SerializeField]
    private Transform _itemsParent;
    [SerializeField]
    private ItemSlot[] _itemSlots;

    private void OnValidate()
    {
        if (_itemsParent != null)
        {
            _itemSlots = _itemsParent.GetComponentsInChildren<ItemSlot>();
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < _items.Count && i < _itemSlots.Length; i++)
        {
            _itemSlots[i].Item = _items[i];
        }
    }

    public bool AddItem(Item item)
    {
        if (IsFull())
            return false;

        _items.Add(item);
        RefreshUI();
        return true;
    }

    public bool RemoveItem(Item item)
    {
        if (_items.Remove(item))
        {
            RefreshUI();
            return true;
        }
        else
            return false;
    }

    private bool IsFull()
    {
        return _items.Count >= _itemSlots.Length;
    }
}
