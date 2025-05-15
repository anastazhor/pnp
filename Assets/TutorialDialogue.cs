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
    [TextArea(3, 5)] public string welcomeText = "����� ���������� � ����, �����������!\n������ ��� � ���?";
    [TextArea(3, 5)] public string yesResponse = "�������! ����� ��������� WASD ��� ��������.\n��������!";
    [TextArea(3, 5)] public string noResponse = "��� ������� �����, ��? ����� �� ���� ������.\n����� ������ ����������� ����� 7 �������. ��� ����� ������� � ����� � ����� Q.";
    [TextArea(3, 5)] public string jumpInstruction = "�����, � ������ ��� ����� ������ ����� ��������!";
    [TextArea(3, 5)] public string completionText = "�������� ����, ��? ������ ����� 7 ������� � ������ ����.\n��� ����� ������� � ����� � ����� Q.";
    [TextArea(3, 5)] public string questCompleteText = "��� ��� ������? �������, ������� �� ��� � ����� Y. �� ������ ������!";

    [Header("Timing Settings")]
    public float textSpeed = 0.05f;
    public float initialDelay = 4f;
    public float instructionDelay = 2f;
    public float questCompleteDelay = 0.05f;
    public float noResponseDuration = 8f; // ����� ������ noResponse
    public float questCompleteDuration = 8f; // ����� ������ questCompleteText

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
        nameText.text = "�������";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        ShowMessage(questCompleteText);
        Invoke("HideDialogue", questCompleteDuration);
    }

    void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        nameText.text = "������";
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