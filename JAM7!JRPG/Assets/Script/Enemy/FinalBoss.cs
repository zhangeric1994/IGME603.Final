using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BossStage : int {
    Stage0,
    Stage1,
    Stage2,
    Stage3,
    Stage4,

    End
}

public enum bossMovement: int {
    Idle = 0,
    MiniFireballs,
    Dash,
    chest,
    missle,
    NumStatus
}
public class FinalBoss : Enemy {

    private BossStage currentStage;
    private BossStage defaultStage;
    private GameObject chest;
    private GameObject fireBall;
    public Transform[] Stage1Positions;
    public Transform stage2Position;
    public GameObject shield;
    private GameObject player;
    private GameObject missle;
    private GameObject stage3;
    public Transform misslePosition;
    private bool stage2isMove;
    private bool isSmall;
    public bool isEnterStage0;
    private bool isEnterStage2;
    private bool isInStage1Move;
    private float showUpDistance;
    public List<GameObject> enemiesS1;
    private GameObject currentS1emermy;
    private GameObject e;
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
    public GameObject crackup;
    public GameObject crackdown;
    public bool isInCrack;

    //------- new features ----------
    private Vector3 dashPlayerPosition;
    private GameObject miniFireball;

    private bool isFreezeStage;

    private bool isIdle;
    private bool isDash;

    private Transform currentBossPosition;
    private Transform groud;

    private Transform currentDashPosition;

    private Animator phoenix;

    public bool isActive = false;
    //-------------------------------

    //------- test param ------------
    public Transform wallLeft;
    public Transform wallRight;

    //-------------------------------

    void Start(){
        //enemy_anim = null;
        rb2d = null;
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

        miniFireball = (GameObject)Resources.Load("Prefabs/FinalBoss/miniFireball");
        if (!miniFireball) Debug.Log("mini fire ball not loaded");
        miniFireball.transform.localScale = new Vector3(0.04f, 0.04f, 1.0f);
        
        e = (GameObject)Resources.Load("Prefabs/Enemy/Runner");
        if (!e) Debug.Log("runner not loaded.");

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
        stage2HP = 3;
        isFireBall = false;
        stage3HP = 6;
        missleCnt = 0;
        isMissle = false;
        isstage3Silent = false;
        isInCrack = false;
        
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player) Debug.Log("Player not found");
        player.transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);

        s1EnemiesNum = enemiesS1.Count;

        if (s1EnemiesNum == 0) Debug.Log("No enemy loaded.");

        crackup.SetActive(false);
        crackdown.SetActive(false);
        shield.SetActive(false);

        phoenix = GetComponent<Animator>();

        //-------- new features ---------------
        dashPlayerPosition = player.transform.position;
        isFreezeStage = false;
        isIdle = true;
        isDash = false;
        currentBossPosition = gameObject.transform;
        groud = player.transform;
        Debug.Log(groud.position.y);
        groud.position = new Vector3(groud.position.x, groud.position.y + 1.0f, 0);
        Debug.Log(groud.position.y);

        currentDashPosition = wallLeft;


        //-------------------------------------

        //-------------------------------------

        //-------- Inherit from enemy class ---
        defaultState = EnemyState.Idle;
        setState(defaultState);
        health = 1000;
        //-------------------------------------
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

        // if (Input.GetKeyDown(KeyCode.U)){
        //     DestroyCurrentEnemy();
        // }

        // if (Input.GetKeyDown(KeyCode.I)){
        //     GenerateStage3();
        // }

        // if (Input.GetKeyDown(KeyCode.Alpha1)){
        //     throwMultiFireballs();
        // }

        //-------------------------------------

        //------ movement for boss in different stages ------
        // BossMovement();

        // if (isEnterStage0) ShowUp();

        // if (isEnterStage2) StartCoroutine(Stage1toStage2Move());

        // if (isInStage1Move) {
        //     Instage1Move(Stage1Positions[currentPosition].position);
        // }

        // if (currentStage == BossStage.End){
        //     gameObject.transform.position += new Vector3(0, Time.deltaTime, 0);
            
        // }

        // if (gameObject.transform.position.y > 2.0f){
        //     Destroy(gameObject);
        // }

        // StageSwitch();
        //---------------------------------------------------
        if (isActive){
            if (isIdle & isShowUp){
                isIdle = false;
                BossMovement();
            }


            if (isDash){
                Dash();
            }

            if (!isShowUp & !GameObject.FindGameObjectWithTag("chessBoss")){
                ShowUp();
                currentBossPosition = gameObject.transform;
            }
        }

        //Dash();
    }

    //------- Boss Movement Controller ---------
    //In each stage, boss will move differently
    void BossMovement(){
        // switch(currentStage){
        //     case BossStage.Stage0:
        //         return;

        //     case BossStage.Stage1:
        //         //spawn chest
        //         if (!currentS1emermy & !isInStage1Move & enemiesS1.Count > 0){
        //             isInStage1Move = true;
        //             currentS1emermy = enemiesS1[0];
        //             enemiesS1.RemoveAt(0);

        //             StartCoroutine(GenerateChest());
        //         }
        //         return;

        //     case BossStage.Stage2:

        //         float distance = Vector2.Distance(gameObject.transform.position, stage2Position.position);
        //         if (!currentFireBall & distance < 0.1f & !isFireBall & stage2HP > 0){
        //             isFireBall = true;
        //             StartCoroutine(GenerateFireBall());
        //         }

        //         if (isInCrack){
        //             if (Input.GetKeyDown(KeyCode.Alpha2)){
        //                 //enable shield
        //                 enableShield();
        //                 isInCrack = false;
        //                 StartCoroutine(shieldDelay());
        //             }
                    
        //         }

        //         return;

        //     case BossStage.Stage3:
        //         if (!currentMissle & !isMissle & !isstage3Silent){
        //             isMissle = true;

        //             if (missleCnt >= 3){
        //                 missleCnt = 0;
        //                 stage3HP--;
        //             }
        //             StartCoroutine(fireMissle());
        //         }
        //         return;

        //     case BossStage.Stage4:
        //         if (!isSmall) {
        //             gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);
        //             gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.843f, 0);
        //             isSmall = true;
        //         }
        //         return;
        //     case BossStage.End:
        //         return;
        //     default:
        //         Debug.Log("In boss movement: boss has an unknown stage.");
        //         return;
        // }
        var bossMoveCnt = (int)bossMovement.NumStatus;
        
         int randomMove = Random.Range(0, 100);
        //int randomMove = 4;
        int bossMove = 0;
        if (randomMove < 10){
            bossMove = 0;
        }
        else if(randomMove < 20){
            bossMove = 3;
        }
        else if (randomMove < 30){
            bossMove = 4;
        }
        else if (randomMove < 65){
            bossMove = 1;
        }

        else if (randomMove < 100){
            bossMove = 2;
        }

        switch((bossMovement) bossMove){
            case bossMovement.Idle:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, Stage1Positions[1].position.y, 0);
                StartCoroutine(Idle());
                break;

            case bossMovement.MiniFireballs:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, Stage1Positions[1].position.y, 0);
                StartCoroutine(shootMiniFireballs());
                break;

            case bossMovement.Dash:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, Stage1Positions[1].position.y, 0);
                dashPlayerPosition = new Vector3(player.transform.position.x, gameObject.transform.position.y, 0);
                StartCoroutine(DashPre());
                break;
            
            case bossMovement.chest:
                    StartCoroutine(GenerateChest());
                break;

            case bossMovement.missle:
                StartCoroutine(fireMissle());
                break;
        }
    }

    IEnumerator Idle(){
        yield return new WaitForSeconds(1.0f);
        isIdle = true;
    }

    IEnumerator shootMiniFireballs(){
        var bossColor = GetComponent<SpriteRenderer>().color;

        for (int i = 0; i < 8; i++){
            if (i % 2 == 0){
                GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(0.1f);
            }
            else {
                
                GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
        GetComponent<SpriteRenderer>().color = bossColor;
        throwMultiFireballs();
        yield return new WaitForSeconds(4.0f);
        isIdle = true;
    }

    void Dash(){
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentDashPosition.position.x, transform.position.y), Time.deltaTime);
        float distance = Vector2.Distance(gameObject.transform.position, new Vector2(currentDashPosition.position.x, transform.position.y));

        if (distance < 0.1f){
            isDash = false;
            isIdle = true;
            phoenix.SetBool("isRight", false);
            phoenix.SetBool("isLeft", false);
            phoenix.SetBool("isIdle", true);
            
        }
    }

    IEnumerator DashPre(){
        if (player.transform.position.x > gameObject.transform.position.x){
            currentDashPosition = wallRight;
            phoenix.SetBool("isRight", true);
            phoenix.SetBool("isIdle", false);
            phoenix.SetBool("isLeft", false);
        }
        else{
            currentDashPosition = wallLeft;
            phoenix.SetBool("isLeft", true);
            phoenix.SetBool("isIdle", false);
            phoenix.SetBool("isRight", false);
        }

        var bossColor = GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 8; i++){
            if (i % 2 == 0){
                GetComponent<SpriteRenderer>().color = Color.blue;
                yield return new WaitForSeconds(0.1f);
            }
            else {
                
                GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
        GetComponent<SpriteRenderer>().color = bossColor;

        
        isDash = true;
    }

    IEnumerator GenerateChest(){
        var bossColor = GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 8; i++){
            if (i % 2 == 0){
                GetComponent<SpriteRenderer>().color = Color.black;
                yield return new WaitForSeconds(0.1f);
            }
            else {
                
                GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
        GetComponent<SpriteRenderer>().color = bossColor;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Stage1Positions[1].position.y + 1.0f, 0.0f);

        yield return new WaitForSeconds(1.0f);
        GameObject _chest = Instantiate(chest, gameObject.transform.position, Quaternion.identity);
        Vector3 force = new Vector3(100 * (player.transform.position.x - _chest.transform.position.x), 0.0f, 0.0f);

        //throw towards player (looks like that)
        _chest.GetComponent<Rigidbody2D>().AddForce(force);
        Chest chestScript = _chest.GetComponent<Chest>();
        chestScript.SetEnemy(e);
        yield return new WaitForSeconds(4.0f);
        isIdle = true;
    }

    void throwMultiFireballs(){
        Vector3 moveDirection = new Vector3(0, 1, 0);
        for (int i = 0; i < 8; i++){
            moveDirection = Quaternion.Euler(0, 0, 45) * moveDirection;
            GameObject _miniFireBall = Instantiate(miniFireball, gameObject.transform.position, Quaternion.identity);
            miniFireball miniFireballSrc = _miniFireBall.GetComponent<miniFireball>();
            if (!miniFireballSrc) Debug.Log("src for mini fire ball not founded");
            miniFireballSrc.setForce(moveDirection.x, moveDirection.y);
        }
    }

    public void ShowUp(){
    
        transform.position = Vector2.MoveTowards(transform.position, Stage1Positions[1].position, Time.deltaTime);

        //distance to target position
        float distance = Vector2.Distance(transform.position, Stage1Positions[1].position);

        if (distance > showUpDistance) showUpDistance = distance;

        //scale factor
        Vector3 scaleFactor = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0), new Vector3(0.2f, 0.2f, 0), 1 - (distance / showUpDistance));

        gameObject.transform.localScale = scaleFactor;

        //boss reaches the target position
        if (distance < 0.1f){
            isEnterStage0 = false;
            isShowUp = true;
        }
        
    }

    



    //------- functions for stage 1 boss AI ----
    

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

    public void enableCrackUp(){
        crackup.SetActive(true);
    }

    public void disableCrackUp(){
        crackup.SetActive(false);
    }

    public void enableCrackDown(){
        crackdown.SetActive(true);
    }

    public void disableCrackDown(){
        crackdown.SetActive(false);
    }

    private void enableShield(){
        shield.SetActive(true);
    }

    private void disableShield(){
        shield.SetActive(false);
    }

    IEnumerator shieldDelay(){
        yield return new WaitForSeconds(0.1f);
        disableShield();
        isInCrack = true;
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
        var bossColor = GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 8; i++){
            if (i % 2 == 0){
                GetComponent<SpriteRenderer>().color = Color.gray;
                yield return new WaitForSeconds(0.1f);
            }
            else {
                
                GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
        GetComponent<SpriteRenderer>().color = bossColor;
        gameObject.transform.position = new Vector3(player.transform.position.x, Stage1Positions[1].position.y + 1.0f, 0.0f);
        GenerateMissle();
        isMissle = false;
        if(!isstage3Silent)missleCnt++;
        yield return new WaitForSeconds(1.0f);
        isIdle = true;
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

        if (currentStage == BossStage.Stage1 & enemiesS1.Count == 0 & currentS1emermy == null){
            currentStage = BossStage.Stage2;
            isEnterStage2 = true;
            crackdown.SetActive(true);
            Debug.Log("In stage switch: switch stage 1 to stage2");
        }

        if (currentStage == BossStage.Stage2 & stage2HP == 0){
            currentStage = BossStage.Stage3;
            Debug.Log("In stage switch: switch stage 2 to stage3");
        }

        if (currentStage == BossStage.Stage3 & stage3HP <= 0){
            currentStage = BossStage.End;
            Debug.Log("Congratulations");

        }
    }
    //----------------------------------------

    //Boss shows up (stage 0 -> stage 1)
    

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

    //--------- Boss Attack -----------------------
    
    void OnTriggerEnter2D(Collider2D other) {
        if (isDash){
            if (other.name == "Hitbox"){
                other.GetComponentInParent<PlayerCombatController>().Hurt(1);
            }
        }
    }

    //---------------------------------------------
}
