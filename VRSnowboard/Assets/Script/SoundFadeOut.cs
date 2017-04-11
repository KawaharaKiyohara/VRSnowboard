using UnityEngine;
using System.Collections;

public class SoundFadeOut : MonoBehaviour {
    AudioSource audioSource;
	// Use this for initialization
	void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        audioSource.volume -= 2.0f * Time.deltaTime;
        if(audioSource.volume <= 0.0f)
        {
            audioSource.volume = 0.0f;
            //フェードアウト終わり。
            Destroy(this);
        }
	}
}
