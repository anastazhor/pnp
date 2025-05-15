using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SecondDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Image backgroundPanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image masterImage;
    public Image npcImage;

    [Header("Диалоги")]
    [TextArea(3, 5)] public string masterText1 = "Ты просыпаешься в тусклой комнате...";
    public float masterText1Duration = 3f;
    [TextArea(3, 5)] public string masterText2 = "Кто это там идёт?...";
    public float masterText2Duration = 3f;
    [TextArea(3, 5)] public string npcText1 = "Странник! Ты тоже выбрался из решетки!";
    public float npcText1Duration = 3f;
    [TextArea(3, 5)] public string npcText2 = "Как я рада, что не одна здесь...";
    public float npcText2Duration = 3f;
    [TextArea(3, 5)] public string npcText3 = "...ты же знаешь как найти выход?";
    public float npcText3Duration = 5f;

    [Header("Настройки")]
    public float textSpeed = 0.05f;
    private Color masterPanelColor = new Color(0.77f, 0.63f, 0.89f, 0.39f);
    private Color npcPanelColor = new Color(0.259f, 0.353f, 0.843f, 0.39f);

    private Coroutine currentDialogue;

    void Start()
    {
        InitializeDialogue();
        Invoke("StartDialogue", 1f);
    }

    void InitializeDialogue()
    {
        dialoguePanel.SetActive(false);
        masterImage.gameObject.SetActive(false);
        npcImage.gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        if (currentDialogue != null)
            StopCoroutine(currentDialogue);

        currentDialogue = StartCoroutine(RunDialogueSequence());
    }

    IEnumerator RunDialogueSequence()
    {
        dialoguePanel.SetActive(true);

        // Реплики Мастера
        masterImage.gameObject.SetActive(true);
        nameText.text = "Мастер";
        backgroundPanel.color = masterPanelColor;

        yield return StartCoroutine(TypeText(masterText1));
        yield return new WaitForSeconds(masterText1Duration);

        yield return StartCoroutine(TypeText(masterText2));
        yield return new WaitForSeconds(masterText2Duration);

        // Реплики NPC
        masterImage.gameObject.SetActive(false);
        npcImage.gameObject.SetActive(true);
        nameText.text = "Энн Питнам";
        backgroundPanel.color = npcPanelColor;

        yield return StartCoroutine(TypeText(npcText1));
        yield return new WaitForSeconds(npcText1Duration);

        yield return StartCoroutine(TypeText(npcText2));
        yield return new WaitForSeconds(npcText2Duration);

        yield return StartCoroutine(TypeText(npcText3));
        yield return new WaitForSeconds(npcText3Duration);

        dialoguePanel.SetActive(false);
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public bool IsDialogueInProgress()
    {
        return dialoguePanel.activeInHierarchy;
    }
}