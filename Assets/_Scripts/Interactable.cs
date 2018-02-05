using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer;

    private Material _onHoverMaterial;
    private GameObject _player;
    private BoxCollider _collider;

    private bool _pointerOver;

    // Use this for initialization
    void OnEnable()
    {
        _onHoverMaterial = new Material(SerializedGlobalVariables.instance.InteractableMaterial);
        _player = GameObject.FindGameObjectWithTag("Player");
        _collider = GetComponent<BoxCollider> ();
    }

    private void Update()
    {
        if(_pointerOver && EventSystem.current.IsPointerOverGameObject ())
        {
            OnMouseExit();
        }
    }

    private void OnMouseExit()
    {
        if (isActiveAndEnabled)
        {
            _pointerOver = false;
            List<Material> materials = new List<Material>();
            materials.Add(_renderer.materials[0]);
            _renderer.materials = materials.ToArray();
        }
    }

    private void OnMouseOver()
    {
        if (isActiveAndEnabled && !EventSystem.current.IsPointerOverGameObject())
        {
            _pointerOver = true;
            if (!IsInteractable())
            {
                _onHoverMaterial.color = SerializedGlobalVariables.instance.interactableOutOfRangeColor;
            }
            else
            {
                _onHoverMaterial.color = SerializedGlobalVariables.instance.interactableInRangeColor;
            }
            List<Material> materials = new List<Material>();
            materials.Add(_renderer.materials[0]);
            materials.Add(_onHoverMaterial);
            _renderer.materials = materials.ToArray();
        }
    }

    public bool IsInteractable()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        bool inRange = Vector3.Distance(_player.transform.position, _collider.ClosestPointOnBounds(_player.transform.position)) <= GlobalVariables.INTERACT_DISTANCE;

        return inRange;
    }
}
