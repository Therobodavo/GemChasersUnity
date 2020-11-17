﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public IType.QuestType type;
    int[] numToKill = new int[10];
    public List<IType.EnemyType> enemyTargets;
    public int[] numKilled = new int[10];
    public NPC questTalkTarget = null;
    public NPC npcGiver = null;
    public bool hasTalkedToNPC = false;
    public bool isLastQuest = false;
    public GameObject completionObject;
    public Quest(NPC giver, IType.QuestType questType) 
    {
        this.type = questType;
        npcGiver = giver;
        enemyTargets = new List<IType.EnemyType>();
    }
    public void AddEnemyGoal(IType.EnemyType enemyType, int num) 
    {      
        if (num > 0) 
        {
            enemyTargets.Add(enemyType);
            numToKill[(int)enemyType] += num;
        }
    }
    public void SetTalkTarget(NPC npcTarget) 
    {
        questTalkTarget = npcTarget;
    }
    public string GetString() 
    {
        string output = "";
        if (type == IType.QuestType.KillQuest)
        {
            if (!IsComplete())
            {
                for (int i = 0; i < enemyTargets.Count; i++)
                {
                    int index = (int)enemyTargets[i];
                    output += numKilled[index] + " / " + numToKill[index] + " " + enemyTargets[i].ToString() + "\n";
                }
            }
            else
            {
                output += "Talk to NPC " + npcGiver;
            }
        }
        else if (type == IType.QuestType.TalkQuest) 
        {
            output += "Talk to NPC " + questTalkTarget.npcName;
        }
        return output;
    }
    public bool IsComplete() 
    {
        bool isDone = true;
        if (type == IType.QuestType.KillQuest)
        {
            for (int i = 0; i < enemyTargets.Count; i++)
            {
                int index = (int)enemyTargets[i];
                if (numToKill[index] > numKilled[index])
                {
                    isDone = false;
                    break;
                }
            }
        }
        else if (type == IType.QuestType.TalkQuest) 
        {
            if (!hasTalkedToNPC) 
            {
                isDone = false;
            }
        }
        return isDone;
    }
}