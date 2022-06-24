using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeContainer;
    [SerializeField] private Image panelMinerImage;
    [SerializeField] private TextMeshProUGUI panelTitle;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI nextBoost;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private Image progressBar;

    [Header("Upgrade Buttons")]
    [SerializeField] private GameObject[] upgradeButtons;
    [SerializeField] private Color buttonDisableColor;
    [SerializeField] private Color buttonEnableColor;

    [Header("Stats")]
    [SerializeField] private GameObject[] stats;

    [Header("Stat Title")]
    [SerializeField] private TextMeshProUGUI stat1Title;
    [SerializeField] private TextMeshProUGUI stat2Title;
    [SerializeField] private TextMeshProUGUI stat3Title;
    [SerializeField] private TextMeshProUGUI stat4Title;

    [Header("Stat Values")]
    [SerializeField] private TextMeshProUGUI stat1CurrentValue;
    [SerializeField] private TextMeshProUGUI stat2CurrentValue;
    [SerializeField] private TextMeshProUGUI stat3CurrentValue;
    [SerializeField] private TextMeshProUGUI stat4CurrentValue;

    [Header("Stat Upgrade Values")]
    [SerializeField] private TextMeshProUGUI stat1UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat2UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat3UpgradeValue;
    [SerializeField] private TextMeshProUGUI stat4UpgradeValue;

    [Header("Stat Icon")]
    [SerializeField] private Image stat1Icon;
    [SerializeField] private Image stat2Icon;
    [SerializeField] private Image stat3Icon;
    [SerializeField] private Image stat4Icon;

    [Header("Panel Info")]
    [SerializeField] private UpgradePanelInfo shaftMinerInfo;
    [SerializeField] private UpgradePanelInfo elevatorMinerInfo;
    [SerializeField] private UpgradePanelInfo warehouseMinerInfo;

    public int UpgradeAmount { get; set; }

    private Shaft _currentShaft;
    private UpgradePanelInfo _currentPanelInfo;
    private BaseUpgrade _currentUpgrade;
    private BasedMiner _currentMiner;
    private int _currentActiveButton;
    private int _minerCount;

    public void OpenCloseUpgradeContainer(bool status)
    {
        UpgradeX1(false);
        upgradeContainer.SetActive(status);
    }

    public void Upgrade()
    {
        if (GoldManager.Instance.CurrentGold >= _currentUpgrade.UpgradeCost)
        {
            _currentUpgrade.Upgrade(UpgradeAmount);
            UpdatePanelValues();
            RefreshUpgradeAmount();
        }
    }

    #region Upgrade Buttons

    public void UpgradeX1(bool animateButton)
    {
        ActivateButton(0, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(1, _currentUpgrade) ? 1 : 0;
        upgradeCost.text = GetUpgradeCost(1, _currentUpgrade).ToCurrency();
    }
    public void UpgradeX10(bool animateButton)
    {
        ActivateButton(1, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(10, _currentUpgrade) ? 10 : 0;
        upgradeCost.text = GetUpgradeCost(10, _currentUpgrade).ToCurrency();
    }
    public void UpgradeX50(bool animateButton)
    {
        ActivateButton(2, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(50, _currentUpgrade) ? 50 : 0;
        upgradeCost.text = GetUpgradeCost(50, _currentUpgrade).ToCurrency();
    }
    public void UpgradeMax(bool animateButton)
    {
        ActivateButton(3, animateButton);
        UpgradeAmount = CalculateUpgradeCount(_currentUpgrade);
        upgradeCost.text = GetUpgradeCost(UpgradeAmount, _currentUpgrade).ToCurrency();
    }

    private int CalculateUpgradeCount(BaseUpgrade upgrade)
    {
        int count = 0;
        float currentGold = GoldManager.Instance.CurrentGold;
        float currentUpgradeCost = upgrade.UpgradeCost;

        if (GoldManager.Instance.CurrentGold >= currentUpgradeCost)
        {
            for (float i = currentGold; i >= 0; i -= currentUpgradeCost)
            {
                count++;
                currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
            }
        }

        return count;
    }

    private bool CanUpgradeManyTimes(int upgradeAmount, BaseUpgrade upgrade)
    {
        int count = CalculateUpgradeCount(upgrade);
        if (count >= upgradeAmount)
        {
            return true;
        }

        return false;
    }

    private float GetUpgradeCost(int amount, BaseUpgrade upgrade)
    {
        float cost = 0f;
        float currentUpgradeCost = upgrade.UpgradeCost;

        for (int i = 0; i < amount; i++)
        {
            cost += currentUpgradeCost;
            currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
        }

        return cost;
    }

    private void ActivateButton(int buttonIndex, bool animateButton)
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].GetComponent<Image>().color = buttonDisableColor;
        }

        _currentActiveButton = buttonIndex;
        upgradeButtons[buttonIndex].GetComponent<Image>().color = buttonEnableColor;
        if (animateButton == true)
        {
            upgradeButtons[buttonIndex].transform.DOPunchPosition(transform.localPosition +
                                                                  new Vector3(0f, 5f, 0f), 0.5f).Play();
        }
    }

    private void RefreshUpgradeAmount()
    {
        switch (_currentActiveButton)
        {
            case 0:
                UpgradeX1(false);
                break;
            case 1:
                UpgradeX10(false);
                break;
            case 2:
                UpgradeX50(false);
                break;
            case 3:
                UpgradeMax(false);
                break;
        }
    }

    #endregion

    private void UpdateUpgradeInfo()
    {
        if (_currentPanelInfo.Location == Locations.Elevator)
        {
            stats[3].SetActive(false);
        }
        else
        {
            stats[3].SetActive(true);
        }

        panelTitle.text = _currentPanelInfo.PanelTitle;
        panelMinerImage.sprite = _currentPanelInfo.PanelMinerIcon;

        stat1Title.text = _currentPanelInfo.Stat1Title;
        stat2Title.text = _currentPanelInfo.Stat2Title;
        stat3Title.text = _currentPanelInfo.Stat3Title;
        stat4Title.text = _currentPanelInfo.Stat4Title;

        stat1Icon.sprite = _currentPanelInfo.Stat1Icon;
        stat2Icon.sprite = _currentPanelInfo.Stat2Icon;
        stat3Icon.sprite = _currentPanelInfo.Stat3Icon;
        stat4Icon.sprite = _currentPanelInfo.Stat4Icon;
    }

    private void UpdatePanelValues()
    {
        upgradeCost.text = _currentUpgrade.UpgradeCost.ToString();
        level.text = $"Level {_currentUpgrade.CurrentLevel}";
        progressBar.DOFillAmount(_currentUpgrade.GetNextBoostProgress(), 0.5f).Play();
        nextBoost.text = $"Next Boost at Level {_currentUpgrade.BoostLevel}";

        //Move Speed
        float minerMoveSpeed = _currentMiner.MoveSpeed;
        float collectMoveSpeedUpgrade = Mathf.Abs(minerMoveSpeed - (minerMoveSpeed * _currentUpgrade.MoveSpeedMultiplier));

        //Collect Per Second
        float minerCollectPerSecond = _currentMiner.CollectPerSecond;
        float collectPerSecondUpgrade = Mathf.Abs(minerCollectPerSecond - (minerCollectPerSecond * _currentUpgrade.CollectPerSecondMultiplier));

        //Collect Capacity
        float minerCollectCapacity = _currentMiner.CollectCapacity;
        float collectCapacityUpgrade = Mathf.Abs(minerCollectCapacity - (minerCollectCapacity * _currentUpgrade.CollectCapacityMultiplier));

        if (_currentPanelInfo.Location == Locations.Elevator)
        {
            stat1CurrentValue.text = $"{_currentMiner.CollectCapacity}";
            stat2CurrentValue.text = $"{_currentMiner.MoveSpeed}";
            stat3CurrentValue.text = $"{_currentMiner.CollectPerSecond}";

            stat1UpgradeValue.text = $"+{collectCapacityUpgrade}";
            stat2UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? $"+{collectMoveSpeedUpgrade}/s" : "+0";
            stat3UpgradeValue.text = $"+{collectPerSecondUpgrade}";
        }
        else
        {
            //Current Values
            stat1CurrentValue.text = $"{_minerCount}";
            stat2CurrentValue.text = $"{_currentMiner.MoveSpeed}";
            stat3CurrentValue.text = $"{_currentMiner.CollectPerSecond}";
            stat4CurrentValue.text = $"{_currentMiner.CollectCapacity}";

            //Miner Stat
            stat1UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? "+1" : "+0";
            stat2UpgradeValue.text = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? $"+{collectMoveSpeedUpgrade}/s" : "+0";
            stat3UpgradeValue.text = $"+{collectPerSecondUpgrade}";
            stat4UpgradeValue.text = $"+{collectCapacityUpgrade}";
        }

    }

    private void ShaftUpgradeRequest(Shaft selectedShaft, ShaftUpgrade selectedUpgrade)
    {
        _minerCount = selectedShaft.Miners.Count;
        _currentMiner = selectedShaft.Miners[0];
        _currentUpgrade = selectedUpgrade;
        _currentPanelInfo = shaftMinerInfo;

        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }

    private void ElevatorUpgradeRequest(ElevatorUpgrade selectedUpgrade)
    {
        _currentMiner = selectedUpgrade.GetComponent<Elevator>().Miner;
        _currentUpgrade = selectedUpgrade;
        _currentPanelInfo = elevatorMinerInfo;

        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }

    private void WarehouseUpgradeRequest(WarehouseUpgrade warehouseUpgrade)
    {
        _minerCount = warehouseUpgrade.GetComponent<Warehouse>().Miners.Count;
        _currentMiner = warehouseUpgrade.GetComponent<Warehouse>().Miners[0];
        _currentUpgrade = warehouseUpgrade;
        _currentPanelInfo = warehouseMinerInfo;

        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }

    void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest += WarehouseUpgradeRequest;
    }

    void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest -= WarehouseUpgradeRequest;
    }
}
