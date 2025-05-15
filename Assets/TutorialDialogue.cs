using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image characterImage;
    public Button yesButton;
    public Button noButton;

    [Header("Dialogue Settings")]
    [TextArea(3, 5)] public string welcomeText = "Добро пожаловать в игру, приключенец!\nПервый раз у нас?";
    [TextArea(3, 5)] public string yesResponse = "Отлично! Тогда используй WASD для движения.\nПопробуй!";
    [TextArea(3, 5)] public string noResponse = "Уже бывалый игрок, да? Тогда не буду тянуть.\nЧтобы начать приключение найди 7 кубиков. Для этого подойди к столу и нажми Q.";
    [TextArea(3, 5)] public string jumpInstruction = "Супер, а теперь еще нажми ПРОБЕЛ чтобы прыгнуть!";
    [TextArea(3, 5)] public string completionText = "Классный звук, да? Теперь найди 7 кубиков и начнем игру.\nДля этого подойди к столу и нажми Q.";
    [TextArea(3, 5)] public string questCompleteText = "Уже все собрал? Отлично, подойди ко мне и нажми Y. Мы готовы начать!";

    [Header("Timing Settings")]
    public float textSpeed = 0.05f;
    public float initialDelay = 4f;
    public float instructionDelay = 2f;
    public float questCompleteDelay = 0.05f;
    public float noResponseDuration = 8f; // Время показа noResponse
    public float questCompleteDuration = 8f; // Время показа questCompleteText

    [Header("Quest Reference")]
    public QuestBoard questBoard;

    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private bool waitingForInput = false;
    private bool wasdPressed = false;
    private bool questCompleted = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        Invoke("StartDialogue", initialDelay);

        if (questBoard != null)
        {
            questBoard.OnQuestComplete += ShowQuestCompleteDialogue;
        }
    }

    void OnDestroy()
    {
        if (questBoard != null)
        {
            questBoard.OnQuestComplete -= ShowQuestCompleteDialogue;
        }
    }

    void Update()
    {
        if (waitingForInput && !wasdPressed)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                wasdPressed = true;
                Invoke("ShowJumpInstruction", instructionDelay);
            }
        }

        if (waitingForInput && wasdPressed && Input.GetKeyDown(KeyCode.Space))
        {
            waitingForInput = false;
            ShowCompletion();
        }
    }

    void ShowQuestCompleteDialogue()
    {
        if (questCompleted) return;

        questCompleted = true;
        Invoke("DisplayQuestComplete", 6f);
    }

    void DisplayQuestComplete()
    {
        dialoguePanel.SetActive(true);
        nameText.text = "Система";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        ShowMessage(questCompleteText);
        Invoke("HideDialogue", questCompleteDuration);
    }

    void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        nameText.text = "Мастер";
        ShowMessage(welcomeText, true);
    }

    void ShowMessage(string message, bool showButtons = false)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message, showButtons));
    }

    IEnumerator TypeText(string text, bool showButtons)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;

        if (showButtons)
        {
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
        }
    }

    public void OnYesClicked()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        ShowMessage(yesResponse);
        waitingForInput = true;
        wasdPressed = false;
    }

    public void OnNoClicked()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        ShowMessage(noResponse);
        Invoke("HideDialogue", noResponseDuration);
    }

    void ShowJumpInstruction()
    {
        ShowMessage(jumpInstruction);
    }

    void ShowCompletion()
    {
        ShowMessage(completionText);
        Invoke("HideDialogue", 8f);
    }

    void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}