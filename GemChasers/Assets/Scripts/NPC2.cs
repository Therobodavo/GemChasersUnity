using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * NPC2 Class
 * Programmed by David Knolls
 * 
 * New NPC
 */

public class NPC2 : NPC
{
    public override void SetName()
    {
        npcName = "Carl";
    }
    public override void CreateDialogue()
    {
        convos = new List<string>[2];
        convos[0] = new List<string>();
        convos[1] = new List<string>();

        convos[0].Add("George sent you?");
        convos[0].Add("That's great to hear! My name is Carl, hopefully you can help with the monsters around here...");
        convos[0].Add("Why don't you start by taking out 6 coconut enemies nearby!");
        convos[0].Add("After that I'll have you check out more of the island so you can see the different monsters we fight.");

        convos[1].Add("Nice job taking out those coconut monsters!");
        convos[1].Add("These were only heat type monsters, but throughout the island you'll find different variants of each monster.");
        convos[1].Add("Now it's time for you to see for yourself! why don't you go see Sammy and see what you can do to help out there.");
    }
    public override void CreateQuests()
    {
        newQuests = new List<Quest>();
        Quest quest;
        quest = new Quest(this, IType.QuestType.KillQuest);
        quest.AddEnemyGoal(IType.EnemyType.Coconut, 6);
        quest.completionObject = GameObject.Find("DOOR3");
        newQuests.Add(quest);
        quest = new Quest(this, IType.QuestType.TalkQuest);
        quest.SetTalkTarget(allNPC[2]);
        newQuests.Add(quest);
    }
}
