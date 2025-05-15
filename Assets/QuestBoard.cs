using UnityEngine;
using System;

public class QuestBoard : MonoBehaviour
{
    [Header("Настройки")]
    public KeyCode interactionKey = KeyCode.Q;
    public float interactionDistance = 2f;
    public PrefabSpawner prefabSpawner;
    public GameObject questUI;
    public NPCDialog npcDialog;
    public ParticleSystem interactionParticles;
    public MusicManager musicManager;

    public event Action OnQuestComplete;

    private bool isPlayerNear;
    private bool questStarted;
    private int collectedItems;
    private const int requiredItems = 7;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(interactionKey) && !questStarted)
        {
            StartQuest();
        }
    }

    public void StartQuest()
    {
        questStarted = true;
        prefabSpawner.StartSpawning();

        if (interactionParticles != null)
        {
            interactionParticles.Play();
        }

        if (questUI != null)
        {
            questUI.SetActive(true);
        }

        if (musicManager != null)
        {
            musicManager.StartQuestMusic();
        }
    }

    public void RegisterItemCollected()
    {
        collectedItems++;
        if (collectedItems >= requiredItems)
        {
            CompleteQuest();
        }
    }

    void CompleteQuest()
    {
        if (npcDialog != null)
        {
            npcDialog.UnlockDialog();
        }

        if (musicManager != null)
        {
            musicManager.StopQuestMusic();
        }

        OnQuestComplete?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}