using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour, MineLocation
{
    [Header("====Prefab====")]
    [SerializeField] private WarehouseMiner minerPrefab;

    [Header("Manager")]
    [SerializeField] private WarehouseWorkManager warehouseManagerPrefab;
    [SerializeField] private Transform warehouseManagerPosition;

    [Header("====Extras====")]
    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorDepositLocation;
    [SerializeField] private Transform warehouseDepositLocation;

    public WarehouseWorkManager WorkManager { get; set; }

    private List<WarehouseMiner> _miners = new List<WarehouseMiner>();
    public List<WarehouseMiner> Miners => _miners;

    void Start()
    {
        AddMiner();
        CreateManager();
    }

    public void AddMiner()
    {
        WarehouseMiner newMiner = Instantiate(minerPrefab, warehouseDepositLocation.position, Quaternion.identity);
        newMiner.ElevatorDeposit = elevatorDeposit;
        newMiner.ElevatorDepositLocation = new Vector3(elevatorDepositLocation.position.x
        , warehouseDepositLocation.position.y);
        newMiner.WarehouseLocation = new Vector3(warehouseDepositLocation.position.x
        , warehouseDepositLocation.position.y);

        if (_miners.Count > 0)
        {
            newMiner.CollectCapacity = _miners[0].CollectCapacity;
            newMiner.CollectPerSecond = _miners[0].CollectPerSecond;
            newMiner.MoveSpeed = _miners[0].MoveSpeed;
        }

        _miners.Add(newMiner);
    }

    private void CreateManager()
    {
        WorkManager = Instantiate(warehouseManagerPrefab, warehouseManagerPosition.position, Quaternion.identity);
        WorkManager.transform.SetParent(transform);
        WorkManager.CurrentMineLocation = this;
    }

    public void ApplyManagerBoost()
    {
        Debug.Log("Warehouse Boost");

        switch (WorkManager.ManagerAssigned.BoostType)
        {
            case BoostType.Movement:
                foreach (WarehouseMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunMovementBoost(miner,
                    WorkManager.ManagerAssigned.BoostDuration,
                    WorkManager.ManagerAssigned.BoostValue);
                }
                break;
            case BoostType.Loading:
                foreach (WarehouseMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunLoadingBoost(miner,
                    WorkManager.ManagerAssigned.BoostDuration,
                    WorkManager.ManagerAssigned.BoostValue);
                }
                break;
        }
    }
}
