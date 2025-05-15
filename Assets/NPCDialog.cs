using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class NPCDialog : MonoBehaviour
{
    [Header("���������")]
    public KeyCode interactKey = KeyCode.Y;
    public float interactDistance = 3f;

    [Header("UI Elements")]
    public GameObject npcPanel;
    public TextMeshProUGUI npcText;
    public Button confirmButton;

    [Header("�������")]
    public string targetSceneName = "second_scene";

    private Transform player;
    private bool isPlayerNear;
    private bool isDialogActive;
    private CameraController cameraController;

    [SerializeField] private bool isQuestComplete; // ����������� ��� ������

    void Start()
    {
        npcPanel.SetActive(false);
        isQuestComplete = false; // ����� �������������
    }

    // ���������� �� QuestBoard ��� ���������� ������
    public void UnlockDialog()
    {
        isQuestComplete = true;
        Debug.Log("������ � NPC �������������!");
    }

    void Update()
    {
        if (!isQuestComplete) return; // ������ ��������

        if (isPlayerNear && Input.GetKeyDown(interactKey))
        {
            ToggleDialog();
        }

        if (isDialogActive && Input.GetKeyDown(KeyCode.Return))
        {
            TeleportToScene();
        }
    }

    void ToggleDialog()
    {
        if (!isDialogActive)
        {
            ShowDialog();
        }
        else
        {
            HideDialog();
        }
    }

    void ShowDialog()
    {
        isDialogActive = true;
        npcText.text = "������ �� ������ ��� �����. ����� � �����������?";
        npcPanel.SetActive(true);

        if (player != null)
        {
            cameraController = player.GetComponentInChildren<CameraController>();
            if (cameraController != null) cameraController.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideDialog()
    {
        isDialogActive = false;
        npcPanel.SetActive(false);

        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void TeleportToScene()
    {
        HideDialog();
        SceneManager.LoadScene(targetSceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = other.transform;
            Debug.Log("����� � ���� NPC. ����� ��������: " + isQuestComplete);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            HideDialog();
        }
    }
}