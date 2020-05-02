using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthBuff : Buff
{
    protected float attackPercent = 1.5f;
    public StrengthBuff() : base(0)
    {
        energyCost = 20;

        instantAttackCombinations = new IType.BuffType[4];

        instantAttackCombinations[0] = IType.BuffType.Strength;
        instantAttackCombinations[1] = IType.BuffType.Speed;
        instantAttackCombinations[2] = IType.BuffType.Combo;
        instantAttackCombinations[3] = IType.BuffType.Split;

        delayedAttackCombinations = new IType.BuffType[1];
        delayedAttackCombinations[0] = IType.BuffType.Linger;
    }

    public override void OnInstantAttack(PlayerAttack attackObj)
    {
        base.OnInstantAttack(attackObj);

        attackObj.turnStatBuffs[0] *= attackPercent; 
    }
    public override void OnDelayedAttack(PlayerAttack attackObj)
    {
        base.OnDelayedAttack(attackObj);

        attackObj.playerScript.currentBattleArea.GetEnemyScripts()[attackObj.playerScript.UIScript.selectedEnemyTarget].turnDamage.Add(new BattleEffect(attackObj.playerScript,IType.Stat.Health, (((attackObj.playerScript.baseStats[0] * attackObj.playerScript.statModifiers[attackObj.gemType.gemTypeID, 0]) * attackObj.turnStatBuffs[0]) + 10f) / 3, (IType.ElementType)attackObj.gemType.gemTypeID, 3));
    }
}
