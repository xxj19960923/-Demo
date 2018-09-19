using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Move : MonoBehaviour {

    private Animator anim;   //动画组件
    private Rigidbody rigi; // 刚体组件
    private UIManager UImanager; //  UI管理器
    private GameManager Gamemanager;// 游戏管理器
    private GameObject box;//用于存储碰撞到的Box
    private Collider tool;//用于存储触发到的道具

    internal static DOTweenAnimation Task;
    internal static DOTweenAnimation MyTask;
    internal static DOTweenAnimation BagAnim;
    private Tweener TaskTweener;

    public float speed = 0.2f;   //移速
    float timer = 0.2f;
    float t1 = 0;
    float t2 = 0;

    int count = 0;
    int ShotType = 1;//默认攻击类型

    public static bool RightOpen = false; // 右键放大标志位
    public static bool OpenAgain = false; // 右键开镜二次放大标志位
    bool getTool = false; //是否在捡东西标志位
    public static bool isShow = false; //是否处于功能状态
    bool OpenBox = false; // 是否处于打开箱子状态
    bool shotOut = false;//是否点射执行
    public static bool isTask = false;//是否进入任务模式
    public static bool isShake = false;//是否狙击开枪

    private object[] message = new object[2];//传递多个参数


    private void Awake() {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        UImanager = GameObject.Find("UIManager").GetComponent<UIManager>();
        Gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Task = GameObject.Find("UIManager/Task").GetComponent<DOTweenAnimation>();
        MyTask = GameObject.Find("UIManager/MyTask").GetComponent<DOTweenAnimation>();
        BagAnim = GameObject.Find("UIManager/BagSystem").GetComponent<DOTweenAnimation>();
    }


    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!RightOpen && !OpenAgain && !isShow) {
            SkillController();
        }
        if (!isShow) {
            Controller();
        }
    }

    //移动控制
    void Controller() {

        if (!anim.GetBool("Shot")) {
            //按下Shift键为跑步
            if (Input.GetKey(KeyCode.LeftShift)) {
                Dir8("Running", speed * 2);
            }
            //不按下为走路
            else {
                Dir8("Move", speed);
            }
            Jump();
            ChangeShot();
            Shot();
        }

        ButtonUp();
    }
    //键位抬起处理
    void ButtonUp(){
        //判断是否播放跑步动画
        CheckShift();
        if(Input.GetMouseButtonUp(0) && ShotType == 1) {
            anim.SetBool("Shot", false);
        }
        if (Input.GetKeyUp(KeyCode.F)) {
            if (getTool) {
                getTool = false;
            }           
        }
    }

    //8个方向的位置以及动画处理
    void Dir8(string state,float speed) {
 
        if (Input.GetKey(KeyCode.W)) {
            transform.localPosition += transform.forward * speed;
            anim.SetFloat(state, 0.5f);
        }
        else if (Input.GetKey(KeyCode.S)) {
            transform.localPosition -= transform.forward * speed;
            anim.SetFloat(state, 1);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.localPosition -= transform.right * speed;
            anim.SetFloat(state, 0.25f);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.localPosition += transform.right * speed;
            anim.SetFloat(state, 0.75f);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) {
            transform.localPosition += (transform.forward - transform.right) / 10 * speed;
            anim.SetFloat(state, 0.125f);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) {
            transform.localPosition += (transform.forward + transform.right) / 10 * speed;
            anim.SetFloat(state, 0.375f);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) {
            transform.localPosition -= (transform.forward + transform.right) / 10 * speed;
            anim.SetFloat(state, 0.625f);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) {
            transform.localPosition += (-transform.forward + transform.right) / 10 * speed;
            anim.SetFloat(state, 0.875f);
        }
        
    }
    //重置为站立状态
    void StateChange() {
        anim.SetFloat("Move", 0);
        if (anim.GetBool("Run") == true) {
            anim.SetBool("Run", false);
        }
    }
    
    void CheckShift() {
        //判断在按下任意方向键后按LeftShift键才会播放跑步动画
        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.D)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                anim.SetBool("Run", true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift)) {
                anim.SetBool("Run", false);
            }
        }
        //当没有方向键按下时重置为站立状态
        else {
            StateChange();
        }      
    }

    //跳跃
    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("Jump")) {
            rigi.velocity = new Vector3(0, 6, 0);
            anim.SetBool("Jump", true);
        }
        anim.SetFloat("Velocity", rigi.velocity.y);
    }
    //射击操作
    void Shot() {
        if(Cursor.lockState == CursorLockMode.None) {
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
        //冲锋枪为连射
        if (ShotType == 1) {
            if (Input.GetMouseButton(0) && anim.GetFloat("Move") == 0) {
                anim.SetBool("Shot", true);
                anim.SetFloat("Type", ShotType);
            }
        }
        //其他为点射
        else if (Input.GetMouseButtonDown(0) && anim.GetFloat("Move") == 0) {
            anim.SetBool("Shot", true);
            anim.SetFloat("Type", ShotType);
            if (RightOpen) {
                Gamemanager.SendMessage("CameraShake");                
            }
        }
        //当为点射时可右键放大
        if (Input.GetMouseButtonUp(1) && ShotType == 2) {
            if (!OpenAgain) {
                RightOpen = !RightOpen;
            }
            Gamemanager.SendMessage("CameraChange");//发送改变相机的信息
            UImanager.SendMessage("ImageSetActive", RightOpen);//发送改变图片的信息
        }
    }
    //修改射击类型
    void ChangeShot() {
        if (Input.GetKeyDown(KeyCode.Q) && !RightOpen) {
            ShotType++;
            if (ShotType > 3)
                ShotType = 1;
        }
        UImanager.SendMessage("TypeTxt", ShotType,SendMessageOptions.DontRequireReceiver);
    }


    //重置跳跃条件
    public void JumpState() {
        rigi.velocity = new Vector3(0, 0, 0);
        anim.SetBool("Jump", false);
    }
    //重置射击条件
    IEnumerator ShotState() {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Shot", false);
        if (isShake)
            isShake = false;
    }

    void OnCollisionEnter(Collision col) {
        anim.SetBool("Ground", true);
        //当在地形时才可以跳跃
        if (col.gameObject.tag == "Terrain") {
            anim.SetBool("Jump", false);
        }//碰撞到NPC有提示信息
        else if (col.gameObject.tag == "NPC" && InTask.isGet == false) {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(true, "按V键接任务"));
            isTask = true;
        }
    }
    void OnCollisionStay(Collision col) {
        //接完任务不在显示提示信息而且在接任务时不会显示
        if (col.gameObject.tag == "NPC" && InTask.isGet) {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(false, null));
        }
    }

    void OnCollisionExit(Collision col) {
        if (col.gameObject.tag == "Terrain") {
            anim.SetBool("Ground", false);
            anim.SetFloat("Velocity", rigi.velocity.y);
        }
        else if (col.gameObject.tag == "NPC") {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(false, null));
            isTask = false;
        }
    }

    //与道具发生触发事件
    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Box") {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(true, "按G键打开"));
            OpenBox = true;
            box = col.gameObject;
        }
    }
    void OnTriggerStay(Collider col) {
        if(col.gameObject.tag == "Tool" && !getTool) {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(true, "按F键拾取"));
            getTool = true;
            tool = col;
        }        
    }
    void OnTriggerExit(Collider col) {
        if(col.gameObject.tag == "Tool") {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(false, null));
            getTool = false;
        }else if(col.gameObject.tag == "Box") {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(false, null));
            OpenBox = false;
        }       
    }

    //为message赋值,用于给UIManager发送消息
    object[] SetMessage(bool active,string txt) {
        message[0] = active;
        message[1] = txt;
        return message;
    }

    //判断道具名称
    void CheckToolName(Collider col) {
        if(col.gameObject.name == "coin5") {
            PlayerState.coin += 5;
        }else if(col.gameObject.name == "coin2"){
            PlayerState.coin += 2;
        }
        else if (col.gameObject.name == "coin1") {
            PlayerState.coin += 1;
        }
        else {//通过分离字符串确认道具的id
            string str = col.gameObject.name;
            string[] strArray = str.Split(',');
            int id = int.Parse(strArray[1]);
            Bag._Instance.GetID(id);
        }
    }

    //功能按键
    void SkillController() {
        //开启任务栏，需要时间过渡才能继续
        if (Input.GetKeyDown(KeyCode.V) && isTask && !UIManager.timeOut && !InTask.isGet) {
            StateChange();
            Cursor.lockState = CursorLockMode.None;            
            Task.DOPlayForward();
            isShow = true;
        }//开箱子
        else if(Input.GetKeyDown(KeyCode.G) && OpenBox) {
            OpenBox = false;
            box.gameObject.SendMessage("BoxOpenAnim");
            UImanager.SendMessage("GameObjectSetActive", SetMessage(false, null)); 
        }//捡道具
        else if(Input.GetKeyDown(KeyCode.F) && getTool) {
            UImanager.SendMessage("GameObjectSetActive", SetMessage(false, null));
            CheckToolName(tool);
            Destroy(tool.gameObject);
            getTool = false;
        }//查看自己任务
        else if(Input.GetKeyDown(KeyCode.V) && !UIManager.timeOut) {
            StateChange();
            Cursor.lockState = CursorLockMode.None;
            MyTask.DOPlayForward();
            isShow = true;
        }else if(Input.GetKeyDown(KeyCode.I) && !UIManager.timeOut) {
            StateChange();
            Cursor.lockState = CursorLockMode.None;
            BagAnim.DOPlayForward();
            isShow = true;
        }
    }
}
//if (!timeOut) {
//    if (!isShow) {
//        StateChange();
//        Cursor.lockState = CursorLockMode.None;
//        Task.DOPlayForward();
//    }
//    else{
//        Cursor.lockState = CursorLockMode.Locked;
//        Task.DOPlayBackwards();                    
//    }
//    isShow = !isShow;
//    StartCoroutine(Wait(timeOut));
//}