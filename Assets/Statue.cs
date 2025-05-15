using UnityEngine;

public class Statue : MonoBehaviour
{
    [Header("��������� �����")]
    public AudioClip approachSound;       // ���� ��� �����������
    [Range(0, 1)] public float volume = 1f; // ��������� �����
    public float triggerDistance = 3f;   // ��������� ������������

    [Header("����������")]
    public Transform player;             // ������ �� ������
    private AudioSource audioSource;
    private bool hasPlayed = false;      // ���� ���������������

    private void Awake()
    {
        // ������������� AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D ����
        audioSource.volume = volume;

        // ��������� ������ ���� �� ��������
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    private void Update()
    {
        // ���� ���� ��� ��������������� - �������
        if (hasPlayed || player == null || approachSound == null) return;

        // �������� ���������
        if (Vector3.Distance(transform.position, player.position) <= triggerDistance)
        {
            PlaySoundOnce();
        }
    }

    private void PlaySoundOnce()
    {
        audioSource.PlayOneShot(approachSound);
        hasPlayed = true; // ��������� ��������� ���������������

        // �����������: ���������� � �������
        Debug.Log("������: ���� ������������� ���� ���");
    }

    // ������������ ���� ������������
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
}