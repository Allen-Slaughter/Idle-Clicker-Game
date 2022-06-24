using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour, MineLocation
{
    [Header("Prefab")]
    [SerializeField] private ShaftMiner minePrefab;
    [SerializeField] private Deposit depositPrefab;

    [Header("Manager")]
    [SerializeField] private ShaftWorkManager shaftManagerPrefab;
    [SerializeField] private Transform shaftManagerPosition;

    [Header("Locations")]
    [SerializeField] private Transform miningLocation;
    [SerializeField] private Transform depositLocation;
    [SerializeField] private Transform depositCreationLocation;

    public int ShaftID { get; set; }
    //只读MiningLocation属性以表达式主体定义的形式实现，该表达式主体定义返回私有miningLocation字段值
    public Transform MiningLocation => miningLocation;
    public Transform DepositLocation => depositLocation;
    public Deposit ShaftDeposit { get; set; }
    public ShaftUI ShaftUI { get; set; }
    public ShaftWorkManager WorkManager { get; set; }

    private List<ShaftMiner> _miners = new List<ShaftMiner>();
    public List<ShaftMiner> Miners => _miners;

    void Awake()
    {
        ShaftUI = GetComponent<ShaftUI>();
    }

    void Start()
    {
        CreateMiner();
        CreateDeposit();
        CreateManager();
    }

    public void CreateMiner()
    {
        ShaftMiner newMiner = Instantiate(minePrefab, depositLocation.position, Quaternion.identity);
        newMiner.CurrentShaft = this;
        newMiner.transform.SetParent(transform);

        if (_miners.Count > 0)
        {
            newMiner.CollectCapacity = _miners[0].CollectCapacity;
            newMiner.CollectPerSecond = _miners[0].CollectPerSecond;
            newMiner.MoveSpeed = _miners[0].MoveSpeed;
        }

        _miners.Add(newMiner);
    }

    private void CreateDeposit()
    {
        ShaftDeposit = Instantiate(depositPrefab, depositCreationLocation.position, Quaternion.identity);
        ShaftDeposit.transform.SetParent(transform);
    }

    private void CreateManager()
    {
        WorkManager = Instantiate(shaftManagerPrefab, shaftManagerPosition.position, Quaternion.identity);
        WorkManager.transform.SetParent(transform);
        WorkManager.CurrentMineLocation = this;
    }

    public void ApplyManagerBoost()
    {
        Debug.Log("Shaft Boost");

        switch (WorkManager.ManagerAssigned.BoostType)
        {
            case BoostType.Movement:
                foreach (ShaftMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunMovementBoost(miner,
                    WorkManager.ManagerAssigned.BoostDuration,
                    WorkManager.ManagerAssigned.BoostValue);
                }
                break;
            case BoostType.Loading:
                foreach (ShaftMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunLoadingBoost(miner,
                    WorkManager.ManagerAssigned.BoostDuration,
                    WorkManager.ManagerAssigned.BoostValue);
                }
                break;
        }
    }
}
