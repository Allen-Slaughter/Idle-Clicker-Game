using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftMiner : BasedMiner
{
    public Shaft CurrentShaft { get; set; }

    //将所有可能的位置使用我们当前创建的Shaft的引用
    public Vector3 DepositLocation => new Vector3(CurrentShaft.DepositLocation.position.x, transform.position.y);
    public Vector3 MiningLocation => new Vector3(CurrentShaft.MiningLocation.position.x, transform.position.y);

    //获得动画的哈希代码
    private int walkAnimation = Animator.StringToHash("Walk");
    private int miningAnimation = Animator.StringToHash("Mining");

    public override void OnClick()
    {
        MoveMiner(MiningLocation);
    }

    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetTrigger(walkAnimation);
    }

    protected override void CollectGold()
    {
        _animator.SetTrigger(miningAnimation);
        //获取收集时间
        float collectTime = CollectCapacity / CollectPerSecond;
        StartCoroutine(IECollect(CollectCapacity, collectTime));
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        CurrentGold = gold;
        ChangeGold();
        RotateMiner(-1);
        MoveMiner(DepositLocation);
    }

    protected override void DepositGold()
    {
        CurrentShaft.ShaftDeposit.DepositGold(CurrentGold);

        CurrentGold = 0;
        ChangeGold();
        RotateMiner(1);
        MoveMiner(MiningLocation);
    }
}
