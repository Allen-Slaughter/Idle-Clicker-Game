using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class WorkManagerCard : MonoBehaviour
{
    public static Action<WorkManagerCard> OnAssignRequest;

    [SerializeField] private Image boostIcon;
    [SerializeField] private TextMeshProUGUI managerType;
    [SerializeField] private TextMeshProUGUI boostDuration;
    [SerializeField] private TextMeshProUGUI boostDescription;

    public WorkManagerInfo ManagerInfoAssigned { get; set; }

    public void SetupWorkManagerCard(WorkManagerInfo managerInfo)
    {
        ManagerInfoAssigned = managerInfo;
        boostIcon.sprite = managerInfo.BoostIcon;
        managerType.text = managerInfo.ManagerType.ToString();
        managerType.color = managerInfo.LevelColor;
        boostDuration.text = $"Duration: {managerInfo.BoostDuration}";
        boostDescription.text = managerInfo.BoostDescription;
    }

    public void AssignManager()
    {
        OnAssignRequest?.Invoke(this);
        gameObject.SetActive(false);
    }
}
