using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField]
    Text textScore; 

    [SerializeField]
    Text textSpeed; 
    
    [SerializeField]
    Text textLines; 

    //обновление счета игры
    public void TextForScore(int scr)
    {
        textScore.text = scr.ToString();
    }

    //обновление скорости
    public void TextForSpeed(int spd)
    {
        textSpeed.text = spd.ToString();
    }

    //обновление линий 
    public void TextForLines(int ln)
    {
        textLines.text = ln.ToString();
    }
}
