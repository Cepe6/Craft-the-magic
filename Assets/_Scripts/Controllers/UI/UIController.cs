using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField]
    private GameObject _inventoryPanel;
    [SerializeField]
    private GameObject _craftingPanel;
    [SerializeField]
    private GameObject _researchPanel;
    [SerializeField]
    private GameObject _menuPanel;
    [SerializeField]
    private GameObject _mapPanel;

    [SerializeField]
    private HotBarController _hotBarController;

    [SerializeField]
    private Button _backToMenuBtn;
    [SerializeField]
    private Button _exitGameBtn;

    private void Start()
    {
        //SceneController sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();

        //_backToMenuBtn.onClick.AddListener(sceneController.OnBackToMenu);
        //_exitGameBtn.onClick.AddListener(sceneController.OnExit);
    }

    public void ShowInventory()
    {
        _inventoryPanel.GetComponent<Canvas> ().enabled = true;
    }

    public void CloseInventory()
    {
        _inventoryPanel.GetComponent<Canvas> ().enabled = false;
    }

    public void ShowMenuPanel()
    {
        _menuPanel.SetActive(true);
    }

    public void CloseMenuPanel()
    {
        _menuPanel.SetActive(false);
    }

    public bool MenuPanelState()
    {
        return _menuPanel.activeSelf;
    }

    public void ShowMapPanel()
    {
        _mapPanel.GetComponent <Canvas>().enabled = true;
    }

    public void CloseMapPanel()
    {
        _mapPanel.GetComponent <Canvas>().enabled = false;
    }

    public bool MapPanelState()
    {
        return _mapPanel.GetComponent <Canvas>().isActiveAndEnabled;
    }

    public bool InventoryState()
    {
        return _inventoryPanel.GetComponent<Canvas>().isActiveAndEnabled;
    }

    public void ChangeHotbarPosition(int index)
    {
        _hotBarController.ChangePosition(index);
    }

    public void ShowCraftingPanel()
    {
        _craftingPanel.SetActive(true);
    }

    public void CloseCraftingPanel()
    {
        _craftingPanel.SetActive(false);

    }

    public void ShowResearchPanel()
    {
        _researchPanel.SetActive(true);
    }

    public void CloseResearchPanel()
    {
        _researchPanel.SetActive(false);
    }
}
