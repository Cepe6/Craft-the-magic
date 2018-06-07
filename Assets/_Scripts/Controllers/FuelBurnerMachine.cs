using UnityEngine;
using System.Collections;

public class FuelBurnerMachine : MachineController
{
    private float _currentFuelLeft;
    protected bool _fuelBurning;

    // Update is called once per frame
    protected void Update()
    {
        _slidersDictionary["FuelAmmountLeft"].value = Mathf.Lerp(0f, 1f, _currentFuelLeft / GlobalVariables.COAL_FUEL_AMMOUNT);

        base.Update();
    }

    protected IEnumerator BurnFuel()
    {
        _fuelBurning = true;
        _slotsDictionary["FuelSlot"].currentAmmount--;

        _currentFuelLeft = GlobalVariables.COAL_FUEL_AMMOUNT;
        while (_currentFuelLeft > 0)
        {
            _currentFuelLeft -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _fuelBurning = false;
        yield return null;
    }
}
