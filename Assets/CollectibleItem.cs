using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("���������")]
    public KeyCode collectKey = KeyCode.T;
    public float collectDistance = 2f;
    public GameObject collectEffect;
    public AudioClip collectSound; // ��������� ��� ����� �����
    [Range(0, 1)] public float collectSoundVolume = 1f; // ��������� ����� �����

    private PrefabSpawner spawner;
    private Transform player;
    private bool isInRange = false;
    private AudioSource audioSource; // ������ �� AudioSource

    public void Initialize(PrefabSpawner spawner)
    {
        this.spawner = spawner;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // �������� ��� ��������� AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ����������� AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D ����
        audioSource.volume = collectSoundVolume;
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(collectKey))
        {
            Collect();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("����� ������� (������� T)");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    void Collect()
    {
        // ����������� ���� �����
        if (collectSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(collectSound);
        }

        // ���������� ������
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        // ���������� �������
        spawner?.RegisterCollect(gameObject);
        QuestManager.Instance?.UpdateQuestProgress();
        FindObjectOfType<QuestBoard>().RegisterItemCollected();

        // ��������� ��������� � ��������, �� ��������� ������ ���� ���� ������
        GetComponent<Collider>().enabled = false;
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().enabled = false;
        }

        // ���������� ������ ����� ���������� �����
        if (collectSound != null)
        {
            Destroy(gameObject, collectSound.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectDistance);
    }
}