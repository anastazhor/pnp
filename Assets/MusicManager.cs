using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("����������� �����")]
    public AudioSource backgroundMusicSource;
    public AudioSource questMusicSource;

    [Header("��������� ��������")]
    public float fadeDuration = 1f;

    private bool isTransitioning;
    private float backgroundMusicTime;

    private void Start()
    {
        InitializeMusic();
    }

    void InitializeMusic()
    {
        // �������������� ��������� ��������� ������
        if (questMusicSource != null)
        {
            questMusicSource.Stop();
            questMusicSource.volume = 0f;
        }

        // �������� ������� ������
        backgroundMusicSource.volume = 1f;
        backgroundMusicSource.Play();
        backgroundMusicTime = backgroundMusicSource.time;
    }

    public void StartQuestMusic()
    {
        if (isTransitioning || questMusicSource == null) return;

        // ���� ��������� ��������� ������ ����� ���������
        questMusicSource.Play();
        StartCoroutine(TransitionMusic(backgroundMusicSource, questMusicSource));
    }

    public void StopQuestMusic()
    {
        if (isTransitioning || questMusicSource == null) return;

        StartCoroutine(TransitionMusic(questMusicSource, backgroundMusicSource));
    }

    IEnumerator TransitionMusic(AudioSource fadeOut, AudioSource fadeIn)
    {
        isTransitioning = true;

        // ��������� ������� ������� ������ ��� ��������
        if (fadeIn == backgroundMusicSource)
        {
            fadeIn.time = backgroundMusicTime;
            fadeIn.Play();
        }

        // ������� �������
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            fadeOut.volume = Mathf.Lerp(1f, 0f, progress);
            fadeIn.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        // ����������� ���������
        fadeOut.volume = 0f;
        fadeIn.volume = 1f;

        if (fadeOut == backgroundMusicSource)
        {
            fadeOut.Pause();
        }
        else
        {
            fadeOut.Stop();
        }

        isTransitioning = false;
    }
}