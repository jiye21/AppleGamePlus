using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public int appleNum;

    void Start()
    {
        switch (this.name)
        {
            case "Apple_01(Clone)":
                appleNum = 1;
                break;
            case "Apple_02(Clone)":
                appleNum = 2;
                break;
            case "Apple_03(Clone)":
                appleNum = 3;
                break;
            case "Apple_04(Clone)":
                appleNum = 4;
                break;
            case "Apple_05(Clone)":
                appleNum = 5;
                break;
            case "Apple_06(Clone)":
                appleNum = 6;
                break;
            case "Apple_07(Clone)":
                appleNum = 7;
                break;
            case "Apple_08(Clone)":
                appleNum = 8;
                break;
            case "Apple_09(Clone)":
                appleNum = 9;
                break;
        }
    }

    void Update()
    {
        
    }
}
