using UnityEngine;

public class BladeTrap : MonoBehaviour
{
    [Header("��������� �������")]
    public GameObject bladePrefab;
    public float activationDelay = 0.5f;
    public float bladeDuration = 2f;
    public Vector3 bladeSpawnOffset = new Vector3(0, 0.1f, 0);

    [Header("��������� ������")]
    public Transform respawnPoint;
    public float respawnDelay = 1f;

    [Header("�������� �������")]
    public AudioClip trapActivationSound; // ���� ��������� �������
    public AudioClip playerDeathSound;   // ���� ������ ������
    [Range(0, 1)] public float soundVolume = 1f;

    private bool isActivated = false;
    private GameObject currentBlade;
    private AudioSource audioSource;

    private void Awake()
    {
        // ������������� AudioSource
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
            PlaySound(trapActivationSound); // ���� ��������� �������
            Invoke("ActivateBlades", activationDelay);
        }
    }

    void ActivateBlades()
    {
        // ������� ������
        currentBlade = Instantiate(bladePrefab,
            transform.position + bladeSpawnOffset,
            Quaternion.identity);

        // ����������� ������
        BladeController bladeController = currentBlade.AddComponent<BladeController>();
        bladeController.Initialize(respawnPoint, respawnDelay);
        bladeController.SetDeathSound(playerDeathSound, soundVolume); // �������� ���� ������

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