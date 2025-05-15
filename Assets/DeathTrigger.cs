using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [Header("���������")]
    public Transform respawnPoint; // ����� �����������
    public float respawnDelay = 1f; // �������� ����� ������������
    public GameObject deathEffect; // ������������ ������ ������

    [Header("���� ������")]
    public AudioClip deathSound; // ���� ��� ������
    [Range(0, 1)] public float deathSoundVolume = 1f; // ��������� ����� ������

    private AudioSource audioSource;

    private void Awake()
    {
        // ������� AudioSource ���� ��� ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ����������� AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D ����
        audioSource.volume = deathSoundVolume;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController player = other.GetComponent<CharacterController>();
            if (player != null)
            {
                // ����������� ���� ������
                if (deathSound != null)
                {
                    audioSource.PlayOneShot(deathSound);
                }

                StartCoroutine(RespawnPlayer(player));
            }
        }
    }

    System.Collections.IEnumerator RespawnPlayer(CharacterController player)
    {
        // 1. ��������� ����������
        player.enabled = false;

        // 2. ������ ������ (���� ����)
        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.transform.position, Quaternion.identity);
        }

        // 3. ���� ��������� �����
        yield return new WaitForSeconds(respawnDelay);

        // 4. ���������� �� ����� �����������
        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
        }
        else
        {
            // ���� ����� �� ��������� - ���������� � ������ �����
            player.transform.position = Vector3.zero;
        }

        // 5. �������� ���������� �������
        player.enabled = true;
    }
}