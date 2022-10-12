using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;
using toio;
public class MoveSceneScript : MonoBehaviour
{

    float intervalTime = 0.05f;
        float elapsedTime = 0;
        Cube cube;

         [SerializeField] int port = 4000;
    OscServer server;
    public int speed;
        CubeManager cubeManager;

    public Transform mainCamera;
    private bool resetFlg = false;
    private int toio1Left = 0;
    private int toio1Right = 0;
    private int toio1Speed = 0;

    private int toio2Left = 0;
    private int toio2Right = 0;
    private int toio2Speed = 0;

    void OnEnable()
    {
        server = new OscServer(port);
        server.MessageDispatcher.AddCallback(
            "/test/recieve",
            (string address, OscDataHandle data) => {
                var stringValue = data.GetElementAsInt(0);
                var floatValue = data.GetElementAsFloat(1);
                var intValue = data.GetElementAsInt(2);
                Debug.Log($"OscJack receive: {address} {stringValue} {floatValue} {intValue}");
                 // Cube変数の生成が完了するまで早期リターン
                 speed = stringValue;

            }
       );

        server.MessageDispatcher.AddCallback(
            "/toio/1",
            (string address, OscDataHandle data) => {

                var leftValue = data.GetElementAsInt(0);
                var rightValue = data.GetElementAsInt(1);
                var speedValue = data.GetElementAsInt(2);
                Debug.Log($"OscJack receive: {address} left {leftValue} right {rightValue} speed {speedValue}");
                toio1Left = leftValue;
                toio1Right = rightValue;
                toio1Speed = speedValue;
                //textValue = ($"OSC recieved from {address}, with message: left {stringValue} right {floatValue} speed {intValue}");
            }
        );

        server.MessageDispatcher.AddCallback(
            "/toio/2",
            (string address, OscDataHandle data) => {
                var leftValue = data.GetElementAsInt(0);
                var rightValue = data.GetElementAsInt(1);
                var speedValue = data.GetElementAsInt(2);
                Debug.Log($"OscJack receive: {address} left {leftValue} right {rightValue} speed {speedValue}");
                toio2Left = leftValue;
                toio2Right = rightValue;
                toio2Speed = speedValue;
                //textValue = ($"OSC recieved from {address}, with message: left {stringValue} right {floatValue} speed {intValue}");
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
        server.Dispose();
        server = null;

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
        initToStartPosition();
        //initToResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // Cube変数の生成が完了するまで早期リターン

        if (toio1Left != 0 && toio1Right != 0 && toio1Speed != 0)
        {
            cubeManager.cubes[0].Move(toio1Left, toio1Right, toio1Speed);
            resetToio1Flags();
        }


        if (toio2Left != 0 && toio2Right != 0 && toio2Speed != 0)
        {
            cubeManager.cubes[1].Move(toio2Left, toio2Right, toio2Speed);
            resetToio2Flags();
        }
        if (resetFlg)
        {
            //reset

            resetFlg = false;
        }


        //if (speed != 0 ){
        //        cubeManager.cubes[0].Move(speed, speed, 200);
        //         cubeManager.cubes[1].Move( -speed, -speed, 200);
        
        ////          foreach(var cube in cubeManager.cubes)
        //// {
        ////     if (cubeManager.IsControllable(cube))
        ////     {
        ////                  cube.Move(speed, speed, 200);
                          

        ////     }
        //// }
        //   speed = 0;
        
        //    }
        //mainCamera.transform.position = new Vector3(cubeManager.cubes[0]., cubeManager.cubes[0].pos.y, 0);
    }

    void initToStartPosition(){
        //-0.5048941
        //3.107823e-06
        //-0.001395815
        //rot
        //-0.013
        //93.046
        //0.013
       
        cubeManager.cubes[0].TargetMove(113, 360, 0,0,0,Cube.TargetMoveType.RoundBeforeMove);
        cubeManager.cubes[1].TargetMove(870,360,180,0,0, Cube.TargetMoveType.RoundBeforeMove);
       
    }

    void initToResetPosition()
    {
        cubeManager.cubes[0].TargetMove(492, 307, 0, 0, 0, Cube.TargetMoveType.RoundBeforeMove);
        cubeManager.cubes[1].TargetMove(492, 414, 180, 0, 0, Cube.TargetMoveType.RoundBeforeMove);
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
