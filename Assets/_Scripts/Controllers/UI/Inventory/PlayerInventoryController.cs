using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInventoryController : InventoryAbstract {
    private void Awake()
    {
        _slotsCount = GlobalVariables.PLAYER_INVENTORY_SIZE;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SortInventory();
        }
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
