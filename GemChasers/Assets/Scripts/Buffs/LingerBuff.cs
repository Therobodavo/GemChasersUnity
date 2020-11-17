using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Linger Buff Class
 * Programmed by David Knolls
 * 
 * Sets up functionality for linger buff
 */

public class LingerBuff : Buff
{
    protected float turnsActive = 1.5f;
    public LingerBuff() : base(7)
    {
        energyCost = 10;

        delayedPassiveCombinations = new IType.BuffType[4];
        delayedPassiveCombinations[0] = IType.BuffType.Heal;
        delayedPassiveCombinations[1] = IType.BuffType.Relax;
        delayedPassiveCombinations[2] = IType.BuffType.Shield;
        delayedPassiveCombinations[3] = IType.BuffType.Speed;

        delayedAttackCombinations = new IType.BuffType[1];
        delayedAttackCombinations[0] = IType.BuffType.Strength;
    }

    //Function call for attacks that deal damage over X number of turns
    public override void OnDelayedAttack(PlayerAttack attackObj)
    {
        base.OnDelayedAttack(attackObj);
    }

    //Function call for effects triggered over X number of turns
    public override void OnDelayedPassive(PlayerAttack attackObj)
    {
        base.OnDelayedPassive(attackObj);
    }


}
