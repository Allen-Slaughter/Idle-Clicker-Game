using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMiner : BasedMiner
{
    [SerializeField] private Elevator elevator;
    public Vector3 DepositLocation => new Vector3(transform.position.x, elevator.DepositLocation.position.y);

    private Deposit _CurrentShaftDeposit;
    private int _CurrentShaftIndex = -1;

    public override void OnClick()
    {
        MoveToNextLocation();
    }

    private void MoveToNextLocation()
    {
        _CurrentShaftIndex++;
        Shaft currentShaft = ShaftManager.Instance.Shafts[_CurrentShaftIndex];
        _CurrentShaftDeposit = currentShaft.ShaftDeposit;
        Vector3 shaftDepositLocation = currentShaft.DepositLocation.position;
        Vector3 fixedPos = new Vector3(transform.position.x, shaftDepositLocation.y);
        MoveMiner(fixedPos);
    }

    protected override void CollectGold()
    {
        if (_CurrentShaftIndex == ShaftManager.Instance.Shafts.Count - 1 && !_CurrentShaftDeposit.CanCurrentGold)
        {
            ChangeGold();
            MoveMiner(DepositLocation);
            _CurrentShaftIndex = -1;
            return;
        }

        float amountToCollect = _CurrentShaftDeposit.CollectGold(this);
        float collectTime = amountToCollect / CollectPerSecond;
        StartCoroutine(IECollect(amountToCollect, collectTime));
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        if (CurrentGold < CollectCapacity && gold < (CollectCapacity - CurrentGold))
        {
            CurrentGold += gold;
            _CurrentShaftDeposit.RemoveGold(gold);
        }

        yield return new WaitForSeconds(0.5f);

        if (CurrentGold < CollectCapacity && _CurrentShaftIndex != ShaftManager.Instance.Shafts.Count - 1)
        {
            MoveToNextLocation();
        }
        else
        {
            ChangeGold();
            MoveMiner(DepositLocation);
            _CurrentShaftIndex = -1;
        }
    }

    protected override void DepositGold()
    {
        if (CurrentGold <= 0)
        {
            ChangeGold();
            MoveToNextLocation();
            return;
        }

        float depositTime = CurrentGold / CollectPerSecond;
        StartCoroutine(IEDeposit(depositTime));
    }

    protected override IEnumerator IEDeposit(float depositTime)
    {
        yield return new WaitForSeconds(depositTime);

        elevator.ElevatorDeposit.DepositGold(CurrentGold);
        CurrentGold = 0;

        ChangeGold();
        MoveToNextLocation();
    }
}
