using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CalculateApple : MonoBehaviour
{
    Vector2 mousePositionStart;
    Vector2 mousePositionEnd;

    int score = 0;
    float time = 145;
    public int goldApple;
    public bool doAppleCheck = false;

    GameObject gm;
    GameObject inGameCanvas;
    TextMeshProUGUI timeText;
    GameObject gameOverCanvas;

    static int platform;

    int mouseFlag = 0;

    RaycastHit2D firstHit;
    RaycastHit2D secondHit;

    Collider2D firstCol;
    Collider2D secondCol;

    Collider2D[] goldCol;

    float checkTime;

    Transform goldAppleImage;
    Coroutine coroutine;
    private void Awake()
    {
        gm = GameObject.Find("GameManager");
        inGameCanvas = gm.transform.GetChild(0).gameObject;
        gameOverCanvas = gm.transform.GetChild(1).gameObject;

        goldApple = 0;
    }
    void Start()
    {
        if(Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            platform = 0;
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            platform = 1;
        }

        timeText = inGameCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        goldAppleImage = inGameCanvas.transform.Find("GoldAppleButton");
    }

    void Update()
    {
        TimeCheckBar();
        TouchCheck();

        ScoreCheck();

        GoldAppleCheck();

    }
    

    public void GetMousePos(int flag)
    {
        switch (flag)
        {
            case 0:
                mousePositionStart =
                    Camera.main.ScreenToWorldPoint(Input.mousePosition);
                break;
            case 1:
                mousePositionEnd =
                    Camera.main.ScreenToWorldPoint(Input.mousePosition);
                break;
        }

    }

    void CalculateNum()
    {
        int appleSum = 0;
        int appleCount = 0;
        Collider2D[] collisionArray = Physics2D.OverlapAreaAll(mousePositionStart, mousePositionEnd);
        foreach (var appleCollider in collisionArray)
        {
            appleSum += appleCollider.GetComponent<Apple>().appleNum;
            appleCount++;
        }
        if(appleSum == 10)
        {
            foreach (var apple in collisionArray)
                Destroy(apple.transform.gameObject);
            score += appleCount;
            if(appleCount >= 3)
            {
                goldApple++;
            }
        }

    }

    void TimeCheckBar()
    {
        time -= Time.deltaTime;
        timeText.text = (Mathf.Ceil(time)).ToString();
        inGameCanvas.transform.GetChild(0).GetComponent<Image>().fillAmount = (time) /145f ;

        if(Mathf.Ceil(time) <= 0)
        {
            gameOverCanvas.gameObject.SetActive(true);
            gameOverCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = 
                "Score: " + score.ToString();
            Destroy(gameObject);
        }
    }
    void ScoreCheck()
    {
        inGameCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = score.ToString();
        inGameCanvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = goldApple.ToString();
    }


    void TouchCheck()
    {
        switch (platform)
        {
            case 0:
                {
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Began)
                        {
                            mousePositionStart =
                            Camera.main.ScreenToWorldPoint(touch.position);
                        }
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            mousePositionEnd =
                            Camera.main.ScreenToWorldPoint(touch.position);

                            CalculateNum();
                        }
                    }
                        
                }
                break;
            case 1:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        GetMousePos(0);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        GetMousePos(1);

                        CalculateNum();
                    }
                }
                break;
        }
    }

    

    void GoldAppleCheck()
    {
        if (!doAppleCheck)
        {
            checkTime = 0;
            return;
        }
        if (checkTime == 0)
        {   //시작시 한번만 코루틴 호출하기 위함 
            coroutine = StartCoroutine(GoldAppleAnimation());
        }
        //황금사과를 누르고 6초이상 지나면 루프를 빠져나감
        checkTime+=Time.deltaTime;
        if (checkTime >= 6)
        {
            StopCoroutine(coroutine);
            goldAppleImage.GetComponent<Image>().color = new Color32(255, 212, 0, 255);
            goldApple++;
            mouseFlag = 0;
            doAppleCheck = false;
        }

        if (platform == 0)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    if (mouseFlag == 0)
                    {
                        firstCol = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(touch.position),
                            new Vector2(1f, 1f),
                            LayerMask.GetMask("Apple"));

                        if(firstCol != null)
                        {
                            mouseFlag++;
                        }
                    }
                    else
                    {
                        secondCol = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(touch.position),
                            new Vector2(1f, 1f),
                            LayerMask.GetMask("Apple"));

                        if(secondCol != null)
                        {
                            SwitchPos(firstCol, secondCol);
                            mouseFlag = 0;
                            doAppleCheck = false;

                            StopCoroutine(coroutine);
                            goldAppleImage.GetComponent<Image>().color =
                                new Color32(255, 212, 0, 255);
                        }
                    }
                }
                
            }
                
        }
        else if (platform == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mouseFlag == 0)
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    firstHit = Physics2D.Raycast(mousePos, new Vector2(1f, 1f),
                        0.5f, LayerMask.GetMask("Apple"));

                    if(firstHit.collider != null)
                    {
                        mouseFlag++;
                    }
                }
                else
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    secondHit = Physics2D.Raycast(mousePos, new Vector2(1f, 1f),
                        0.5f, LayerMask.GetMask("Apple"));

                    if(secondHit.collider != null)
                    {
                        SwitchPos(firstHit, secondHit);
                        mouseFlag = 0;
                        doAppleCheck = false;

                        StopCoroutine(coroutine);
                        goldAppleImage.GetComponent<Image>().color =
                            new Color32(255, 212, 0, 255);
                    }
                }
            }
        }

    }

    void SwitchPos(RaycastHit2D firstHit, RaycastHit2D secondHit)
    {
        Vector2 tempPos = firstHit.transform.position;
        firstHit.transform.position = secondHit.transform.position;
        secondHit.transform.position = tempPos;
    }

    void SwitchPos(Collider2D firstCol, Collider2D secondCol)
    {
        Vector2 tempPos = firstCol.transform.position;
        firstCol.transform.position = secondCol.transform.position;
        secondCol.transform.position = tempPos;
    }

    

    IEnumerator GoldAppleAnimation()
    {
        Debug.Log("GoldAnimation");

        int countTime = 0;
        while (countTime < 30)  //만일을 대비해 종료조건 추가
        {
            if (countTime % 2 == 0)
                goldAppleImage.GetComponent<Image>().color =
                    new Color32(255, 212, 0, 90);
            else
                goldAppleImage.GetComponent<Image>().color =
                    new Color32(255, 212, 0, 180);

            yield return new WaitForSeconds(0.4f);
            countTime++;
        }

        goldAppleImage.GetComponent<Image>().color =
                    new Color32(255, 212, 0, 255);
        yield break;
    }

}
