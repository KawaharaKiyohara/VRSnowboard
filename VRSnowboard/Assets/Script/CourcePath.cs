using UnityEngine;
using System.Collections;


/// <summary>
/// コースパス
/// </summary>
public class CourcePath  {
    public Node rootNode;
    /// <summary>
    /// コースパスのノード
    /// </summary>
    public class Node
    {
        public Vector3 position;
        public Node nextNode;
    }
    public CourcePath()
    {
        //コースパスを作成。
        //ウェイポイントのルートを検索。
        Transform waypoint = GameObject.Find("waipointRoot").transform;
        rootNode = new Node();
        rootNode.position = waypoint.position;
        Node currentNode = rootNode;
        while (true){
            if(waypoint.childCount == 0)
            {
                //終わり。
                break;
            }
            waypoint = waypoint.GetChild(0);
            currentNode.nextNode = new Node();
            currentNode.nextNode.position = waypoint.position;
            currentNode = currentNode.nextNode;
        }
    }
}
