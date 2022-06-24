using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElevatorUI : MonoBehaviour
{
    public static Action<ElevatorUpgrade> OnUpgradeRequest;

    [SerializeField] private TextMeshProUGUI elevatorDepositGoldText;
    [SerializeField] private TextMeshProUGUI currentLevel;

    private Elevator _elevator;
    private ElevatorUpgrade _elevatorUpgrade;

    void Start()
    {
        _elevator = GetComponent<Elevator>();
        _elevatorUpgrade = GetComponent<ElevatorUpgrade>();
    }

    void Update()
    {
        elevatorDepositGoldText.text = _elevator.ElevatorDeposit.CurrentGold.ToCurrency();
    }

    public void OpenElevatorUpgrade()
    {
        OnUpgradeRequest?.Invoke(_elevatorUpgrade);
    }

    private void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_elevatorUpgrade == upgrade)
        {
            currentLevel.text = upgrade.CurrentLevel.ToString();
        }
    }

    void OnEnable()
    {
        ElevatorUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    void OnDisable()
    {
        ElevatorUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
