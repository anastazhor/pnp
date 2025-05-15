using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [Header("Настройки")]
    public Transform respawnPoint; // Точка возрождения
    public float respawnDelay = 1f; // Задержка перед возрождением
    public GameObject deathEffect; // Опциональный эффект смерти

    [Header("Звук смерти")]
    public AudioClip deathSound; // Звук при смерти
    [Range(0, 1)] public float deathSoundVolume = 1f; // Громкость звука смерти

    private AudioSource audioSource;

    private void Awake()
    {
        // Создаем AudioSource если его нет
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Настраиваем AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D звук
        audioSource.volume = deathSoundVolume;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController player = other.GetComponent<CharacterController>();
            if (player != null)
            {
                // Проигрываем звук смерти
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
        // 1. Отключаем управление
        player.enabled = false;

        // 2. Эффект смерти (если есть)
        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.transform.position, Quaternion.identity);
        }

        // 3. Ждем указанное время
        yield return new WaitForSeconds(respawnDelay);

        // 4. Возвращаем на точку возрождения
        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
        }
        else
        {
            // Если точка не назначена - возвращаем в начало сцены
            player.transform.position = Vector3.zero;
        }

        // 5. Включаем управление обратно
        player.enabled = true;
    }
}