using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalGoldText;
    void Update()
    {
        totalGoldText.text = GoldManager.Instance.CurrentGold.ToCurrency();
    }
}
