using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : MonoBehaviour
{
    public float CurrentGold { get; set; }

    public bool CanCurrentGold => CurrentGold > 0;

    public void DepositGold(float amount)
    {
        CurrentGold += amount;
    }

    public void RemoveGold(float amount)
    {
        if (amount <= CurrentGold)
        {
            CurrentGold -= amount;
        }
    }

    public float CollectGold(BasedMiner miner)
    {
        float minerCapacity = miner.CollectCapacity - miner.CurrentGold;
        return EvaluateAmountToCollect(minerCapacity);
    }

    public float EvaluateAmountToCollect(float minerCollectCapacity)
    {
        if (minerCollectCapacity <= CurrentGold)
        {
            return minerCollectCapacity;
        }

        return CurrentGold;
    }
}
