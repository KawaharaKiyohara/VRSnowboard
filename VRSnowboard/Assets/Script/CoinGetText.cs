using UnityEngine;
using System.Collections;

public class CoinGetText : MonoBehaviour {
    TextMesh[] textMeshs;
    Player player;
	// Use this for initialization
	void Start () {
        textMeshs = transform.GetComponentsInChildren<TextMesh>();
        player = GameObject.Find("Player").GetComponent<Player>();
        //最初はテキストメッシュを非表示にしておく。
        foreach(var textMesh in textMeshs)
        {
            textMesh.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach(var textMesh in textMeshs)
        {
            textMesh.text = string.Format("SCORE {0}", player.getCoinNum);
        }
    }
    /// <summary>
    /// レースが開始したことを通知。
    /// </summary>
    void NotifyStart()
    {
        foreach(var textMesh in textMeshs)
        {
            textMesh.gameObject.SetActive(true);
        }
    }
}
