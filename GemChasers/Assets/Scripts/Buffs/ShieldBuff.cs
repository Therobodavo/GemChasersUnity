using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : Buff
{
    public float defenseMultiplier = 1.5f;
    public ShieldBuff() : base(2)
    {
        energyCost = 10;

        instantPassiveCombinations = new IType.BuffType[3];
        instantPassiveCombinations[0] = IType.BuffType.Heal;
        instantPassiveCombinations[0] = IType.BuffType.Relax;
        instantPassiveCombinations[0] = IType.BuffType.Speed;

        delayedPassiveCombinations = new IType.BuffType[1];
        delayedPassiveCombinations[0] = IType.BuffType.Linger;
    }
    public override void OnInstantPassive(PlayerAttack attackObj)
    {
        base.OnInstantPassive(attackObj);
        attackObj.turnStatBuffs[1] = defenseMultiplier;
    }
    public override void OnDelayedPassive(PlayerAttack attackObj)
    {
        base.OnDelayedPassive(attackObj);
        attackObj.playerScript.turnEffects.Add(new BattleEffect(attackObj.playerScript,IType.Stat.Defense, defenseMultiplier, (IType.ElementType)attackObj.gemType.gemTypeID, 3));
    }
}
