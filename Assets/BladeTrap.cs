using UnityEngine;

public class BladeTrap : MonoBehaviour
{
    [Header("Настройки ловушки")]
    public GameObject bladePrefab;
    public float activationDelay = 0.5f;
    public float bladeDuration = 2f;
    public Vector3 bladeSpawnOffset = new Vector3(0, 0.1f, 0);

    [Header("Настройки смерти")]
    public Transform respawnPoint;
    public float respawnDelay = 1f;

    [Header("Звуковые эффекты")]
    public AudioClip trapActivationSound; // Звук активации ловушки
    public AudioClip playerDeathSound;   // Звук смерти игрока
    [Range(0, 1)] public float soundVolume = 1f;

    private bool isActivated = false;
    private GameObject currentBlade;
    private AudioSource audioSource;

    private void Awake()
    {
        // Инициализация AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.volume = soundVolume;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            PlaySound(trapActivationSound); // Звук активации ловушки
            Invoke("ActivateBlades", activationDelay);
        }
    }

    void ActivateBlades()
    {
        // Создаем лезвия
        currentBlade = Instantiate(bladePrefab,
            transform.position + bladeSpawnOffset,
            Quaternion.identity);

        // Настраиваем лезвия
        BladeController bladeController = currentBlade.AddComponent<BladeController>();
        bladeController.Initialize(respawnPoint, respawnDelay);
        bladeController.SetDeathSound(playerDeathSound, soundVolume); // Передаем звук смерти

        Destroy(currentBlade, bladeDuration);
        Invoke("ResetTrap", bladeDuration + 1f);
    }

    void ResetTrap()
    {
        isActivated = false;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}