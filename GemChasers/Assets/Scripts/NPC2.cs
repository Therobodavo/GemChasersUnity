using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC2 : NPC
{
    public override void SetIndex()
    {
        npcIndex = 1;
    }
    public override void CreateDialogue()
    {
        convos = new List<string>[2];
        convos[0] = new List<string>();
        convos[1] = new List<string>();

        convos[0].Add("George sent you?");
        convos[0].Add("That's great to hear! Hopefully you can help with the monsters around here :)");
        convos[0].Add("Go let George know I'll get you all set up around here.");
    }
    public override void CreateQuests()
    {
        newQuests = new List<Quest>();
        Quest quest = new Quest(npcIndex, IType.QuestType.TalkQuest);
        quest.SetTalkTarget(0);
        quest.isLastQuest = true;
        newQuests.Add(quest);
    }
}
