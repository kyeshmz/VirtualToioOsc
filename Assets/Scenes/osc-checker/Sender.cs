using UnityEngine;
using UnityEngine.UI;

using OscJack;

public class Sender : MonoBehaviour
{

    private float textValue;
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
        textValue = Random.Range (1.0f, 100.0f); 
        client.Send("/test/send", textValue);
        textElement.text = ($"Sending OSC to /test/send {textValue.ToString()}");
    }
}
