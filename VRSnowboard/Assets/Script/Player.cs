using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Vector3 moveSpeed;  //移動速度。
    Vector3 addVelocity = Vector3.zero;
    float waittimer = 0.0f;
    Vector3 playerTurnVelocity = Vector3.zero;   //旋回速度。
    Vector3 cameraTurnVelocity = Vector3.zero;  //カメラの進行方向。
    Vector3 boardTurnVelocity = Vector3.zero;
    Vector3 boardDirection;                     //ボードの向き。
    Camera camera;
    public GameObject goGameOver;
    public GameObject snowboard;
    public TextMesh timerTextMesh;
    public GameObject startText;
    public GameObject goGameClear;
    float gameOverTimer = 0.0f;
    bool isGameOver = false;
    public int getCoinNum;
    public int bestGetCoinNum { get; private set; }
    AudioSource coinGetSE;
    public TextMesh coinTextMesh;
    public TextMesh coinTextMeshSh;
    bool isGameStart = false;
    public GameObject goBGM;
    GameObject cameraCorrect;
    /// <summary>
    /// コインを取得したことを通知する。
    /// </summary>
    public void NotifyGetCoin()
    {
        getCoinNum++;
        coinGetSE.time = 0.0f;
        coinGetSE.Play();
    }
    // Use this for initialization
    void Start()
    {
        coinGetSE = GameObject.Find("CoinGetSE").GetComponent<AudioSource>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestGetCoinNum = PlayerPrefs.GetInt("BestScore");
        }else
        {
            bestGetCoinNum = 0;
        }
        cameraCorrect = GameObject.Find("CameraCorrect");
        boardDirection = transform.forward;
#if UNITY_EDITOR
        GvrViewer.Instance.UseUnityRemoteInput = true;
#endif
    }
    private void LateUpdate()
    {
        if (!isGameStart)
        {
            //メインカメラの回転を打ち消す逆クォータニオンを計算。
            cameraCorrect.transform.localRotation = Quaternion.Inverse(camera.transform.localRotation);
        }
    }
    public Vector3 CameraDir;
    // Update is called once per frame
    void Update()
    {
        float waitTime = 3.9f;
        waittimer += Time.deltaTime;
        int time = (int)(waitTime - waittimer);
        if (time > 0)
        {
            timerTextMesh.text = time.ToString();
        }
        else
        {
            timerTextMesh.text = "GO";
            timerTextMesh.color = Color.green;
        }

        if(waittimer < waitTime)
        {
            return;
        }
        if (!isGameStart)
        {
            gameObject.BroadcastMessage("NotifyStart");
            timerTextMesh.gameObject.SetActive(false);
            startText.SetActive(false);
            
            StartCoroutine(StartBGM());
            
            isGameStart = true;
        }
        //適当に前に進めてみる。
        Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);
        addVelocity = gravity * Time.deltaTime;
        Vector3 localMoveSpeed = moveSpeed + addVelocity;

        CharacterController charaCtr = GetComponent<CharacterController>();
        if (CollisionFlags.None == charaCtr.Move(localMoveSpeed))
        {
            moveSpeed = localMoveSpeed;
        }
        //摩擦
        moveSpeed *= 0.98f;

        Turn();
    }

    private IEnumerator StartBGM()
    {
        yield return new WaitForSeconds(0.14f);
        AudioSource s = goBGM.GetComponent<AudioSource>();
        s.volume = 0.0f;
        s.Play();
        SoundFadeIn fadeIn = goBGM.AddComponent<SoundFadeIn>();
        fadeIn.targetVolume = 0.5f;
        //goBGM.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// キャラクターコントローラーがコライダーにヒットしたときに呼ばれる処理。
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float t = Vector3.Dot(-hit.normal, Vector3.up);
        if (Mathf.Abs(t) < 0.7f)
        {
            //壁と判定。
            moveSpeed.x *= 0.98f;
            moveSpeed.z *= 0.98f;
            gameOverTimer += Time.deltaTime;
            if (gameOverTimer > 1.0f && isGameOver == false)
            {
                //ゲームオーバー。
                goGameOver.SetActive(true);
                StartCoroutine(GameSceneLoad());
                goBGM.AddComponent<SoundFadeOut>();
                isGameOver = true;
            }
            return;
        }
        gameOverTimer = 0.0f;
        //このフレームで加速した速度を水平成分と垂直成分に分解する。
        Vector3 velocity_v = -hit.normal * Vector3.Dot(-hit.normal, addVelocity);
        Vector3 velocity_h = addVelocity - velocity_v;
        Vector3 localBoardDirection = transform.worldToLocalMatrix * boardDirection;
        Vector3 localUp = transform.worldToLocalMatrix * hit.normal;
        Vector3 localBoardRight = Vector3.Cross(localBoardDirection, localUp);
        snowboard.transform.localRotation = Quaternion.LookRotation(Vector3.Cross(localBoardRight, localUp), localUp);
        //snowboard.transform.localRotation = Quaternion.Slerp(snowboard.transform.localRotation, boardTargetRotation, 0.8f);
        Vector3 pos = hit.point;
        pos.y += 3.0f;
        snowboard.transform.position = pos;

        //水平方向の速度をボードの向きに射影する。
        float d = Mathf.Max(Vector3.Dot(boardDirection, velocity_h.normalized), 0.4f);
        float vel = d * 3.2f * Time.deltaTime;

        Vector3 addVel = boardDirection * vel;

        //水平成分を加算。
        moveSpeed += addVel;// velocity_h;
    }

    private IEnumerator GameSceneLoad()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("game");

    }

    /// <summary>
    /// 旋回。
    /// </summary>
    private void Turn()
    {
        //プレイヤーの体の向きをボードの進行方向に向ける
        Vector3 direction = Vector3.SmoothDamp(transform.forward, boardDirection, ref playerTurnVelocity, 0.5f);
        transform.localRotation = Quaternion.LookRotation(direction);

        Quaternion qRot = Quaternion.identity;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            qRot = Quaternion.AngleAxis(-1.0f, Vector3.up);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            qRot = Quaternion.AngleAxis(1.0f, Vector3.up);
        }
        boardDirection = qRot * boardDirection;
        boardDirection.Normalize();
#else
        //ボードはカメラの方向に向ける。
        //  Vector3 forward = Vector3.forward * GvrViewer.Instance.EyePose.direction;
        CameraDir = camera.transform.forward;
         Vector3 targetBoardDirection = camera.transform.forward;
        targetBoardDirection.y = 0.0f;
        targetBoardDirection.Normalize();
        boardDirection = Vector3.SmoothDamp(boardDirection, targetBoardDirection, ref playerTurnVelocity, 0.2f);
        boardDirection.Normalize();
#endif

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Coin")
        {
            goGameClear.SetActive(true);
            //ベストスコアを保存
            if (bestGetCoinNum <= getCoinNum)
            {
                PlayerPrefs.SetInt("BestScore", getCoinNum);
                PlayerPrefs.Save();
            }
            StartCoroutine(GameSceneLoad());
            isGameOver = true;
        }
    }
}