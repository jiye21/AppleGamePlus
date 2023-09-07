using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    GameObject menu;
    public GameObject appleGroup;
    public GameObject inGameCanvas;
    public GameObject gameOverCanvas;
    public GameObject TooltipCanvas01;
    public GameObject TooltipCanvas02;
    public GameObject calculate;
    GameObject appleGroupPrefab;
    GameObject calculatePrefab;


    void Start()
    {
        menu = GameObject.Find("MenuCanvas");


        CanvasScaler inGameCanvasScaler = inGameCanvas.GetComponent<CanvasScaler>();
        //Default 해상도 비율
        float fixedAspectRatio = inGameCanvasScaler.referenceResolution.x/
            inGameCanvasScaler.referenceResolution.y;

        //현재 해상도의 비율
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        //현재 해상도 가로 비율이 더 길 경우
        if (currentAspectRatio > fixedAspectRatio)
            inGameCanvasScaler.matchWidthOrHeight = 1;
        //현재 해상도의 세로 비율이 더 길 경우
        else if (currentAspectRatio < fixedAspectRatio)
            inGameCanvasScaler.matchWidthOrHeight = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }


    }

    public void HideMenu()
    {
        menu.SetActive(false);

        TooltipCanvas01.SetActive(true);
    }
    public void NextTooltip()
    {
        TooltipCanvas01.SetActive(false);
        TooltipCanvas02.SetActive(true);
    }
    public void StartGame()
    {
        TooltipCanvas02.SetActive(false);

        calculatePrefab = Instantiate(calculate);
        appleGroupPrefab = Instantiate(appleGroup);
        inGameCanvas.SetActive(true);
    }

    public void ReStart()
    {
        gameOverCanvas.SetActive(false);
        Destroy(appleGroupPrefab);

        calculatePrefab = Instantiate(calculate);
        appleGroupPrefab = Instantiate(appleGroup);
        inGameCanvas.SetActive(true);
    }

    public void Home()
    {
        Destroy(appleGroupPrefab);
        Destroy(calculatePrefab);

        inGameCanvas.SetActive(false);
        menu.SetActive(true);
    }

    public void ReLoad()
    {
        Destroy(appleGroupPrefab);
        Destroy(calculatePrefab);

        calculatePrefab = Instantiate(calculate);
        appleGroupPrefab = Instantiate(appleGroup);
    }

    public void GoldApple()
    {
        if (calculatePrefab.GetComponent<CalculateApple>().goldApple == 0) return;

        calculatePrefab.GetComponent<CalculateApple>().doAppleCheck = true;

        calculatePrefab.GetComponent<CalculateApple>().goldApple--;
    }

}
