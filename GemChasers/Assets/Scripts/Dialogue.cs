using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    string[] messages = {"Level 1 tutorial info is stated here...",
        "Level 2 tutorial info is stated here...",
        "Level 3 tutorial info is stated here...",
        "Level 4 tutorial info is stated here...",
        "Level 5 tutorial info is stated here...",
        "Level 6 tutorial info is stated here..." };
    int messageIndex = 0;
    int currentCharIndex = 0;

    public enum TypingState { WaitingForMessage, IsTyping, MessageFinished };
    TypingState currentState = TypingState.WaitingForMessage;
    public GameObject dialogueOutput;
    float lastCharTime = 0;
    float delay = .025f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == TypingState.IsTyping && Time.timeSinceLevelLoad - lastCharTime > delay)
        {
            lastCharTime = Time.timeSinceLevelLoad;
            dialogueOutput.GetComponent<Text>().text += messages[messageIndex][currentCharIndex];
            currentCharIndex++;
            if (currentCharIndex >= messages[messageIndex].Length)
            {
                currentState = TypingState.MessageFinished;
            }
        }
        else if (currentState == TypingState.MessageFinished && Input.GetKeyDown(KeyCode.Return))
        {
            currentCharIndex = 0;
            messageIndex++;
            dialogueOutput.GetComponent<Text>().text = "";
            currentState = TypingState.WaitingForMessage;
        }
        else if (currentState == TypingState.WaitingForMessage)
        {
            if (messageIndex >= 0 && messageIndex < messages.Length)
            {
                currentState = TypingState.IsTyping;
            }
        }
    }
}
