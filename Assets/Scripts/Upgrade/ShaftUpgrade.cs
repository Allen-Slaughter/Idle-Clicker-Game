using UnityEngine;

public class ShaftUpgrade : BaseUpgrade
{
    protected override void ExecuteUpgrade()
    {
        if (CurrentLevel % 10 == 0)
        {
            _shaft.CreateMiner();
        }

        foreach (ShaftMiner miner in _shaft.Miners)
        {
            miner.CollectCapacity *= CollectCapacityMultiplier;
            miner.CollectPerSecond *= CollectPerSecondMultiplier;

            if (CurrentLevel % 10 == 0)
            {
                miner.MoveSpeed *= MoveSpeedMultiplier;
            }
        }
    }
}
