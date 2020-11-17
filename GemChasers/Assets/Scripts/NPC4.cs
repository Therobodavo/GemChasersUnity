using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC4 : NPC
{
    public override void SetName()
    {
        npcName = "Tom";
    }
    public override void CreateDialogue()
    {
        convos = new List<string>[2];
        convos[0] = new List<string>();
        convos[1] = new List<string>();

        convos[0].Add("Howdy Partner!");
        convos[0].Add("You must be the fellow who washed up on shore.. and you have a gauntlet already!");
        convos[0].Add("Sammy sent you to try out the battlefield here??? Hmm, ok but be warned that these monsters appear in different variants.");
        convos[0].Add("To get a good variety of battles in, why don't you try taking out 6 coconut and 6 mushroom enemies!");
        convos[0].Add("I know that's a lot for your first day on the job, but it'll really help you see what we're up against.");

        convos[1].Add("Nice job! You looked great using that gauntlet out there.");
        convos[1].Add("You've done enough work for your first day on the job, and I'm sure you have a bunch of questions..");
        convos[1].Add("Why don't you go back and talk with George so you can go over everything you learned today :)");
    }
    public override void CreateQuests()
    {
        newQuests = new List<Quest>();
        Quest quest;
        quest = new Quest(this, IType.QuestType.KillQuest);
        quest.AddEnemyGoal(IType.EnemyType.Coconut, 6);
        quest.AddEnemyGoal(IType.EnemyType.Mushroom, 6);
        newQuests.Add(quest);
        quest = new Quest(this, IType.QuestType.TalkQuest);
        quest.SetTalkTarget(allNPC[0]);
        quest.isLastQuest = true;
        newQuests.Add(quest);
    }
}
