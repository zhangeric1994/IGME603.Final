using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossStage : int {
    Stage0,
    Stage1,
    Stage2,
    Stage3,
    Stage4,

    End
}
public class FinalBoss : MonoBehaviour {

    private BossStage currentStage;
    private BossStage defaultStage;

    private GameObject chest;

    private GameObject fireBall;

    public Transform[] Stage1Positions;

    public Transform stage2Position;

    public GameObject player;

    private GameObject missle;

    private GameObject stage3;

    public Transform misslePosition;

    private bool stage2isMove;
    private bool isSmall;

    public bool isEnterStage0;
    private bool isEnterStage2;
    private bool isInStage1Move;

    private float showUpDistance;

    //stage 1 enemies
    public List<GameObject> enemiesS1;

    private GameObject currentS1emermy;

    private int s1EnemiesNum;

    private bool isShowUp;

    private int currentPosition;

    private int fireBallCnt;

    public Transform fireBallPoint;

    private int stage2HP;

    private GameObject currentFireBall;

    private bool isFireBall;

    private int missleCnt;

    private int stage3HP;

    private GameObject currentMissle;

    private bool isMissle;

    private bool isstage3Silent;

    public Transform skullPoint;

    private GameObject currentSkull;
    void Start(){
        defaultStage = BossStage.Stage0;
        currentStage = defaultStage;
        //Debug.Log("Default stage is: " + defaultStage.ToString());

        //------ load prefabs -----------
        chest = (GameObject)Resources.Load("Prefabs/FinalBoss/Stage1/chest");
        //chest.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);
        if (!chest) Debug.Log("Chest prefabs load failed.");

        fireBall = (GameObject)Resources.Load("Prefabs/FinalBoss/Stage2/fireBall");
        if (!fireBall) Debug.Log("fire ball not loaded.");

        missle = (GameObject)Resources.Load("Prefabs/FinalBoss/Stage3/missle");
        if (!missle) Debug.Log("missle load failed");

        stage3 = (GameObject)Resources.Load("Prefabs/FinalBoss/Stage3/stage3");
        if (!stage3) Debug.Log("Stage3 not loaded");

        //-------------------------------

        //------ initialize param--------
        stage2isMove = false;
        isSmall = false;
        isEnterStage0 = false;
        showUpDistance = 0.0f;
        isShowUp = false;
        isEnterStage2 = false;
        isInStage1Move = false;
        currentPosition = Random.Range(0, 3);
        fireBallCnt = 0;
        stage2HP = 1;
        isFireBall = false;
        stage3HP = 6;
        missleCnt = 0;
        isMissle = false;
        isstage3Silent = false;
        
        s1EnemiesNum = enemiesS1.Count;

        if (s1EnemiesNum == 0) Debug.Log("No enemy loaded.");
        //-------------------------------
    }

    void Update(){
        //------ test for boss functions --------

        //switch stage
        // if (Input.GetKeyDown(KeyCode.Tab)){
        //     switchStage();
        //     Debug.Log("Test for switch boss stage: current stage is: " + currentStage);
        // }

        // //throw chest
        // if (Input.GetKeyDown(KeyCode.E)) {
        //     GenerateChest();
        // }

        // //generate fire ball
        // if (Input.GetKeyDown(KeyCode.R)){
        //     GenerateFireBall();
        // }

        // if (Input.GetKeyDown(KeyCode.T)){
        //     GenerateMissle();
        // }

        // if (Input.GetKeyDown(KeyCode.Y)){
        //     Debug.Log(gameObject.transform.position.y);
        // }

        if (Input.GetKeyDown(KeyCode.U)){
            DestroyCurrentEnemy();
        }

        // if (Input.GetKeyDown(KeyCode.I)){
        //     GenerateStage3();
        // }

        //-------------------------------------

        //------ movement for boss in different stages ------
        BossMovement();

        if (isEnterStage0) ShowUp();

        if (isEnterStage2) StartCoroutine(Stage1toStage2Move());

        if (isInStage1Move) {
            Instage1Move(Stage1Positions[currentPosition].position);
        }

        

        StageSwitch();
        //---------------------------------------------------
    }

    //------- test function for switch satge -----
    void switchStage(){
        if (currentStage == BossStage.Stage4) return;
        currentStage += 1;
    }

    //------- Boss Movement Controller ---------
    //In each stage, boss will move differently
    void BossMovement(){
        switch(currentStage){
            case BossStage.Stage0:
                return;

            case BossStage.Stage1:
                //todo : 
                //1. determine how frequently change boss's position
                //2. Make boss move to the position not teleport to it
                
                //gameObject.transform.position = Stage1Positions[currentPosition].position;


                //spawn chest
                if (!currentS1emermy & !isInStage1Move){
                    isInStage1Move = true;
                   // gameObject.transform.position = Stage1Positions[currentPosition].position;
                    currentS1emermy = enemiesS1[0];
                    enemiesS1.RemoveAt(0);

                   
                    StartCoroutine(GenerateChest());
                }
                return;

            case BossStage.Stage2:

                float distance = Vector2.Distance(gameObject.transform.position, stage2Position.position);
                if (!currentFireBall & distance < 0.1f & !isFireBall & stage2HP > 0){
                    isFireBall = true;
                    StartCoroutine(GenerateFireBall());
                }
                return;

            case BossStage.Stage3:
                if (!currentMissle & !isMissle & !isstage3Silent){
                    isMissle = true;

                    //if(!isstage3Silent) missleCnt = 0;
                    // if (missleCnt >= 3) {
                    //     isstage3Silent = true;
                    //     missleCnt = 0;

                    //     if (isstage3Silent){
                    //         Stage3Silent();
                    //         //deleteSkull();
                    //     }
                    //     return;
                    // }
                    if (missleCnt >= 3){
                        missleCnt = 0;
                        stage3HP--;
                    }
                    StartCoroutine(fireMissle());
                }
                return;

            case BossStage.Stage4:
                if (!isSmall) {
                    gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.843f, 0);
                    isSmall = true;
                }
                return;
            case BossStage.End:
                return;
            default:
                Debug.Log("In boss movement: boss has an unknown stage.");
                return;
        }
    }

    //------- functions for stage 1 boss AI ----

    //1.generate chest in the boss's position
    //2.throw chest to the player
    IEnumerator GenerateChest(){
        yield return new WaitForSeconds(1.0f);
        GameObject _chest = Instantiate(chest, gameObject.transform.position, Quaternion.identity);
        Vector3 force = new Vector3(100 * (player.transform.position.x - _chest.transform.position.x), 0.0f, 0.0f);

        //throw towards player (looks like that)
        _chest.GetComponent<Rigidbody2D>().AddForce(force);
        Chest chestScript = _chest.GetComponent<Chest>();
        chestScript.SetEnemy(currentS1emermy);
    }

    //throw the chest when enemy in chest is defeated.
    //when the amount of enemies reaches a value, change the stage and trigger stage attack


    //----------------------------------------


    //-------- Stage 2 boss AI ---------------
    //boss will shoot fire ball towards player
    IEnumerator GenerateFireBall(){
        yield return new WaitForSeconds(2.0f);
        currentFireBall = Instantiate(fireBall, fireBallPoint.position, Quaternion.identity);
        isFireBall = false;
    }

    public void fireballDamage(){
        stage2HP--;
    }

    //todo:
    //2. how to defend fire ball

    //----------------------------------------

    //-------- Stage 3 boss AI ---------------
    GameObject GenerateMissle(){
        GameObject _missle = Instantiate(missle, misslePosition.position, Quaternion.identity);
        _missle.GetComponent<Rigidbody2D>().transform.Rotate(new Vector3(0, 0, -90.0f));
        return _missle;
    }

    IEnumerator fireMissle(){
        yield return new WaitForSeconds(2.0f);
        gameObject.transform.position = new Vector3(player.transform.position.x, Stage1Positions[0].position.y, 0.0f);
        currentMissle = GenerateMissle();
        isMissle = false;
        if(!isstage3Silent)missleCnt++;
    }

    void Stage3Silent(){
        StartCoroutine(GenerateStage3());
        StartCoroutine(changeSilentState());
    }

    IEnumerator GenerateStage3(){
        yield return new WaitForSeconds(0.3f);
        GameObject _stage3 = Instantiate(stage3, skullPoint.position, Quaternion.identity);
        currentSkull = _stage3;
    }

    IEnumerator changeSilentState(){
        yield return new WaitForSeconds(1.0f);
        isstage3Silent = false;
    }

    IEnumerator deleteSkull(){
        yield return new WaitForSeconds(1.2f);
        Destroy(currentSkull);
    }

    public void SkullDamage(){
        stage3HP--;
    }

    //----------------------------------------

    //-------- Stage 4 boss AI ---------------
    

    //----------------------------------------

    //-------- Switch Stage ------------------

    //switch stages for boss
    void StageSwitch(){
        if (currentStage == BossStage.Stage0 & isShowUp){
            currentStage++;
            Debug.Log("In stage switch: switch stage 0 to stage 1");
        }

        if (currentStage == BossStage.Stage1 & enemiesS1.Count == 0){
            currentStage++;
            isEnterStage2 = true;
            Debug.Log("In stage switch: switch stage 1 to stage2");
        }

        if (currentStage == BossStage.Stage2 & stage2HP == 0){
            currentStage++;
            Debug.Log("In stage switch: switch stage 2 to stage3");
        }

        if (currentStage == BossStage.Stage3 & stage3HP <= 0){
            currentStage = BossStage.End;
            Debug.Log("Congratulations");
        }
    }
    //----------------------------------------

    //Boss shows up (stage 0 -> stage 1)
    void ShowUp(){
    
        transform.position = Vector2.MoveTowards(transform.position, Stage1Positions[1].position, Time.deltaTime);

        //distance to target position
        float distance = Vector2.Distance(transform.position, Stage1Positions[1].position);

        if (distance > showUpDistance) showUpDistance = distance;

        //scale factor
        Vector3 scaleFactor = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0), new Vector3(0.4f, 0.4f, 0), 1 - (distance / showUpDistance));

        gameObject.transform.localScale = scaleFactor;

        //boss reaches the target position
        if (distance < 0.1f){
            isEnterStage0 = false;
            isShowUp = true;
        }
        
    }

    IEnumerator Stage1toStage2Move(){
        yield return new WaitForSeconds(3.0f);
        transform.position = Vector2.MoveTowards(transform.position, stage2Position.position, Time.deltaTime);

        float distance = Vector2.Distance(transform.position, stage2Position.position);

        if (distance < 0.1f){
            isEnterStage2 = false;
        }
    }

    void Instage1Move(Vector3 targetPosition) {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime);

        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance < 0.1f){
            isInStage1Move = false;
            currentPosition = Random.Range(0, 3);
        }
    }

    

    //--------- Debug functions -------------------
    //stage 1 debug function
    void DestroyCurrentEnemy(){
        Destroy(currentS1emermy);
    }
    //---------------------------------------------
}
