using UnityEngine;
using System.Collections;

public class hogehoge : MonoBehaviour {
    public TerrainData terrainData;
	// Use this for initialization
	void Start () {
        //αマップを書き換え。
        float[,,] oldMap = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        float[,,] newMap = new float[1024, 1024, 4];
        for(var x = 0; x < 1024; x++)
        {
            for(var y = 0; y < 1024; y++)
            {
                var oldMapX = Mathf.Min(x * 4, terrainData.alphamapWidth-1);
                var oldMapY = Mathf.Min(y * 4, terrainData.alphamapHeight - 1);
                newMap[x,y,0] = oldMap[oldMapX,oldMapY,0];
                newMap[x, y, 1] = oldMap[oldMapX, oldMapY, 1];
                newMap[x, y, 2] = oldMap[oldMapX, oldMapY, 2];
                newMap[x, y, 3] = oldMap[oldMapX, oldMapY, 3];

            }
        }
        //αマップを差し替える。
        terrainData.alphamapResolution = 1024;
        terrainData.SetAlphamaps(0, 0, newMap);
        terrainData.RefreshPrototypes();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
