using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip cardFlipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameOverClip;

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioClips.Add("CardFlip", cardFlipClip);
        audioClips.Add("Match", matchClip);
        audioClips.Add("Mismatch", mismatchClip);
        audioClips.Add("GameOver", gameOverClip);
    }

    public void PlaySound(string clipName)
    {
        if (audioClips.ContainsKey(clipName))
        {
            audioSource.PlayOneShot(audioClips[clipName]);
        }
    }
}