using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BasedMiner : MonoBehaviour, IClickable
{
    [Header("====INITIAL VALUES====")]
    //初始收集容量
    [SerializeField] protected float initialCollectCapacity;
    //每秒初始收集
    [SerializeField] protected float initialCollectPerSecond;
    [SerializeField] protected float moveSpeed;

    public float CurrentGold { get; set; }
    public float CollectCapacity { get; set; }
    public float CollectPerSecond { get; set; }
    public bool IsTimeToCollectGold { get; set; }
    public bool MinerClicked { get; set; }
    public float MoveSpeed { get; set; }

    protected Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        IsTimeToCollectGold = true;

        CollectCapacity = initialCollectCapacity;
        CollectPerSecond = initialCollectPerSecond;
        MoveSpeed = moveSpeed;
    }

    private void OnMouseDown()
    {
        if (!MinerClicked)
        {
            OnClick();
            MinerClicked = true;
        }
    }

    public virtual void OnClick()
    {

    }

    protected virtual void MoveMiner(Vector3 newPosition)
    {
        //OnComplete是itween移动完成以后所执行的内容
        transform.DOMove(newPosition, MoveSpeed).SetEase(Ease.Linear).OnComplete((() =>
        {
            if (IsTimeToCollectGold)
            {
                //收集金矿返回存款位置
                CollectGold();
            }
            else
            {
                //Miner返回采矿位置
                DepositGold();
            }
        })).Play();
    }

    protected virtual void CollectGold()
    {

    }

    //需要收集的黄金及收集的时间
    protected virtual IEnumerator IECollect(float gold, float collectTime)
    {
        yield return null;
    }

    protected virtual void DepositGold()
    {

    }

    protected virtual IEnumerator IEDeposit(float depositTime)
    {
        yield return null;
    }

    protected void ChangeGold()
    {
        IsTimeToCollectGold = !IsTimeToCollectGold;
    }

    protected void RotateMiner(int direction)
    {
        if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


}
