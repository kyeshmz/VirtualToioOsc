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
        cubeManager = new CubeManager();
        await cubeManager.MultiConnect(2);
    }

    // Update is called once per frame
    void Update()
    {
         // Cube変数の生成が完了するまで早期リターン
  
            if(speed != 0 ){

                 foreach(var cube in cubeManager.cubes)
        {
            if (cubeManager.IsControllable(cube))
            {
                         cube.Move(speed, speed, 200);
                          

            }
        }
           speed = 0;
        
            }
    }
}
