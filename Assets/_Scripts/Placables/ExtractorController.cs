using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExtractorController : FuelBurnerMachine {
    
    private TileData[,] _tilesUnder;
    private Vector2 _size;

    
    private float _currentMineTime = 0f;
    private TileData _currentMined;

    private MachineOutputController _output;

    protected void OnEnable()
    {
        _output = transform.Find("Output").GetComponent<MachineOutputController> ();

        _tilesUnder = GetComponent<Placable>().GetTilesUnder(); 

        base.OnEnable();
    }
     
    protected void Update()
    {
        _slidersDictionary["OutputProgressSlider"].value = Mathf.Lerp(0f, 1f, _currentMineTime / GlobalVariables.EXTRACTOR_MINE_TIME);
        Mine();

        base.Update();
    }

    private void Mine()
    {
        if (_currentMineTime < GlobalVariables.EXTRACTOR_MINE_TIME)
        {
            if (_currentMined == null || _currentMined.GetObjectAbove() == null)
                _currentMined = RandomTile();

            if (_currentMined != null && _currentMined.GetObjectAbove() != null)
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
                    _currentMineTime += Time.deltaTime;
                }
            }
        }
        else if (_currentMineTime >= GlobalVariables.EXTRACTOR_MINE_TIME && _currentMined != null && _currentMined.GetObjectAbove() != null)
        {
            bool output = false;

            if (_output.GetInventoryColliding() != null)
            {
                if (_output.GetInventoryColliding().AddItemAndReturnRemainingAmmount(_currentMined.GetObjectAbove().GetComponent<ResourceController>().item, 1) == 0)
                {
                    _currentMined.GetObjectAbove().GetComponent<ResourceController>().ammount--;
                    _currentMineTime = 0;
                    output = true;
                }
            }

            if (!output && _slotsDictionary["OutputSlot"].currentAmmount < GlobalVariables.MAX_STACK_AMMOUNT)
            {
                Debug.Log("HERE");
                _currentMined.GetObjectAbove().GetComponent<ResourceController>().ammount--;
                if (_slotsDictionary["OutputSlot"].item == null)
                {
                    _slotsDictionary["OutputSlot"].InitItem(_currentMined.GetObjectAbove().GetComponent<ResourceController>().item, 1);
                }
                else
                {
                    _slotsDictionary["OutputSlot"].currentAmmount++;
                }
                _currentMineTime = 0;
            }
        }
    }
    
    private TileData RandomTile()
    {
        int x = (int)Random.Range(0, _size.x);
        int y = (int)Random.Range(0, _size.y);
        while(_tilesUnder[x, y].GetTileType() != TilesEnum.IRON_ORE && _tilesUnder[x, y].GetTileType() != TilesEnum.COAL && _tilesUnder[x, y].GetObjectAbove() == null)
        {
            x = (int)Random.Range(0, _size.x);
            y = (int)Random.Range(0, _size.y);
        }

        return _tilesUnder[x, y];
    } 
}
