using UnityEngine;
using System.Collections;

public class MachineOutputController : MonoBehaviour
{
    private InventoryAbstract _collidingInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InventoryAbstract>() != null && other != this.gameObject && _collidingInventory == null)
        {
            _collidingInventory = other.GetComponent<InventoryAbstract>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_collidingInventory != null && _collidingInventory.gameObject == other.gameObject)
            _collidingInventory = null;
    }

    public InventoryAbstract GetInventoryColliding()
    {
        return _collidingInventory;
    }
}
