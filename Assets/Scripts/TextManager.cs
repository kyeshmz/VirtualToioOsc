using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;//TextMeshProUGUIを使うのに必要

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Cube1_text;
    [SerializeField] TextMeshProUGUI Cube2_text;
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(Cube1_text.enabled==true){
                Cube1_text.enabled = false;
            }else{
                Cube1_text.enabled = true;
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(Cube2_text.enabled==true){
                Cube2_text.enabled = false;
            }else{
                Cube2_text.enabled = true;
            }
            
        }
        
        
    }
}