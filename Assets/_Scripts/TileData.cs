using UnityEngine;
using System.Collections;

public class TileData
{
    private TilesEnum _type;
    private GameObject _objectAbove;

    public TileData(TilesEnum type, GameObject objectAbove)
    {
        _type = type;
        _objectAbove = objectAbove;
    }

    public TileData(TilesEnum type)
    {
        _type = type;
        _objectAbove = null;
    }

    public TilesEnum GetTileType()
    {
        return _type;
    }

    public GameObject GetObjectAbove()
    {
        return _objectAbove;
    }

    public void SetObjectAbove(GameObject objectAbove)
    {
        _objectAbove = objectAbove;
    }
}
