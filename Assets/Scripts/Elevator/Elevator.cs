using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, MineLocation
{
    [SerializeField] private Transform depositLocation;
    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private ElevatorMiner miner;

    [Header("Manager")]
    [SerializeField] private ElevatorWorkManager elevatorManagerPrefab;
    [SerializeField] private Transform elevatorManagerPosition;

    public ElevatorWorkManager WorkManager { get; set; }

    public Deposit ElevatorDeposit => elevatorDeposit;
    public Transform DepositLocation => depositLocation;
    public ElevatorMiner Miner => miner;

    void Start()
    {
        CreateManager();
    }

    private void CreateManager()
    {
        WorkManager = Instantiate(elevatorManagerPrefab, elevatorManagerPosition.position, Quaternion.identity);
        WorkManager.transform.SetParent(transform);
        WorkManager.CurrentMineLocation = this;
    }

    public void ApplyManagerBoost()
    {
        Debug.Log("Elevator Boost");

        switch (WorkManager.ManagerAssigned.BoostType)
        {
            case BoostType.Movement:
                WorkManagerController.Instance.RunMovementBoost(miner,
                WorkManager.ManagerAssigned.BoostDuration,
                WorkManager.ManagerAssigned.BoostValue);
                break;
            case BoostType.Loading:
                WorkManagerController.Instance.RunLoadingBoost(miner,
                WorkManager.ManagerAssigned.BoostDuration,
                WorkManager.ManagerAssigned.BoostValue);
                break;
        }
    }
}
