using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Split Buff Class
 * Programmed by David Knolls
 * 
 * Sets up functionality for split buff
 * 
 * Effect: Attack all enemies
 */

public class SplitBuff : Buff
{
    public SplitBuff() : base(5)
    {
        energyCost = 20;

        //Sets up combinations
        instantAttackCombinations = new IType.BuffType[3];

        instantAttackCombinations[0] = IType.BuffType.Strength;
        instantAttackCombinations[1] = IType.BuffType.Speed;
        instantAttackCombinations[2] = IType.BuffType.Combo;
    }

    public override void InitAttack(PlayerAttack attackObj)
    {
        base.InitAttack(attackObj);

        //Set variable to target all enemies
        attackObj.targetAllSide = true;
    }
}
