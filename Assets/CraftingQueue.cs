using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingQueue : MonoBehaviour {
    private static CraftingQueue _instance;

    [SerializeField]
    private GameObject _defaultSlot;
    [SerializeField]
    private GameObject _slotsWrapper;
    [SerializeField]
    private GameObject _entryGO;
    [SerializeField]
    private Slider _progressSlider;

    private List<QueueEntry> _entries = new List<QueueEntry> ();
    private PlayerInventoryController _inventory;

    private bool _currentlyCrafting = false;
    private float _currentCraftingTime = 0f;

    private void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventoryController>();
    }

    private void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public void AddEntry(Item item, int ammount, RecipeSlot recipe)
    {
        GameObject entry = Instantiate(_entryGO, _slotsWrapper.transform);
        entry.GetComponent<QueueEntry>().Init(item, ammount, recipe);

        _entries.Add(entry.GetComponent<QueueEntry> ());

        if(!_currentlyCrafting)
        {
            StartCoroutine(CraftQueue());
        }
    }

    public static CraftingQueue Instance
    {
        get { return _instance; }
    }

    private IEnumerator CraftQueue()
    {
        _currentlyCrafting = true;
        while(_entries.Count > 0)
        {
            float targetCraftTime = _entries[0].Recipe.CraftTime * _entries[0].Ammount;
            _defaultSlot.GetComponent<QueueEntry>().Init(_entries[0].item, _entries[0].Ammount, _entries[0].Recipe);

            Destroy(_entries[0].gameObject);
            _entries.RemoveAt(0);

            for (int i = 0; i < _defaultSlot.GetComponent<QueueEntry>().Ammount; i++)
            {
                _currentCraftingTime = 0f;
                while (_currentCraftingTime < targetCraftTime)
                {
                    _currentCraftingTime += Time.deltaTime;
                    _progressSlider.value = Mathf.Lerp(0f, 1f, _currentCraftingTime / targetCraftTime);

                    yield return new WaitForEndOfFrame();
                }

                _inventory.AddOrDrop(_defaultSlot.GetComponent<QueueEntry>().item, 1);
                _defaultSlot.GetComponent<QueueEntry>().DecrementAmmount();
            }
        }

        _defaultSlot.GetComponent<QueueEntry>().Nullify();
        _progressSlider.value = 0f;
        _currentlyCrafting = false;
    }
}
