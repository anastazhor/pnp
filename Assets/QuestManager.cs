using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI questText;
    public GameObject completionPanel;
    public TextMeshProUGUI completionText;
    public float completionDisplayTime = 10f;

    private int collectedItems = 0;
    private const int totalItems = 7; // ���������, ��� ��� � ��� ������������� ����������
    private Coroutine hidePanelCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������ ����� �� ��������� ����������, ����������� ������� �� 1
    public void UpdateQuestProgress()
    {
        collectedItems++;
        UpdateUI();

        if (collectedItems >= totalItems)
        {
            CompleteQuest();
        }
    }

    public void CompleteQuest()
    {
        UpdateUI();
        ShowCompletionPanel();

        if (hidePanelCoroutine != null)
        {
            StopCoroutine(hidePanelCoroutine);
        }
        hidePanelCoroutine = StartCoroutine(HidePanelAfterDelay());
    }

    void ShowCompletionPanel()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }
    }

    IEnumerator HidePanelAfterDelay()
    {
        yield return new WaitForSeconds(completionDisplayTime);
        HideCompletionPanel();
    }

    void HideCompletionPanel()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (questText != null)
        {
            questText.text = $"�������� ��������: {collectedItems}/{totalItems}";
        }
    }

    // ����� ��� ������ ��������� ��� ������ ������ �������
    public void ResetProgress()
    {
        collectedItems = 0;
        UpdateUI();
        HideCompletionPanel();
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
    }
}