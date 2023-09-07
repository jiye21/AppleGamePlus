using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] appleArray = new GameObject[9];
    int appleNum;

    private void Awake()
    {
        Spawn();

    }
    void Start()
    {


    }

    void Update()
    {

    }

    public void Spawn()
    {
        for (int i = 0; i < 18; i++)
        {
            appleNum = Random.Range(0, 9);
            Instantiate(appleArray[appleNum], transform.GetChild(i));
        }
    }
}
