using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC3 : NPC
{
    public override void SetName()
    {
        npcName = "Sammy";
    }
    public override void CreateDialogue()
    {
        convos = new List<string>[2];
        convos[0] = new List<string>();
        convos[1] = new List<string>();

        convos[0].Add("Why hello there, I don't think we've met yet! My name is Sammy and I stand guard around this area.");
        convos[0].Add("Around here you'll find mushroom monsters all the time.");
        convos[0].Add("These mushroom monsters are always of the forest type, but they can change in other areas.");
        convos[0].Add("I won't work you too hard right away, so how about you take out 3 mushroom monsters then report back!");

        convos[1].Add("You took out those mushrooms swiftly!");
        convos[1].Add("I think it's about time you go check out the main area.. Why don't you go see Tom and see where the real battle is!");
    }
    public override void CreateQuests()
    {
        newQuests = new List<Quest>();
        Quest quest;
        quest = new Quest(this, IType.QuestType.KillQuest);
        quest.AddEnemyGoal(IType.EnemyType.Mushroom, 3);
        quest.completionObject = GameObject.Find("DOOR4");
        newQuests.Add(quest);
        quest = new Quest(this, IType.QuestType.TalkQuest);
        quest.SetTalkTarget(allNPC[3]);
        newQuests.Add(quest);
    }
}
