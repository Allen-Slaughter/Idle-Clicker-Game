using System;
using TMPro;
using UnityEngine;

public class WarehouseUI : MonoBehaviour
{
    public static Action<WarehouseUpgrade> OnUpgradeRequest;

    [SerializeField] private TextMeshProUGUI currentLevel;

    private WarehouseUpgrade _warehouseUpgrade;

    void Start()
    {
        _warehouseUpgrade = GetComponent<WarehouseUpgrade>();
    }

    public void OpenWarehouseUpgradePanel()
    {
        OnUpgradeRequest?.Invoke(_warehouseUpgrade);
    }

    private void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_warehouseUpgrade == upgrade)
        {
            currentLevel.text = upgrade.CurrentLevel.ToString();
        }
    }

    void OnEnable()
    {
        WarehouseUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    void OnDisable()
    {
        WarehouseUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
