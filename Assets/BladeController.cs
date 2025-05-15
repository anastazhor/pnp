using UnityEngine;

public class BladeController : MonoBehaviour
{
    private Transform respawnPoint;
    private float respawnDelay;
    private AudioClip deathSound;
    private float soundVolume;

    public void Initialize(Transform respawn, float delay)
    {
        respawnPoint = respawn;
        respawnDelay = delay;
    }

    public void SetDeathSound(AudioClip sound, float volume)
    {
        deathSound = sound;
        soundVolume = volume;
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
                    AudioSource.PlayClipAtPoint(deathSound, transform.position, soundVolume);
                }

                StartCoroutine(RespawnPlayer(player));
            }
        }
    }

    System.Collections.IEnumerator RespawnPlayer(CharacterController player)
    {
        player.enabled = false;
        yield return new WaitForSeconds(respawnDelay);

        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
        }
        else
        {
            player.transform.position = Vector3.zero;
        }

        player.enabled = true;
    }
}