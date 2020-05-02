using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxBuff : Buff
{
    public float instantEnergyValue = 75;
    public float delayedEnergyValue = 35;
    public RelaxBuff() : base(4)
    {
        energyCost = 25;
        instantPassiveCombinations = new IType.BuffType[3];

        instantPassiveCombinations[0] = IType.BuffType.Heal;
        instantPassiveCombinations[1] = IType.BuffType.Shield;
        instantPassiveCombinations[2] = IType.BuffType.Speed;

        delayedPassiveCombinations = new IType.BuffType[1];
        delayedPassiveCombinations[0] = IType.BuffType.Linger;
    }
    public override void OnInstantPassive(PlayerAttack attackObj)
    {
        base.OnInstantPassive(attackObj);
        attackObj.turnStatBuffs[4] = instantEnergyValue;
    }
    public override void OnDelayedPassive(PlayerAttack attackObj)
    {
        base.OnDelayedPassive(attackObj);
        attackObj.playerScript.turnEffects.Add(new BattleEffect(attackObj.playerScript,IType.Stat.Energy, delayedEnergyValue, (IType.ElementType)attackObj.gemType.gemTypeID, 3));
    }
}
