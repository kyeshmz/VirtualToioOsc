using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1Camera : MonoBehaviour
{

   public Transform cameraPosition;
   
    private void Update()
    {
        
        transform.position = new Vector3(cameraPosition.position.x, cameraPosition.position.y, 0);
        // transform.localEulerAngles = new Vector3(cameraPosition.rotation.x,0,0);
        // transform.LookAt(cameraPosition.position.x, cameraPosition.position.y, 0);
        transform.rotation = cameraPosition.rotation;
        // transform.rotation = Quaternion.Euler(cameraPosition.rotation.x,0,0);
        transform.rotation = Quaternion.Euler(0,cameraPosition.rotation.y *45 ,0);

        // transform.rotation = new Vector3(transform.rotation.x, cameraPosition.rotation.y, transform.rotation.z);
    }
}
