using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Настройки")]
    public KeyCode collectKey = KeyCode.T;
    public float collectDistance = 2f;
    public GameObject collectEffect;
    public AudioClip collectSound; // Аудиоклип для звука сбора
    [Range(0, 1)] public float collectSoundVolume = 1f; // Громкость звука сбора

    private PrefabSpawner spawner;
    private Transform player;
    private bool isInRange = false;
    private AudioSource audioSource; // Ссылка на AudioSource

    public void Initialize(PrefabSpawner spawner)
    {
        this.spawner = spawner;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Получаем или добавляем AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Настраиваем AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D звук
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
            Debug.Log("Можно собрать (Нажмите T)");
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
        // Проигрываем звук сбора
        if (collectSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(collectSound);
        }

        // Визуальный эффект
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        // Уведомляем системы
        spawner?.RegisterCollect(gameObject);
        QuestManager.Instance?.UpdateQuestProgress();
        FindObjectOfType<QuestBoard>().RegisterItemCollected();

        // Отключаем коллайдер и рендерер, но оставляем объект пока звук играет
        GetComponent<Collider>().enabled = false;
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().enabled = false;
        }

        // Уничтожаем объект после завершения звука
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