using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialogueObject;
    public Text dialogueText;
    public PlayerManager player;
    public GameObject PressEnterUI;
    public enum TypingState { WaitingForMessage, IsTyping, MessageFinished };
    TypingState currentState = TypingState.WaitingForMessage;
    float lastCharTime = 0;
    float delay = .015f;

    public int conversationIndex = 0;
    public int messageIndex = 0;
    private int currentCharIndex = 0;
    public bool startedTalking = false;
    public bool activeQuest = false;
    public string npcName = "NPC";
    public List<string>[] convos;

    public bool goAway = false;
    public List<Quest> newQuests;
    public NPC[] allNPC;
    public LevelManager lm;
    public virtual void Awake()
    {
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        allNPC = lm.npcs;
        dialogueObject = GameObject.Find("Dialogue");
        dialogueText = GameObject.Find("DialogueText").GetComponent<Text>();
        PressEnterUI = GameObject.Find("PressEnter");
        CreateDialogue();
        SetName();
        CreateQuests();
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }
    public virtual void SetName() 
    {
        npcName = "George";
    }
    public virtual void CreateDialogue() 
    {
        convos = new List<string>[2];
        convos[0] = new List<string>();
        convos[1] = new List<string>();

        convos[0].Add("Oh I see you've finally woken up...");
        convos[0].Add("My name is George and I found you washed on shore yesterday...");
        convos[0].Add("Here on this island we're at war against strange monsters that are evolved forms of various objects.");
        convos[0].Add("To fight them off we use these special gauntlets that utilize crystals known as gems and buffs to create special attacks.");
        convos[0].Add("Right now there's a dormant monster nearby, and we really need all the help we can get.");
        convos[0].Add("Take this gauntlet and some crystals and go take out the monster!");

        convos[1].Add("Nice job! Now that the monster is gone I can tell you about the rest of this island and what's needed...");
        convos[1].Add("Around the island you'll find different kinds of monsters. Monsters can appear as different elements, and each monster has it's own battle style.");
        convos[1].Add("I want you to go meet up with my friend Carl. He'll help get you some more experience fighting!");
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude < 2 && Input.GetKeyDown(KeyCode.E)) 
        {
            if (!startedTalking) 
            {
                if (player.currentQuest != null)
                {
                    if (player.currentQuest.type == IType.QuestType.TalkQuest && player.currentQuest.questTalkTarget == this && !player.currentQuest.hasTalkedToNPC)
                    {
                        player.currentQuest.hasTalkedToNPC = true;
                    }

                    bool questGiver = false;
                    bool questcomplete = player.currentQuest.IsComplete();
                    if (player.currentQuest.npcGiver == this)
                    {
                        questGiver = true;
                    }
                    
                    if ((player.currentQuest.type == IType.QuestType.TalkQuest && questcomplete) ||
                        (player.currentQuest.type == IType.QuestType.KillQuest && questGiver && questcomplete))
                    {
                        if (player.currentQuest.isLastQuest)
                        {
                            SayYouWin();

                        }
                        else 
                        {
                            //turn in quest, go to next dialogue
                            TurnInQuest();
                            TalkTo();
                        }
                    }
                    else 
                    {
                        SayGoAway();
                    }
                   
                }
                else 
                {
                    TalkTo();
                }
            }
        }
        if (startedTalking)
        {
            if (currentState == TypingState.IsTyping && Time.timeSinceLevelLoad - lastCharTime > delay)
            {
                PressEnterUI.SetActive(false);
                lastCharTime = Time.timeSinceLevelLoad;
                dialogueText.text += convos[conversationIndex][messageIndex][currentCharIndex];
                currentCharIndex++;
                if (currentCharIndex >= convos[conversationIndex][messageIndex].Length)
                {
                    currentState = TypingState.MessageFinished;
                }
            }
            else if (currentState == TypingState.MessageFinished) 
            {
                PressEnterUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    currentCharIndex = 0;
                    messageIndex++;
                    if (messageIndex >= convos[conversationIndex].Count)
                    {
                        if (newQuests.Count > 0)
                        {
                            player.GetComponent<PlayerManager>().currentQuest = newQuests[0];
                            newQuests.RemoveAt(0);
                        }
                        EndDialogue();
                    }
                    dialogueText.text = "";
                    currentState = TypingState.WaitingForMessage;
                }
            }

            else if (currentState == TypingState.WaitingForMessage)
            {
                if (conversationIndex >= 0 && conversationIndex < convos.Length) 
                {
                    if (messageIndex >= 0 && messageIndex < convos[conversationIndex].Count)
                    {
                        currentState = TypingState.IsTyping;
                    }
                }
            }
        }
        else 
        {
            if (goAway && Input.GetKeyDown(KeyCode.Return))
            {
                if (player.currentQuest != null)
                {
                    bool questGiver = false;
                    bool questcomplete = player.currentQuest.IsComplete();
                    if (player.currentQuest.npcGiver == this)
                    {
                        questGiver = true;
                    }
                    if (player.currentQuest.isLastQuest && (player.currentQuest.type == IType.QuestType.TalkQuest && questcomplete) ||
                        (player.currentQuest.type == IType.QuestType.KillQuest && questGiver && questcomplete))
                    {
                        SceneManager.LoadScene("Menu");
                    }
                }
                EndTalkingEarly();
            }
        }

        if (player.inBattle) 
        {
            EndTalkingEarly();
        }
    }
    public void TurnInQuest() 
    {
        if (player.currentQuest.completionObject) 
        {
            Destroy(player.currentQuest.completionObject);
        }
        activeQuest = false;
        player.currentQuest = null;
    }
    public void TalkTo() 
    {
        dialogueObject.SetActive(true);
        startedTalking = true;
        goAway = false;
        player.talkingToNPC = true;
    }
    public void EndDialogue() 
    {
        dialogueObject.SetActive(false);
        startedTalking = false;
        currentCharIndex = 0;
        dialogueText.text = "";
        lastCharTime = 0;
        messageIndex = 0;
        conversationIndex++;
        goAway = false;
        player.talkingToNPC = false;
        PressEnterUI.SetActive(false);
    }
    public void EndTalkingEarly() 
    {
        dialogueObject.SetActive(false);
        startedTalking = false;
        currentCharIndex = 0;
        dialogueText.text = "";
        lastCharTime = 0;
        messageIndex = 0;
        goAway = false;
        player.talkingToNPC = false;
        PressEnterUI.SetActive(false);
    }
    public virtual void CreateQuests() 
    {
        newQuests = new List<Quest>();
        Quest quest = new Quest(this,IType.QuestType.KillQuest);
        quest.AddEnemyGoal(IType.EnemyType.Coconut, 1);
        quest.completionObject = GameObject.Find("DOOR2");
        newQuests.Add(quest);
        quest = new Quest(this, IType.QuestType.TalkQuest);
        quest.SetTalkTarget(allNPC[1]);
        newQuests.Add(quest);
    }
    public virtual void SayGoAway() 
    {
        player.talkingToNPC = true;
        if (dialogueText.text != "Go finish your active quest then maybe we can talk...")
        {
            dialogueText.text = "Go finish your active quest then maybe we can talk...";
        }
        if (!dialogueObject.activeSelf)
        {
            dialogueObject.SetActive(true);
        }
        goAway = true;
        PressEnterUI.SetActive(true);
    }
    public virtual void SayYouWin() 
    {
        player.talkingToNPC = true;
        if (dialogueText.text != "Congrats! You helped us save the island!")
        {
            dialogueText.text = "Congrats! You helped us save the island!";
        }
        if (!dialogueObject.activeSelf)
        {
            dialogueObject.SetActive(true);
        }
        goAway = true;
        PressEnterUI.SetActive(true);
    }
}
