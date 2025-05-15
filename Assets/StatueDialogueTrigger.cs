using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class StatueDialogueTrigger : MonoBehaviour
{
    [Header("�������")]
    [TextArea(3, 5)] public string masterText1 = "���, ����� ����... ����, ��� �� ������ ������������?";
    public float masterText1Duration = 3f;
    [TextArea(3, 5)] public string masterText2 = "��� �� � ���� ��� �����, ������...";
    public float masterText2Duration = 3f;
    [TextArea(3, 5)] public string npcText1 = "...���...�������?!";
    public float npcText1Duration = 3f;

    [Header("UI Elements")]
    public GameObject statueDialoguePanel;
    public Image statueBackgroundPanel;
    public TMP_Text statueNameText;
    public TMP_Text statueDialogueText;
    public Image statueNpcImage;

    [Header("Final Screen")]
    public GameObject finalPanel;
    public TMP_Text finalText;
    public Image fadeImage;
    public float fadeDuration = 2f;
    public string finalMessage = "����������� �������";

    [Header("Settings")]
    public float textSpeed = 0.05f;
    public Color npcPanelColor = new Color(0.259f, 0.353f, 0.843f, 0.39f);

    private Coroutine currentDialogue;

    void Start()
    {
        statueDialoguePanel.SetActive(false);
        finalPanel.SetActive(false);
        fadeImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !IsDialogueActive())
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (currentDialogue != null)
            StopCoroutine(currentDialogue);

        currentDialogue = StartCoroutine(RunDialogue());
    }

    IEnumerator RunDialogue()
    {
        // �������� ������
        statueDialoguePanel.SetActive(true);
        statueNpcImage.gameObject.SetActive(true);
        statueNameText.text = "��� ������";
        statueBackgroundPanel.color = npcPanelColor;

        yield return StartCoroutine(TypeText(masterText1, statueDialogueText));
        yield return new WaitForSeconds(masterText1Duration);

        yield return StartCoroutine(TypeText(masterText2, statueDialogueText));
        yield return new WaitForSeconds(masterText2Duration);

        yield return StartCoroutine(TypeText(npcText1, statueDialogueText));
        yield return new WaitForSeconds(npcText1Duration);

        statueDialoguePanel.SetActive(false);

        // ��������� �����
        finalPanel.SetActive(true);
        fadeImage.gameObject.SetActive(true);
        finalText.text = finalMessage;

        float elapsed = 0f;
        Color color = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeImage.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ��������, ��� ��������� ������
        color.a = 1f;
        fadeImage.color = color;

        // ����� ����� ������� (�� �������, �������� 1 �������)
        yield return new WaitForSeconds(1f);

        // ����� �� ����
        Application.Quit();

#if UNITY_EDITOR
        // ���� �� ���������� � ���������
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    IEnumerator TypeText(string text, TMP_Text target)
    {
        target.text = "";
        foreach (char letter in text.ToCharArray())
        {
            target.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public bool IsDialogueActive()
    {
        return statueDialoguePanel.activeInHierarchy || finalPanel.activeInHierarchy;
    }
}