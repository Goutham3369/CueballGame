using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Helper 
{
    public static IEnumerator MoveToPosition(Transform moverObj, Transform targetObj, float speed, Action onComplete = null)
    {

        while (Vector3.Distance(moverObj.position, targetObj.position) > 0.01f)
        {
            float step = speed * Time.deltaTime;
            moverObj.position = Vector3.MoveTowards(moverObj.position, targetObj.position, step);
            yield return null;
        }

        moverObj.position = targetObj.position;
        onComplete?.Invoke();
    }
    
    public static IEnumerator Bounce(Transform moverObj, float speed)
    {
        Vector3 OriginalPosition = moverObj.position;
        Vector3 TargetPosition = moverObj.position + new Vector3(0, .8f, 0);
        
        moverObj.transform.DOMoveY(TargetPosition.y,speed / 4).OnComplete(() =>
        {
            moverObj.transform.DOMoveY(OriginalPosition.y, speed / 4);
        });
        
        // while (Vector3.Distance(moverObj.position, TargetPosition) > 0.01f)
        // {
        //     float step = speed * Time.deltaTime;
        //     moverObj.position = Vector3.MoveTowards(moverObj.position, TargetPosition, step);
        //     yield return null;
        // }
        //
        // while (Vector3.Distance(moverObj.position, OriginalPosition) > 0.01f)
        // {
        //     float step = speed * Time.deltaTime;
        //     moverObj.position = Vector3.MoveTowards(moverObj.position, OriginalPosition, step);
        //     yield return null;
        // }

        moverObj.position = OriginalPosition;
        // onComplete?.Invoke();
        yield return null;
    }
    
    public static IEnumerator BounceInfinite(Transform bounceObj, float height,float speed)
    {
        Vector3 originalPosition = bounceObj.position;

        while (true)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                float yOffset = Mathf.Sin(elapsedTime * Mathf.PI) * height;
                bounceObj.position = new Vector3(originalPosition.x, originalPosition.y + yOffset, originalPosition.z);

                elapsedTime += Time.deltaTime * speed;
                yield return null;
            }

            bounceObj.position = originalPosition;
        }
    }

    public static IEnumerator Shake(Transform transformObj, float duration, float intensity)
    {
        Vector3 originalPosition = transformObj.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = originalPosition.x + Random.Range(-1f, 1f) * intensity;
            float y = originalPosition.y + Random.Range(-1f, 1f) * intensity;
            float z = originalPosition.z + Random.Range(-1f, 1f) * intensity;

            transformObj.position = new Vector3(x, y, z);
            elapsed += Time.deltaTime;

            yield return new WaitForSeconds(0.008f); 
        }

        transformObj.position = originalPosition; 
    }

}
