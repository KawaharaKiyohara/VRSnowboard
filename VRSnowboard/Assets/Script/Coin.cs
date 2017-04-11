using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
    float angle = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        angle += Time.deltaTime * 240.0f;
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
	}
    private void OnTriggerEnter(Collider other)
    {
        Player pl = GameObject.Find("Player").GetComponent<Player>();
        pl.NotifyGetCoin();
        Object.Destroy(gameObject);
    }
}
