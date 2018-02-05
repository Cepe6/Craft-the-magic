using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;    

public class ItemToolTipController : MonoBehaviour {

    [SerializeField]
    private GameObject _itemNameGO;
    [SerializeField]
    private GameObject _optionalInfoGO;
    
    // Update is called once per frame
    private void Update () {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y - GetComponent<RectTransform>().rect.height, 0f);

        if(!EventSystem.current.IsPointerOverGameObject())
        {
            Destroy(this.gameObject);
        }
	}

    public void Initialize(string itemName, params GameObject[] optionalGOs)
    {
        _itemNameGO.GetComponent<Text>().text = itemName;

        foreach (GameObject GO in optionalGOs)
        {
            GO.transform.SetParent(transform.Find("OptionalInfo").transform);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
    }
}
