using UnityEngine;
using UnityEngine.UI;

using OscJack;

public class Sender : MonoBehaviour
{

    private float posX;
    private float posY;
    public Text textElement;

    [SerializeField] string ipAddress = "127.0.0.1";
    [SerializeField] int sendPort = 4004;
    OscClient client;

    void OnEnable(){
        client = new OscClient(ipAddress, sendPort);

    }

         void OnDisable()
    {
        client.Dispose();
        client = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posX = Random.Range (1.0f, 100.0f);
        posY = Random.Range(1.0f, 100.0f);
        client.Send("/toio0", 0, ((int)posX), ((int)posY), 100);
        client.Send("/toio1", 1, ((int)posX), ((int)posY), 100);

        textElement.text = ($"Sending OSC to /toio/1 {posX.ToString()} {posY.ToString()}");
    }
}
