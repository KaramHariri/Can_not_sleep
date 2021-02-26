using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float lerpTime = 0.7f;
    private float zoomTime = 1.2f;

    private float xBounds = 5.0f; 
    private float yBounds = 2.5f;

    int currentQuadrant = 0;
    void Start()
    {
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    ActivateMoveBackground();
        //}
    }

    public void ActivateMoveBackground()
    {
        StopAllCoroutines();
        StartCoroutine(MoveBackground());
    }

    IEnumerator MoveBackground()
    {
        float zoomInCurrentTime = 0f;
        while (zoomInCurrentTime < zoomTime)
        {
            zoomInCurrentTime += Time.deltaTime;
            if (zoomInCurrentTime > zoomTime) { zoomInCurrentTime = zoomTime; }
            //Camera.main.rect = new Rect(0, 0, CubicEaseIn(zoomInCurrentTime, 0.5f, 1f, zoomTime), CubicEaseIn(zoomInCurrentTime, 0.5f, 1f, zoomTime));
            Camera.main.orthographicSize = CubicEaseOut(zoomInCurrentTime, 4.1f, 0.9f, zoomTime);
            yield return null;
        }

        float currentTime = 0f;

        int randomQuad = currentQuadrant + Random.Range(1, 4);
        //Debug.Log("Current = " + currentQuadrant + " - New = " + (randomQuad % 4));
        currentQuadrant = randomQuad % 4;

        float randomX = 0;
        float randomY = 0;
        randomX = Random.Range(0.3f, xBounds);
        randomY = Random.Range(0.3f, yBounds);

        Vector3 currentPosition = transform.position;

        switch (currentQuadrant)
        {
            case 0:
            {
                //Quad1
                //Nothing should change;
            }break;

            case 1:
            {
                //Quad2
                randomX = -randomX;
            }
            break;

            case 2:
            {
                //Quad3
                randomX = -randomX;
                randomY = -randomY;
            }
            break;

            case 3:
            {
                //Quad4
                randomY = -randomY;
            }
            break;

            default:
                Debug.Log("Default switch -> Should not happen!");
                break;
        }

        Vector3 destinationPosition = new Vector3(randomX, randomY, transform.position.z);

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > lerpTime) { currentTime = lerpTime; }
            transform.position = Vector3.Lerp(currentPosition, destinationPosition, (currentTime / lerpTime));
            yield return null;
        }

        float zoomOutCurrentTime = 0f;
        while (zoomOutCurrentTime < zoomTime)
        {
            zoomOutCurrentTime += Time.deltaTime;
            if (zoomOutCurrentTime > zoomTime) { zoomOutCurrentTime = zoomTime; }
            //Camera.main.rect = new Rect(0,0,CubicEaseIn(zoomOutCurrentTime,1,0.5f,zoomTime), CubicEaseIn(zoomOutCurrentTime, 1, 0.5f, zoomTime));
            Camera.main.orthographicSize = CubicEaseOut(zoomOutCurrentTime, 5f, -0.9f, zoomTime);
            yield return null;
        }

    }

    float CubicEaseIn( float t, float b, float c, float d )
    {
        return c * (t /= d) * t * t + b;
    }
    float CubicEaseOut( float t, float b, float c, float d )
    {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    }
}
