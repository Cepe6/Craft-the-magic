using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResearchQueue : MonoBehaviour
{
    private static ResearchQueue _instance;

    [SerializeField]
    private GameObject _defaultSlot;
    [SerializeField]
    private GameObject _slotsWrapper;
    [SerializeField]
    private GameObject _entryGO;
    [SerializeField]
    private Slider _progressSlider;

    [SerializeField]
    private GameObject _craftingGO;

    private List<ResearchQueueEntry> _entries = new List<ResearchQueueEntry>();
    private PlayerInventoryController _inventory;

    private bool _currentlyResearching = false;
    private float _currentResearchingTime = 0f;

    private void Awake()
    {
        _inventory = GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<PlayerInventoryController>();
    }

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void AddEntry(Item item, RecipeSlot recipe, List<GameObject> results)
    {
        GameObject entry = Instantiate(_entryGO, _slotsWrapper.transform);
        entry.GetComponent<ResearchQueueEntry>().Init(item, recipe, results);

        _entries.Add(entry.GetComponent<ResearchQueueEntry>());

        if (!_currentlyResearching)
        {
            StartCoroutine(ResearchQueueCoroutine());
        }
    }

    public static ResearchQueue Instance
    {
        get { return _instance; }
    }

    private IEnumerator ResearchQueueCoroutine()
    {
        _currentlyResearching = true;

        while (_entries.Count > 0)
        {
            while (_entries.Count > 0 && _entries[0] == null)
            {
                _entries.RemoveAt(0);
            }

            if (_entries.Count == 0) { break; }
            float targetResearchTime = _entries[0].Recipe.ActionTime;
            _defaultSlot.GetComponent<ResearchQueueEntry>().Init(_entries[0].item, _entries[0].Recipe, _entries[0].Results);

            Destroy(_entries[0].gameObject);
            _entries.RemoveAt(0);

            _currentResearchingTime = 0f;
            while (_currentResearchingTime < targetResearchTime)
            {
                if (_defaultSlot == null)
                {
                    _progressSlider.value = 0f;
                    _currentResearchingTime = 0f;
                    break;
                }

                _currentResearchingTime += Time.deltaTime;
                _progressSlider.value = Mathf.Lerp(0f, 1f, _currentResearchingTime / targetResearchTime);

                yield return new WaitForEndOfFrame();
            }

            if (_defaultSlot != null)
            {
                for(int i = 0; i < _defaultSlot.GetComponent<ResearchQueueEntry> ().Results.Count; i++)
                {
                    Instantiate(_defaultSlot.GetComponent<ResearchQueueEntry>().Results[i], _craftingGO.transform);
                }
            }
        }

        _defaultSlot.GetComponent<ResearchQueueEntry>().Nullify();
        _progressSlider.value = 0f;
        _currentlyResearching = false;
    }
}
