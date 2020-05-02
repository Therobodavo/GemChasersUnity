using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuff : Buff
{
    public float instantHealValue = 35;
    public float delayedHealValue = 15;
    public HealBuff() : base(3)
    {
        energyCost = 25;
        instantPassiveCombinations = new IType.BuffType[3];

        instantPassiveCombinations[0] = IType.BuffType.Relax;
        instantPassiveCombinations[1] = IType.BuffType.Shield;
        instantPassiveCombinations[2] = IType.BuffType.Speed;

        delayedPassiveCombinations = new IType.BuffType[1];
        delayedPassiveCombinations[0] = IType.BuffType.Linger;
    }
    public override void InitAttack(PlayerAttack attackObj)
    {
        base.InitAttack(attackObj);
    }
    public override void OnInstantPassive(PlayerAttack attackObj)
    {
        base.OnInstantPassive(attackObj);
        attackObj.turnStatBuffs[4] = instantHealValue;
    }
    public override void OnDelayedPassive(PlayerAttack attackObj)
    {
        base.OnDelayedPassive(attackObj);
        attackObj.playerScript.turnEffects.Add(new BattleEffect(attackObj.playerScript,IType.Stat.Health, delayedHealValue, (IType.ElementType)attackObj.gemType.gemTypeID,3));
    }
}
