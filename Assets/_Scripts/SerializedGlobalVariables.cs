using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedGlobalVariables : MonoBehaviour {
    private static SerializedGlobalVariables _instance;
    public static SerializedGlobalVariables instance
    {
        get
        {
            if (_instance == null)
                _instance = new SerializedGlobalVariables ();

            return _instance;
        }
    }

    [SerializeField]
    private GameObject _dragContainerPrefab;
    public GameObject dragContainerPrefab
    {
        get
        {
            return _dragContainerPrefab;
        }
    }

    [SerializeField]
    private Color _slotOnHoverColor;
    public Color slotOnHoverColor
    {
        get
        {
            return _slotOnHoverColor;
        }
    }

    [SerializeField]
    private Color _interactableInRangeColor;
    [SerializeField]
    private Color _interactableOutOfRangeColor;
    public Color interactableInRangeColor
    {
        get
        {
            return _interactableInRangeColor;
        }
    }
    public Color interactableOutOfRangeColor
    {
        get
        {
            return _interactableOutOfRangeColor;
        }
    }

    [SerializeField]
    private GameObject _itemInfoPanel = null;
    public GameObject ItemInfoPanel
    {
        get
        {
            return _itemInfoPanel;
        }
    }

    [SerializeField]
    private Material _interactableMaterial;
    public Material InteractableMaterial {
        get
        {
            return _interactableMaterial;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }   
    }
}
