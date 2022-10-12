using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{


    [SerializeField] GameObject Cube1;
    [SerializeField] GameObject Cube2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {

            
            Cube1.transform.position=new Vector3(0.5044644f,8.567236e-06f,-0.002451417f);
            Cube1.transform.eulerAngles = new Vector3(-0.096f, -86.97f, 0.098f);


            Cube2.transform.position=new Vector3(-0.504877925f,8.54860991e-06f,-0.00141326897f);
            Cube2.transform.eulerAngles = new Vector3(-0.096f, 93.046f, 0.098f);
            
            
        }
        
        
    }
}
