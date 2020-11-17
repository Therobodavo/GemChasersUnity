using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Strength Buff Class
 * Programmed by David Knolls
 * 
 * Sets up functionality for strength buff
 * 
 * Effect: Increases damage dealt
 */

public class StrengthBuff : Buff
{
    //Properties
    protected float attackPercent = 1.5f;

    //Constructor - ID = 0
    public StrengthBuff() : base(0)
    {
        energyCost = 20;

        //Sets up combinations
        instantAttackCombinations = new IType.BuffType[4];

        instantAttackCombinations[0] = IType.BuffType.Strength;
        instantAttackCombinations[1] = IType.BuffType.Speed;
        instantAttackCombinations[2] = IType.BuffType.Combo;
        instantAttackCombinations[3] = IType.BuffType.Split;

        delayedAttackCombinations = new IType.BuffType[1];

        delayedAttackCombinations[0] = IType.BuffType.Linger;
    }

    //Function call for attacks that deal damage on use
    public override void OnInstantAttack(PlayerAttack attackObj)
    {
        base.OnInstantAttack(attackObj);
        
        attackObj.turnStatBuffs[0] *= attackPercent; 
    }

    //Function call for attacks that deal damage over X number of turns
    public override void OnDelayedAttack(PlayerAttack attackObj)
    {
        base.OnDelayedAttack(attackObj);
        if (attackObj != null) 
        {
            PlayerManager playerManager = attackObj.playerScript;
            if (playerManager != null) 
            {
                if (playerManager.currentBattleArea != null)
                {
                    //Get enemies in battle
                    IBattle[] enemyScripts = playerManager.currentBattleArea.GetEnemyScripts();

                    UI uiScript = playerManager.UIScript;
                    if (uiScript != null)
                    {
                        if (enemyScripts.Length >= playerManager.UIScript.selectedEnemyTarget + 1)
                        {
                            //Create delayed effect for move (health deals damage)
                            float damageCalc = (((playerManager.baseStats[0] * playerManager.statModifiers[attackObj.gemType.gemTypeID, 0]) * attackObj.turnStatBuffs[0]) + 10f) / 3;
                            enemyScripts[uiScript.selectedEnemyTarget].turnDamage.Add(new BattleEffect(playerManager, IType.Stat.Health, damageCalc, (IType.ElementType)attackObj.gemType.gemTypeID, 3));
                        }
                    }
                }
            }   
        }
    }
}
