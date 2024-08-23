using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHandler : MonoBehaviour
{
    public GameObject blockedObject;
    public Transform[] ballHolder;
    public List<Ball> balls = new();
    public List<ParticleSystem> SlotHitParticles;
    public Ball ballPrefab;
    public bool isLocked, isSlotClosed,isSlotBusy;
    public int topIndex, maxAllowedBalls = 4;

    public  Coroutine movingCoroutine;
    public ParticleSystem FullParticle;
    public DebriHandler _debriHandler;
    public static event Action onBallAdded;

    int[] colorsOfBalls;
    public bool IsFull()
    {
        return topIndex >= maxAllowedBalls;
    }
    public bool IsEmpty()
    {
        return topIndex <= 0;
    }
    public void InstantiateBalls(int[] colorIndices, bool showGrayBalls)
    {
        colorsOfBalls=new int[colorIndices.Length];
        colorsOfBalls = colorIndices;
        for (int i = 0; i < colorIndices.Length; i++)
        {
            var newBall = Instantiate(ballPrefab, ballHolder[i]);
            newBall.transform.SetParent(ballHolder[i]);
           
            if(i!=colorIndices.Length-1&& showGrayBalls && PlayerPrefs.GetInt("LevelModel", 0)!=0)
                newBall.SetColor(9);
            else
                newBall.SetColor(colorIndices[i]);
            balls.Add(newBall);
        }
        topIndex = colorIndices.Length;
    }

    public Ball GetTopBall()
    {
        return balls[topIndex - 1];
    }
    public int GetTopBallIndex()
    {
        //print("top index " + topIndex);
        return balls[topIndex - 1].ballIndex;
    }
    public void RemoveTopBall()
    {
        balls.RemoveAt(topIndex - 1);
        topIndex--;
        
        if (topIndex-1>=0&& balls[topIndex-1].ballIndex == 9)
        {
            balls[topIndex-1].SetColor(colorsOfBalls[topIndex-1]);
        }
    }
    public void AddBallToTop(Ball newBall)
    {
        balls.Add(newBall);
        newBall.transform.SetParent(ballHolder[topIndex]);
        SlotHitParticles[topIndex].Play();
        topIndex++;
        isSlotClosed = IsFull() && IsColorSorted();
        if (IsColorSorted() && IsFull())
        {
            FullParticle.Play();
            AudioManager.Instance.SortBallBlastSound();
            for (int i = 0; i < balls.Count; i++)
            {
                Vector3 OriginalPosition = balls[i].transform.position;
                Vector3 EndValue = balls[i].transform.position + new Vector3(0,0.1f,0);
                StartCoroutine(Helper.Bounce(balls[i].transform, .5f + i * .25f));
                // StartCoroutine(Helper.Bounce(balls[i].transform, 50 * (i + 1)));
            }
        }
        onBallAdded?.Invoke();
    }

    public bool IsColorSorted()
    {
        return balls.Select(ball => ball.ballIndex).Distinct().Count() == 1;
    }

    private void OnMouseDown()
    {
        // Check if the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // The pointer is over a UI element, exit the method to prevent further execution
            return;
        }
        if (!LevelHandler.instance.isGamePaused && !isSlotClosed)
            GetBallFromSlot();
    }

    void GetBallFromSlot()
    {
       
        if (isLocked)
        {
            if (GameHandler.instance.isUsingHammer)
            {
                if(!GameHandler.instance._hammer.isActiveAndEnabled)
                GameHandler.instance._hammer.PositionHammer(transform,this);
                if (PrefHandlre.HammarTutorial == 0)
                {
                    UiObjectsContainer.Instance.FinnishHammarTutorial();
                }
                //Invoke(nameof(ActivateDebris),.15f);
            }       
        }
        else
        {
       //     FindObjectOfType<TutorialHandler>().CloseSlot();
            GameHandler.instance.SelectSlot(this);
        }
    }

    public void ActivateDebris()
    {
        //GameHandler.instance._hammer.gameObject.SetActive(false);
        _debriHandler.ActivateDebris();
        UnlockSlot();
    }
    
    public void LockSlot()
    {
        blockedObject.SetActive(true);
        isLocked = true;
        topIndex = 0;
    }

    public void UnlockSlot()
    {
        blockedObject.SetActive(false);
        isLocked = false;
        GameHandler.instance.isUsingHammer = false;
    }

}
