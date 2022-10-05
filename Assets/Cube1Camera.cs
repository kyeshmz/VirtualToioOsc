using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1Camera : MonoBehaviour
{

    public Transform mainCamera;
    public GameObject player;
    void Start()

    {
        mainCamera = Camera.main.transform.parent.transform;
    }
    void Update()
    {
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
    }
}
