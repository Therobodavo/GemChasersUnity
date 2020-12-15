using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PlayerAttack Class
 * Programmed by David Knolls
 * 
 * Creates attack based off 2 buffs and a gem
 */
public class PlayerAttack
{
    public Buff[] buffs;
    public Gem gemType;

    private int energyCost;
    public PlayerManager playerScript;
    private IType.MoveType currentType;

    //If all enemies or allies should be targeted
    public bool targetAllSide = false;
    public bool isComboAttack = false;
 
    //Multiply Attack
    //Multiply Defense
    //Add Speed
    //Add Health
    //Add Energy
    public float[] turnStatBuffs = { 1, 1, 1,0,0 };

    bool foundMatch = false;
    public PlayerAttack(PlayerManager player, Buff buff1 = null, Buff buff2 = null, Gem gem = null) 
    {
        playerScript = player;
        buffs = new Buff[2];
        buffs[0] = buff1;
        buffs[1] = buff2;
        buff1.InitAttack(this);
        buff2.InitAttack(this);
        gemType = gem;

        if (buff1 != null && buff2 != null) 
        {
            //Calculatee energy cost
            int sum = buff1.GetEnergyCost() + buff2.GetEnergyCost();
            if (sum > 100)
            {
                energyCost = 100;
            }
            else
            {
                energyCost = sum;
            }
        }

        //Find type based off buffs
        if (CheckBuffCombination(buffs[0].instantAttackCombinations, IType.MoveType.InstantAttack) ||
            CheckBuffCombination(buffs[0].instantPassiveCombinations, IType.MoveType.InstantPassive) ||
            CheckBuffCombination(buffs[0].delayedAttackCombinations, IType.MoveType.DelayedAttack) ||
            CheckBuffCombination(buffs[0].delayedPassiveCombinations, IType.MoveType.DelayedPassive)) 
        {
            foundMatch = true;
        }
    }
    public int GetEnergyCost() 
    {
        return energyCost;
    }
    //Checks if the move targets itself (like heal/relax)
    public bool isTargetingSelf()
    {
        bool result = true;

        for (int i = 0; i < buffs.Length; i++)
        {
            IType.BuffType type = (IType.BuffType)buffs[i].GetBuffID();
            if (type == IType.BuffType.Combo || type == IType.BuffType.Split || type == IType.BuffType.Strength)
            {
                result = false;
                break;
            }
        }
        return result;
    }

    //Main function to trigger effects of move
    public void Use() 
    {
        //If move is valid
        if (buffs[0] != null && buffs[1] != null && gemType != null && foundMatch) 
        {
            playerScript.currentType = (IType.ElementType)gemType.gemTypeID;
            playerScript.TakeEnergy(energyCost);

            //Use attack based on type
            if (currentType == IType.MoveType.InstantAttack) 
            {
                buffs[0].OnInstantAttack(this);
                buffs[1].OnInstantAttack(this);

                IBattle[] enemies = playerScript.currentBattleArea.GetEnemyScripts();

                if (targetAllSide)
                {
                    //attack all that are alive
                    List<int> indexAlive = new List<int>();
                    for (int i = 0; i < enemies.Length; i++) 
                    {
                        if (enemies[i]) 
                        {
                            if (enemies[i].isAlive()) 
                            {
                                indexAlive.Add(i);
                            }
                        }
                    }

                    for (int i = 0; i < indexAlive.Count; i++) 
                    {
                        enemies[indexAlive[i]].TakeDamage(playerScript,((playerScript.baseStats[0] * playerScript.statModifiers[gemType.gemTypeID,0]) * turnStatBuffs[0]) / indexAlive.Count,isComboAttack);
                    }
                }
                else 
                {
                    if (enemies[playerScript.UIScript.selectedEnemyTarget]) 
                    {
                        //Debug.Log("Base Stat: " + playerScript.baseStats[0] + " - Gem Stat: " + gemType.statModifiers[gemType.gemTypeID, 0] + " - Move Stat: " + turnStatBuffs[0]);
                        //attack one
                        enemies[playerScript.UIScript.selectedEnemyTarget].TakeDamage(playerScript,(playerScript.baseStats[0] * playerScript.statModifiers[gemType.gemTypeID, 0]) * turnStatBuffs[0], isComboAttack);
                    }
                    
                }
            }
            else if (currentType == IType.MoveType.InstantPassive)
            {
                buffs[0].OnInstantPassive(this);
                buffs[1].OnInstantPassive(this);

                //Check if targeting player or all (later when we add summons)
                playerScript.Heal(turnStatBuffs[3]);
                playerScript.Relax(turnStatBuffs[4]);
            }
            else if (currentType == IType.MoveType.DelayedAttack)
            {
                buffs[0].OnDelayedAttack(this);
                buffs[1].OnDelayedAttack(this);
            }
            else if (currentType == IType.MoveType.DelayedPassive)
            {
                buffs[0].OnDelayedPassive(this);
                buffs[1].OnDelayedPassive(this);
            }
        }
    }

    //Check if the second buff is compatiable with the first buff and set the type
    private bool CheckBuffCombination(IType.BuffType[] list, IType.MoveType outcome) 
    {
        bool canFind = false;
        if (buffs[0] != null && buffs[1] != null && list != null)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == (IType.BuffType)buffs[1].GetBuffID())
                {
                    currentType = outcome;
                    canFind = true;
                    break;
                }
            }
        }
        return canFind;
    }
}
