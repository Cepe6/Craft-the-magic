using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour {
    private static MapController _instance;

    [SerializeField]
    private GameObject _imagePrefab;

    [SerializeField]
    private Color _grassColor;
    [SerializeField]
    private Color _savannahColor;
    [SerializeField]
    private Color _ironColor;
    [SerializeField]
    private Color _coalColor;
    [SerializeField]
    private Color _waterLightColor;
    [SerializeField]
    private Color _waterNormalColor;
    [SerializeField]
    private Color _waterDarkColor;

    [SerializeField]
    private GameObject _chunksWrapper;

    private Dictionary<TilesEnum, Color> _enumToColorDictionary = new Dictionary<TilesEnum, Color>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void AddChunk(float x, float y, TileData[,] chunkTilesData)
    {
        Texture2D map = new Texture2D(320, 320);

        for(int i = 0, chunkIndex = 0; i < 320; i += 5, chunkIndex++)
        {
            for(int j = 0, chunkJndex = 0; j < 320; j += 5, chunkJndex++)
            {
                for(int k = 0; k < 5; k++)
                {
                    for(int l = 0; l < 5; l++)
                    {
                        switch(chunkTilesData[chunkIndex, chunkJndex].GetTileType())
                        {
                            case TilesEnum.GRASS: map.SetPixel(i + k, j + l, _grassColor); break;
                            case TilesEnum.SAVANNAH: map.SetPixel(i + k, j + l, _savannahColor); break;
                            case TilesEnum.IRON_ORE: map.SetPixel(i + k, j + l, _ironColor); break;
                            case TilesEnum.COAL: map.SetPixel(i + k, j + l, _coalColor); break;
                            case TilesEnum.LIGHT_WATER: map.SetPixel(i + k, j + l, _waterLightColor); break;
                            case TilesEnum.NORMAL_WATER: map.SetPixel(i + k, j + l, _waterNormalColor); break;
                            case TilesEnum.DARK_WATER: map.SetPixel(i + k, j + l, _waterDarkColor); break;
                        }
                    }
                }
            }
        }

        map.wrapMode = TextureWrapMode.Clamp;
        map.Apply();

        Sprite mapSprite = Sprite.Create(map, new Rect(0, 0, map.width, map.height), new Vector2(0.5f, 0.5f));
        Image chunkImage = Instantiate(_imagePrefab, _chunksWrapper.transform).GetComponent<Image> ();
        chunkImage.gameObject.name = "Chunk(" + x + ", " + y + ")";
        chunkImage.sprite = mapSprite;
        chunkImage.rectTransform.localPosition = new Vector3(x * 320 + 160, y * 320 + 160, 0);
    }

    public static MapController Instance {
        get
        {
            return _instance;
        }
    }
}
