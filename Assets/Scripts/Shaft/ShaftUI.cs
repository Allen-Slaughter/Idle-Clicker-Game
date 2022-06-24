using System;
using TMPro;
using UnityEngine;

public class ShaftUI : MonoBehaviour
{
    public static Action<Shaft, ShaftUpgrade> OnUpgradeRequest;

    [SerializeField] private TextMeshProUGUI depositGold;
    [SerializeField] private TextMeshProUGUI shaftID;
    [SerializeField] private TextMeshProUGUI shaftLevel;
    [SerializeField] private TextMeshProUGUI newShaftCost;
    [SerializeField] private GameObject newShaftButton;

    private Shaft _Shaft;
    private ShaftUpgrade _ShaftUpgrade;

    void Awake()
    {
        _Shaft = GetComponent<Shaft>();
        _ShaftUpgrade = GetComponent<ShaftUpgrade>();
    }

    void Update()
    {
        depositGold.text = _Shaft.ShaftDeposit.CurrentGold.ToCurrency();
    }

    public void AddShaft()
    {
        if (GoldManager.Instance.CurrentGold >= ShaftManager.Instance.ShaftCost)
        {
            GoldManager.Instance.RemoveGold(ShaftManager.Instance.ShaftCost);
            ShaftManager.Instance.AddShaft();
            newShaftButton.SetActive(false);
        }
    }

    public void OpenUpgradeContainer()
    {
        OnUpgradeRequest?.Invoke(_Shaft, _ShaftUpgrade);
    }

    public void SetShaftUI(int ID)
    {
        _Shaft.ShaftID = ID;
        shaftID.text = (ID + 1).ToString();
    }

    public void SetNewShaftCost(float newCost)
    {
        newShaftCost.text = newCost.ToString();
    }

    private void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_ShaftUpgrade == upgrade)
        {
            shaftLevel.text = upgrade.CurrentLevel.ToString();
        }
    }

    void OnEnable()
    {
        ShaftUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    void OnDisable()
    {
        ShaftUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
