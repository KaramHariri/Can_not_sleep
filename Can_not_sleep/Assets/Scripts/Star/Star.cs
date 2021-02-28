using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem glowSystem;

    private float appearTime = 0.4f;

    public enum StarState 
    {
        Inactive,
        Active
    }

    private StarState previousState = StarState.Inactive; 
    [SerializeField]
    private StarState currentState = StarState.Inactive;

    void Awake()
    {
        glowSystem = transform.Find("Glow").GetComponent<ParticleSystem>();
        if (glowSystem == null) { Debug.Log("Could not find glow particle System! "); }
    }

    void Update()
    {
        if (currentState != previousState)
        {
            previousState = currentState;

            if(currentState == StarState.Active)
            {
                activateGlow();
            }
            else
            {
                deactivateGlow();
            }
        }

        //if(Input.GetKeyDown(KeyCode.U))
        //{
        //    ActivateAppearance();
        //}


        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    ActivateDisappearance();
        //}
    }

    public bool isActive()
    {
        return (currentState == StarState.Active);
    }

    public void activateGlow()
    {
        Debug.Log("Activated Glow");
        glowSystem.Play();
    }

    void deactivateGlow()
    {
        Debug.Log("Deactivated Glow");
        glowSystem.Clear();
        glowSystem.Stop();
    }

    public void ActivateAppearance()
    {
        StopAllCoroutines();
        StartCoroutine(Appear());
    }

    public void ActivateDisappearance()
    {
        deactivateGlow();
        StopAllCoroutines();
        StartCoroutine(Disappear());
    }

    IEnumerator Appear()
    {
        float currentTime = 0;
        while (currentTime < appearTime)
        {
            currentTime += Time.deltaTime;
            if(currentTime > appearTime) { currentTime = appearTime; }
            transform.localScale = new Vector3(CubicEaseIn(currentTime, 0, 1, appearTime), CubicEaseIn(currentTime, 0, 1, appearTime), 1);
            yield return null;
        }

        yield return null;
    }

    IEnumerator Disappear()
    {
        float currentTime = 0;

        while (currentTime < appearTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > appearTime) { currentTime = appearTime; }
            transform.localScale = new Vector3(CubicEaseOut(currentTime, 1, -1, appearTime), CubicEaseOut(currentTime, 1, -1, appearTime), 1);
            yield return null;
        }

        yield return null;
    }

    //Recall that t is time,b is beginning position,c is the total change in position, and d is the duration of the tween.

    float CubicEaseIn( float t, float b, float c, float d )
    {
        return c * (t /= d) * t * t + b;
    }
    float CubicEaseOut( float t, float b, float c, float d )
    {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Collider")
        {
            currentState = StarState.Active;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name == "Collider")
        {
            currentState = StarState.Inactive;
        }
    }
}
