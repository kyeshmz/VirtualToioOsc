
using UnityEngine;
using UnityEngine.UI;

using OscJack;

public class ChangeRecieveText : MonoBehaviour
{
    private string textValue;
    public Text textElement;


    [SerializeField] int port = 4000;
    OscServer server;

   

    void OnEnable()
    {
        server = new OscServer(port);
        server.MessageDispatcher.AddCallback(
            "/toio/1",
            (string address, OscDataHandle data) => {
              
                var stringValue = data.GetElementAsInt(0);
                var floatValue = data.GetElementAsInt(1);
                var intValue = data.GetElementAsInt(2);
                Debug.Log($"OscJack receive: {address} left {stringValue} right {floatValue} speed {intValue}");
                textValue = ($"OSC recieved from {address}, with message: left {stringValue} right {floatValue} speed {intValue}");
            }
        );

        server.MessageDispatcher.AddCallback(
            "/toio/2",
            (string address, OscDataHandle data) => {
                var stringValue = data.GetElementAsInt(0);
                var floatValue = data.GetElementAsInt(1);
                var intValue = data.GetElementAsInt(2);
                Debug.Log($"OscJack receive: {address} left {stringValue} right {floatValue} speed {intValue}");
                textValue = ($"OSC recieved from {address}, with message: left {stringValue} right {floatValue} speed {intValue}");
            }
        );
    }

      void OnDisable()
    {
        server.Dispose();
        server = null;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = textValue;
    }
}
