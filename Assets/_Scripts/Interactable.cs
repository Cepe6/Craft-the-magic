using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer;

    private Material _onHoverMaterial;
    private GameObject _player;

    // Use this for initialization
    void OnEnable()
    {
        _onHoverMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void OnMouseExit()
    {
        if (isActiveAndEnabled)
        {
            List<Material> materials = new List<Material>();
            materials.Add(_renderer.materials[0]);
            _renderer.materials = materials.ToArray();
        }
    }

    private void OnMouseOver()
    {
        if (isActiveAndEnabled)
        {
            bool inRange = Vector3.Distance(transform.position, _player.transform.position) <= GlobalVariables.INTERACT_DISTANCE;

            if (!inRange)
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
}
