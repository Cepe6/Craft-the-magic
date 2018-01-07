using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
    [SerializeField]
    private GameObject _dropPrefab;

    private PlayerController _playerController;
    private ChunksController _chunksController;

	// Use this for initialization
	void Start () {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _chunksController = GetComponent<ChunksController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnDrop(Item item, int ammount)
    {
        int remainingAmmount = ammount;
        if (item.isStackable)
        {
            foreach (GameObject drop in _playerController.GetCollidingDrops())
            {
                if (remainingAmmount == 0) { return; }

                if (drop.GetComponent<Drop>().item.Equals(item))
                {
                    remainingAmmount = drop.GetComponent<Drop>().AddToAmmountAndReturnRemaining(remainingAmmount);
                }
            }
        }

        while(true)
        {
            float xCoord = Random.Range(-GlobalVariables.DROP_SIZE / 2, GlobalVariables.DROP_SIZE / 2);
            float yCoord = Random.Range(-GlobalVariables.DROP_SIZE / 2, GlobalVariables.DROP_SIZE / 2);
            Vector3 targetCoordinate = new Vector3(_playerController.transform.position.x + xCoord, 0.1f, _playerController.transform.position.z + yCoord);
            if (_chunksController.IsWalkable(targetCoordinate.x, targetCoordinate.z) == 1f)
            {
                Drop newDrop = Instantiate(_dropPrefab, targetCoordinate, _dropPrefab.transform.rotation).GetComponent<Drop>();
                newDrop.InitWithItem(item, remainingAmmount);
                break;
            }
        }
    }


}
