using UnityEngine;
using System.Collections;

public class BestScore : MonoBehaviour {
    TextMesh[] textMeshs;
    Player player;
    // Use this for initialization
    void Start () {
        textMeshs = transform.GetComponentsInChildren<TextMesh>();
        player = GameObject.Find("Player").GetComponent<Player>();
        //最初はテキストメッシュを非表示にしておく。
        foreach (var textMesh in textMeshs)
        {
            textMesh.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// レースが開始したことを通知。
    /// </summary>
    void NotifyStart()
    {
        foreach (var textMesh in textMeshs)
        {
            textMesh.gameObject.SetActive(true);
            textMesh.text = string.Format("BEST SCORE {0}", player.bestGetCoinNum);
        }
    }
}
