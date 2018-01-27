using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SmelterController : FuelBurnerMachine
{    
    private float _currentSmeltTime = 0f;

    protected void Update()
    { 
        _slidersDictionary["OutputProgressSlider"].value = Mathf.Lerp(0f, 1f, _currentSmeltTime / GlobalVariables.SMELTER_SMELT_TIME);
        Smelt();

        base.Update();
    }

    private void Smelt()
    {
        if (_currentSmeltTime < GlobalVariables.SMELTER_SMELT_TIME)
        {
            if (_slotsDictionary["InputSlot"].item != null)
            {
                if (!_fuelBurning)
                {
                    if (_slotsDictionary["FuelSlot"].currentAmmount > 0)
                    {
                        StartCoroutine(BurnFuel());
                    }
                }

                if (_fuelBurning)
                {
                    _currentSmeltTime += Time.deltaTime;
                }
            }
            else
            {
                _currentSmeltTime = 0f;
            }
        }
        else if (_currentSmeltTime >= GlobalVariables.SMELTER_SMELT_TIME)
        {
            if (_slotsDictionary["OutputSlot"].currentAmmount < GlobalVariables.MAX_STACK_AMMOUNT)
            {
                _slotsDictionary["InputSlot"].currentAmmount--;
                if (_slotsDictionary["OutputSlot"].item == null)
                {
                    _slotsDictionary["OutputSlot"].InitItem(_slotsDictionary["InputSlot"].item.interactionResult, 1);
                }
                else if (_slotsDictionary["OutputSlot"].item == _slotsDictionary["InputSlot"].item.interactionResult)
                {
                    _slotsDictionary["OutputSlot"].currentAmmount++;
                }
                _currentSmeltTime = 0;
            }
        }
    }
}
