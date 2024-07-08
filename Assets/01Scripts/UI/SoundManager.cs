using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip loginClip;
    [SerializeField]
    private AudioClip townClip;
    [SerializeField]
    private AudioClip battleClip;

    private AudioClip currentClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(DelayedSetup());
    }

    private void Update()
    {
        if (GameManager.isPlayGame)
        {
            // �ʱ� ���¿� �´� ����� Ŭ�� ����
            UpdateAudioClip();
        }
    }
    private void UpdateAudioClip()
    {
        AudioClip newClip = null;

        if (GameManager.isTown && !GameManager.isBattle)
        {
            newClip = townClip;
        }
        else if (!GameManager.isTown && GameManager.isBattle)
        {
            newClip = battleClip;
        }

        // ���ο� Ŭ���� ���� Ŭ���� �ٸ��� ����
        if (newClip != null && newClip != currentClip)
        {
            currentClip = newClip;
            audioSource.clip = currentClip;
            audioSource.Play();
        }
    }

    private IEnumerator DelayedSetup()
    {
        yield return new WaitUntil(() => GameManager.isPlayGame);

        audioSource = GetComponent<AudioSource>();

        // �ʱ� ���¿� �´� ����� Ŭ�� ����
        UpdateAudioClip();
    }
}
