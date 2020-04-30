using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    public float speedPercent = 2.0f;
    public SpeedBuff() : base(1)
    {
        energyCost = 30;

        instantAttackCombinations = new IType.BuffType[4];

        instantAttackCombinations[0] = IType.BuffType.Strength;
        instantAttackCombinations[2] = IType.BuffType.Combo;
        instantAttackCombinations[3] = IType.BuffType.Split;

        instantPassiveCombinations = new IType.BuffType[3];

        instantPassiveCombinations[0] = IType.BuffType.Heal;
        instantPassiveCombinations[1] = IType.BuffType.Relax;
        instantPassiveCombinations[2] = IType.BuffType.Shield;

        delayedPassiveCombinations = new IType.BuffType[1];
        delayedPassiveCombinations[0] = IType.BuffType.Linger;
    }
    public override void InitAttack(PlayerAttack attackObj)
    {
        base.InitAttack(attackObj);
    }
    public override void OnInstantAttack(PlayerAttack attackObj)
    {
        base.OnInstantAttack(attackObj);

        attackObj.turnStatBuffs[2] *= speedPercent;
    }
    public override void OnInstantPassive(PlayerAttack attackObj)
    {
        base.OnInstantPassive(attackObj);
        attackObj.turnStatBuffs[2] *= speedPercent;
    }
    public override void OnDelayedPassive(PlayerAttack attackObj)
    {
        base.OnDelayedPassive(attackObj);
        attackObj.playerScript.turnEffects.Add(new BattleEffect(attackObj.playerScript,IType.Stat.Speed,1.2f,(IType.ElementType)attackObj.gemType.gemTypeID,3));
    }

}
