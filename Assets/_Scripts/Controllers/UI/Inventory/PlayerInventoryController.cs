using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInventoryController : InventoryAbstract {
    private static PlayerInventoryController _instance;

    public static PlayerInventoryController Instance
    {
        get {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        _slotsCount = GlobalVariables.PLAYER_INVENTORY_SIZE;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SortInventory();
        }

        List<string> items = new List<string>();
        for (int i = 0; i < _slotsCount; i++)
        {
            if (_slots[i].item != null)
            {
                string item = i + "," + _slots[i].item.name + "," + _slots[i].currentAmmount;
                items.Add(item);
            }
        }
        GameSaver.GameInfo.ChangeInventoryItems(name, items);
    }

    public void AddOrDrop(Item item, int ammount)
    {
        int remainingAmmount = AddItemAndReturnRemainingAmmount(item, ammount);
        if(remainingAmmount > 0)
        {
            GameObject.FindGameObjectWithTag("World Manager").GetComponent<WorldController>().SpawnDrop(item, remainingAmmount);
        }
    }
}
