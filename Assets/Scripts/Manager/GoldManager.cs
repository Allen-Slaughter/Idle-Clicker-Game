using UnityEngine;
public class GoldManager : Singleton<GoldManager>
{
    [SerializeField] private float testGold = 0;

    public float CurrentGold { get; set; }
    private readonly string GOLD_KEY = "MY_GOLD";

    void Start()
    {
        PlayerPrefs.DeleteAll();
        LoadGold();
    }

    private void LoadGold()
    {
        CurrentGold = PlayerPrefs.GetFloat(GOLD_KEY, testGold);
    }

    public void AddGold(float amount)
    {
        CurrentGold += amount;
        PlayerPrefs.SetFloat(GOLD_KEY, CurrentGold);
        PlayerPrefs.Save();
    }

    public void RemoveGold(float amount)
    {
        if (amount <= CurrentGold)
        {
            CurrentGold -= amount;
            PlayerPrefs.SetFloat(GOLD_KEY, CurrentGold);
            PlayerPrefs.Save();
        }
    }
}
