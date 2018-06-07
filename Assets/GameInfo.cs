using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class GameInfo {
    public int seed;
    public String playerPosition;

    public List<string> savedChunksCoords = new List<string>();
    public List<StringListWrapper> changedResourcesCoords = new List<StringListWrapper>();
    public List<IntListWrapper> changedResourcesAmmount = new List<IntListWrapper>();

    public List<string> placedMachines = new List<string> ();
    public List<StringListWrapper> placedMachinesInventory = new List<StringListWrapper>();

    public List<string> savedInventories = new List<string>();
    public List<StringListWrapper> savedInventoriesItems = new List<StringListWrapper>();

    public void ChangeResource(Vector2 chunkCoords, Vector2 localResCoord, int ammount)
    {
        string chunkCoordsToString = chunkCoords.x + "," + chunkCoords.y;
        string localResCoordToString = localResCoord.x + "," + localResCoord.y;
        int chunkIndex = savedChunksCoords.IndexOf(chunkCoordsToString);

        if(changedResourcesCoords.ElementAtOrDefault(chunkIndex) == null)
        {
            changedResourcesCoords.Insert(chunkIndex, new StringListWrapper());
            changedResourcesAmmount.Insert(chunkIndex, new IntListWrapper());
        }

        if (changedResourcesCoords[chunkIndex].list.Where(coords => coords == localResCoordToString).SingleOrDefault() == null)
        {
            changedResourcesCoords.ElementAt(chunkIndex).list.Add(localResCoordToString);
            changedResourcesAmmount.ElementAt(chunkIndex).list.Add(ammount);
        } else
        {
            int resIndex = changedResourcesCoords[chunkIndex].list.IndexOf(localResCoordToString);
            changedResourcesAmmount.ElementAt(chunkIndex).list[resIndex] = ammount;
        }
    }
    public void ChangePlacedMachineInventory(string name, List<string> items)
    {
        int machineIndex = placedMachines.IndexOf(name);
        if(placedMachinesInventory.ElementAtOrDefault(machineIndex) == null)
        {
            placedMachinesInventory.Insert(machineIndex, new StringListWrapper(items));
        } else
        {
            placedMachinesInventory.ElementAt(machineIndex).list = new List<string>(items); ;
        }

    }
    public void ChangeInventoryItems(string name, List<string> items)
    {
        int inventoryIndex = savedInventories.IndexOf(name);
        if(inventoryIndex != -1) {
            if (savedInventoriesItems.ElementAtOrDefault(inventoryIndex) == null)
            {
                savedInventoriesItems.Insert(inventoryIndex, new StringListWrapper(items));
            }
            else
            {
                savedInventoriesItems.ElementAt(inventoryIndex).list = new List<string>(items); ;
            }
        }
        
    }
}

[Serializable]
public class StringListWrapper
{
    public List<string> list = new List<string> ();

    public StringListWrapper ()
    {

    }

    public StringListWrapper (List<string> items)
    {
        list = new List<string>(items);
    }
}

[Serializable]
public class IntListWrapper
{
    public List<int> list = new List<int>();
}
