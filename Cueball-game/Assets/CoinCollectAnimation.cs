using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CoinCollectAnimation : MonoBehaviour
{
    public GameObject[] coinPool; // Array of coin GameObjects
    public Transform target; // The target UI element where coins move to
    public float speed = 200f; // Speed of the coin movement
    public Text tragetText;
    public Vector2 spawnOffset = new Vector2(50, 50); // Random spawn offset
    public UnityEvent OnCompletion;

    int oneCoinValue;
    int coinReached = 0;
    // Function to call when the claim button is pressed
    public void OnClaimButtonPressed(int moneyAdded)
    {
        gameObject.SetActive(true);
        oneCoinValue=moneyAdded/coinPool.Length;
        StartCoroutine(SpawnAndMoveCoins());
    }
    private void Start()
    {
        Vector2 res= new Vector2(Screen.width, Screen.height); 
        spawnOffset.x=(res.x/1080)*spawnOffset.x;
        spawnOffset.y=(res.y/1920)*spawnOffset.y;
    }
    private void OnEnable()
    {
        
        //StartCoroutine(SpawnAndMoveCoins());

    }
    private IEnumerator SpawnAndMoveCoins()
    {
        coinReached = 0;
        AudioManager.Instance.PlayCoinClaimSound();
        for (int i = 0; i < coinPool.Length; i++)
        {
            GameObject coin = coinPool[i];
            coin.transform.position = transform.position;

            
            Vector3 randPos= coin.transform.position + new Vector3(Random.Range(-spawnOffset.x, spawnOffset.x), Random.Range(-spawnOffset.y, spawnOffset.y), 0);
            coin.SetActive(true);
            StartCoroutine(MoveCoin(coin,randPos));

            
            yield return new WaitForSeconds(0.05f); // Wait before activating the next coin
        }
    }

    private IEnumerator MoveCoin(GameObject coin,Vector3 randPos)
    {
        float duration = Vector3.Distance(coin.transform.position, randPos) / speed;
        float elapsedTime = 0;
        Vector3 startPosition = coin.transform.position;

      


        coin.transform.localScale = Vector3.one * Random.Range(0.3f, 0.7f);
        while (elapsedTime < duration)
        {
            // Calculate the current proportion of time elapsed
            float t = elapsedTime / duration;
            // Apply a smooth step easing function to t
            float smoothT = t * t * (3f - 2f * t);
            // Move the coin towards the target
            coin.transform.position = Vector3.Lerp(startPosition, randPos, smoothT);

            // Scale the coin up towards Vector3.one
            coin.transform.localScale = Vector3.Lerp(coin.transform.localScale, Vector3.one, smoothT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        coin.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));

        duration = Vector3.Distance(coin.transform.position, target.position) / speed;
        elapsedTime = 0;
        startPosition = coin.transform.position;

        while (elapsedTime < duration)
        {
            // Calculate the current proportion of time elapsed
            float t = elapsedTime / duration;
            // Apply a smooth step easing function to t
            float smoothT = t * t * (3f - 2f * t);
            coin.transform.position = Vector3.Lerp(startPosition, target.position, (smoothT));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        coin.SetActive(false);
        int recenttext = int.Parse(UiObjectsContainer.Instance.coinText.text);
        recenttext += oneCoinValue;
        UiObjectsContainer.Instance.UpdateCoinsUI(recenttext); 
        coinReached++;
        if (coinReached == coinPool.Length - 1)
        {
            OnCompletion.Invoke();
            UiObjectsContainer.Instance.UpdateCoinsUI(PrefHandlre.Coins);
        }
    }
}
