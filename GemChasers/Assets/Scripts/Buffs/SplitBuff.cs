using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBuff : Buff
{
    public SplitBuff() : base(5)
    {
        energyCost = 25;

        instantAttackCombinations = new IType.BuffType[3];

        instantAttackCombinations[0] = IType.BuffType.Strength;
        instantAttackCombinations[1] = IType.BuffType.Speed;
        instantAttackCombinations[2] = IType.BuffType.Combo;
    }

    public override void InitAttack(PlayerAttack attackObj)
    {
        base.InitAttack(attackObj);
        attackObj.targetAllSide = true;
    }
}
