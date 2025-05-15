using UnityEngine;

public class Statue : MonoBehaviour
{
    [Header("Настройки звука")]
    public AudioClip approachSound;       // Звук при приближении
    [Range(0, 1)] public float volume = 1f; // Громкость звука
    public float triggerDistance = 3f;   // Дистанция срабатывания

    [Header("Компоненты")]
    public Transform player;             // Ссылка на игрока
    private AudioSource audioSource;
    private bool hasPlayed = false;      // Флаг воспроизведения

    private void Awake()
    {
        // Инициализация AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D звук
        audioSource.volume = volume;

        // Автопоиск игрока если не назначен
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    private void Update()
    {
        // Если звук уже воспроизводился - выходим
        if (hasPlayed || player == null || approachSound == null) return;

        // Проверка дистанции
        if (Vector3.Distance(transform.position, player.position) <= triggerDistance)
        {
            PlaySoundOnce();
        }
    }

    private void PlaySoundOnce()
    {
        audioSource.PlayOneShot(approachSound);
        hasPlayed = true; // Запрещаем повторное воспроизведение

        // Опционально: показываем в консоли
        Debug.Log("Статуя: звук воспроизведен один раз");
    }

    // Визуализация зоны срабатывания
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
}