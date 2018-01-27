using UnityEngine;
using System.Collections;

public class MachineOutputController : MonoBehaviour
{
    private InventoryAbstract _collidingInventory;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding");
        if (other.GetComponent<InventoryAbstract>() != null && _collidingInventory == null)
        {
            Debug.Log("Colliding inventory set");
            _collidingInventory = other.GetComponent<InventoryAbstract>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _collidingInventory = null;
    }

    public InventoryAbstract GetInventoryColliding()
    {
        return _collidingInventory;
    }
}
