using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;
using toio;
using UnityEngine.UI;
public class MoveSceneScript : MonoBehaviour
{

    float intervalTime = 0.05f;
    float elapsedTime = 0;
    Cube cube;
    private string textValue;
    public Text recieveTextElement;
    public Text sendTextElement;
    public Text sendTextElement2;
    [SerializeField] int recievePort = 4000;
    OscServer server;

    CubeManager cubeManager;

    private float posX;
    private float posY;


    [SerializeField] string sendipAddress = "127.0.0.1";
    [SerializeField] int sendPort = 4004;
    OscClient client;

    private bool resetFlg = false;
    private int toio1Left = 0;
    private int toio1Right = 0;
    private int toio1Speed = 0;
    private int toio1posX = 0;
    private int toio1posY = 0;

    private int toio2Left = 0;
    private int toio2Right = 0;
    private int toio2Speed = 0;
    private int toio2posX = 0;
    private int toio2posY = 0;

    void OnEnable()
    {
        server = new OscServer(recievePort);
        client = new OscClient(sendipAddress, sendPort);

        server.MessageDispatcher.AddCallback(
            "/toio0",
            (string address, OscDataHandle data) => {

                var leftValue = data.GetElementAsInt(0);
                var rightValue = data.GetElementAsInt(1);
                var speedValue = data.GetElementAsInt(2);
                Debug.Log($"OscJack receive: {address} left {leftValue} right {rightValue} speed {speedValue}");
                toio1Left = leftValue;
                toio1Right = rightValue;
                toio1Speed = speedValue;
                textValue = ($"OSC recieved from {address}, with message: left {leftValue} right {rightValue} speed {speedValue}");
            }
        );

        server.MessageDispatcher.AddCallback(
            "/toio1",
            (string address, OscDataHandle data) => {
                var leftValue = data.GetElementAsInt(0);
                var rightValue = data.GetElementAsInt(1);
                var speedValue = data.GetElementAsInt(2);
                Debug.Log($"OscJack receive: {address} left {leftValue} right {rightValue} speed {speedValue}");
                toio2Left = leftValue;
                toio2Right = rightValue;
                toio2Speed = speedValue;
                textValue = ($"OSC recieved from {address}, with message: left {leftValue} right {rightValue} speed {speedValue}");
            }
        );

        server.MessageDispatcher.AddCallback(
           "/reset",
           (string address, OscDataHandle data) => {
               var resetValue = data.GetElementAsInt(0);
               if(resetValue == 1)
               {
                   resetFlg = true;
               }
               Debug.Log($"OscJack receive: {address} reset {resetValue}");
                //textValue = ($"OSC recieved from {address}, with message: left {stringValue} right {floatValue} speed {intValue}");
            }
       );

    }

      void OnDisable()
    {
        client.Dispose();
        server.Dispose();
        server = null;
        client = null;

    }
    // Start is called before the first frame update
    async void Start()
    {
            //        // Bluetoothデバイスを検索
            // var peripheral = await new NearestScanner().Scan();
            // // デバイスへ接続してCube変数を生成
            // cube = await new CubeConnecter().Connect(peripheral); 


          // CubeManagerからモジュールを間接利用した場合:
        cubeManager = new CubeManager(ConnectType.Simulator);
        await cubeManager.MultiConnect(2);
     
        //initToResetPosition();
    }

    // Update is called once per frame
    void Update()
    {

        // Cube変数の生成が完了するまで早期リターン
        

        client.Send("/toio0", 0, ((int)cubeManager.cubes[0].x), ((int)cubeManager.cubes[0].y), 100);
        client.Send("/toio1", 1, ((int)cubeManager.cubes[1].x), ((int)cubeManager.cubes[1].y), 100);
        recieveTextElement.text = textValue;
        sendTextElement.text = ("/toio0 " + ((int)cubeManager.cubes[0].x) + " " + ((int)cubeManager.cubes[0].y));
        sendTextElement2.text = ("/toio1 " + ((int)cubeManager.cubes[1].x) + " "+ ((int)cubeManager.cubes[1].y));

        if (toio1Left != 0 && toio1Right != 0 && toio1Speed != 0)
        {
            //
            if (toio1posY < 307 || toio1posY > 414)
            {

                cubeManager.cubes[0].Move(toio1Left, toio1Right, (int)(toio1Speed* 0.8));

            } else
            {
                cubeManager.cubes[0].Move(toio1Left, toio1Right, toio1Speed);

            }
            resetToio1Flags();
        }


        if (toio2Left != 0 && toio2Right != 0 && toio2Speed != 0)
        {
            if (toio2posY < 307 || toio2posY > 414)
            {

                cubeManager.cubes[1].Move(toio2Left, toio2Right, (int)(toio2Speed * 0.8));

            }
            else
            {
                cubeManager.cubes[1].Move(toio2Left, toio2Right, toio1Speed);
      

            }
            resetToio2Flags();
        }


        if (resetFlg)
        {
            //reset


            resetFlg = false;
        }
        

      



    }


    void resetToio1Flags()
    {
        toio1Left = 0;
        toio1Right = 0;
        toio1Speed = 0;
    }
    void resetToio2Flags()
    {
        toio2Left = 0;
        toio2Right = 0;
        toio2Speed = 0;
    }
}
