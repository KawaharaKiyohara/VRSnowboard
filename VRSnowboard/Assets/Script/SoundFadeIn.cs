using UnityEngine;
using System.Collections;

public class SoundFadeIn : MonoBehaviour {
    public float targetVolume { get; set; }  //ターゲットとなるボリューム。
    AudioSource audioSource;
    // Use this for initialization
    private void Awake()
    {
        targetVolume = 1.0f;
    }
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        audioSource.volume += 2.0f * Time.deltaTime;
        if(audioSource.volume >= targetVolume)
        {
            audioSource.volume = targetVolume;
            //フェードイン終わり。
            Destroy(this);
        }
	}
}
