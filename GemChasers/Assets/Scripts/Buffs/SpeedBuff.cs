using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Speed Buff Class
 * Programmed by David Knolls
 * 
 * Sets up functionality for speed buff
 * 
 * Effect: Increase speed of player
 */

public class SpeedBuff : Buff
{
    //Properties
    public float speedPercent = 2.0f;
    public SpeedBuff() : base(1)
    {
        energyCost = 15;

        //Sets up combinations
        instantAttackCombinations = new IType.BuffType[4];

        instantAttackCombinations[0] = IType.BuffType.Strength;
        instantAttackCombinations[2] = IType.BuffType.Combo;
        instantAttackCombinations[3] = IType.BuffType.Split;
        instantAttackCombinations[3] = IType.BuffType.Speed;

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

    //Function call for attacks triggered on use
    public override void OnInstantAttack(PlayerAttack attackObj)
    {
        base.OnInstantAttack(attackObj);

        attackObj.turnStatBuffs[2] *= speedPercent;
    }

    //Function call for effects triggered on use
    public override void OnInstantPassive(PlayerAttack attackObj)
    {
        base.OnInstantPassive(attackObj);
        attackObj.turnStatBuffs[2] *= speedPercent;
    }

    //Function call for effects triggered over X number of turns
    public override void OnDelayedPassive(PlayerAttack attackObj)
    {
        base.OnDelayedPassive(attackObj);
        if (attackObj != null) 
        {
            PlayerManager playerManager = attackObj.playerScript;
            if (playerManager != null) 
            {
                playerManager.turnEffects.Add(new BattleEffect(playerManager, IType.Stat.Speed, 1.2f, (IType.ElementType)attackObj.gemType.gemTypeID, 3));
            }
        }
            
    }

}
