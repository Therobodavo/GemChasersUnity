using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Combo Buff Class
 * Programmed by David Knolls
 * 
 * Sets up functionality for combo buff
 */

public class ComboBuff : Buff
{

    public ComboBuff() : base(6)
    {
        energyCost = 25;

        instantAttackCombinations = new IType.BuffType[3];

        instantAttackCombinations[0] = IType.BuffType.Strength;
        instantAttackCombinations[1] = IType.BuffType.Speed;
        instantAttackCombinations[2] = IType.BuffType.Split;
    }

    public override void OnInstantAttack(PlayerAttack attackObj)
    {
        base.OnInstantAttack(attackObj);

        attackObj.isComboAttack = true;
    }
}
