using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       //UI�� ��Ʈ�� �� ���̶� �߰�
using System;               //Arrary ���� ����� ����ϱ� ���� �߰�

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private SpeakerUI[] speakers;
    [SerializeField]
    private DialogueDate[] dialogues;
    [SerializeField]
    private bool DialogueInit = true;
    [SerializeField]
    private bool dialogueDB = false;

    public int currentDialogueIndex = -1;
    public int currentSpeakerIndex = 0;
    public float typingSpeed = 0.1f;
    private bool isTypingEffect = false;


    private void Awake()
    {
        SetAllClose();
    }

    public bool UpdateDialogue(int currentIndex, bool InitType)
    {
        if (DialogueInit == true && InitType == true)
        {
            SetAllClose();
            SetNextDialogue(currentIndex);
            DialogueInit = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTypingEffect == true)
            {
                isTypingEffect = false;
                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text = dialogues[currentDialogueIndex].dialogue;
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);
                return false;
            }

            if (dialogues[currentDialogueIndex].nextindex != -100)
            {
                SetNextDialogue(dialogues[currentDialogueIndex].nextindex);
            }

            else
            {
                SetAllClose();
                DialogueInit = true;
                return true;
            }

        }

        return false;
    }

    private void SetActiveObjects(SpeakerUI speaker, bool visible)
    {
        speaker.imageDialogue.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);

        speaker.objectArrow.SetActive(false);

        Color color = speaker.imgCharacter.color;
        if (visible)
        {
            color.a = 1;
        }

        else
        {
            color.a = 0.2f;
        }
        speaker.imgCharacter.color = color;
    }

    private void SetAllClose()
    {
        for (int i = 0; i < speakers.Length; i++)
        {
            SetActiveObjects(speakers[i], false);
        }
    }

    private void SetNextDialogue(int currentIndex)
    {
        SetAllClose();
        currentDialogueIndex = currentIndex;
        currentSpeakerIndex = dialogues[currentDialogueIndex].speakerUIindex;
        SetActiveObjects(speakers[currentSpeakerIndex], true);

        speakers[currentSpeakerIndex].textName.text = dialogues[currentDialogueIndex].name;
        StartCoroutine("OnTypingText");
    }

    private IEnumerator OnTypingText()
    {
        int index = 0; ;
        isTypingEffect = true;

        if (dialogues[currentDialogueIndex].characterPath != "None")
        {
            speakers[currentSpeakerIndex].imgCharacter = (Image)Resources.Load(dialogues[currentDialogueIndex].characterPath);
        }

        while (index < dialogues[currentDialogueIndex].dialogue.Length + 1)
        {
            speakers[currentSpeakerIndex].textDialogue.text = dialogues[currentDialogueIndex].dialogue.Substring(0, index);
            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        speakers[currentSpeakerIndex].objectArrow.SetActive(true);
    }
}

[Serializable]
public struct SpeakerUI
{
    public Image imgCharacter;
    public Image imageDialogue;
    public Text textName;
    public Text textDialogue;
    public GameObject objectArrow;
}

[Serializable]
public struct DialogueDate
{
    public int index;
    public int speakerUIindex;
    public string name;
    public string dialogue;
    public string characterPath;
    public int tweenType;
    public int nextindex;
}
