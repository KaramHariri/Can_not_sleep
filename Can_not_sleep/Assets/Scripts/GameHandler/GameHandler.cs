using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    List<GameObject> starObjects = new List<GameObject>();
    List<Star> starScripts = new List<Star>();
    public GameObject starPrefab;

    public BackgroundMover background;
    float xOffset = 5.0f;

    enum GameState
    {
        Playing,
        EndingRound,
        StartingNewRound
    }
    GameState currentState = GameState.Playing;

    void Start()
    {
        if(background == null) { Debug.Log("Background equals null"); return; }

        for(int i = 0; i < 4; i++)
        {
            if(starPrefab == null) { Debug.Log("Star Prefab equals null !"); return; }
            GameObject temp = Instantiate(starPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Star tempScript = temp.GetComponent<Star>();
            if(tempScript == null) { Debug.Log("temp StarScript equals null"); return; }

            starObjects.Add(temp);
            starScripts.Add(tempScript);

        }

        StartNewRound();
    }

    void Update()
    {
        if (currentState != GameState.Playing)
            return;

        checkRoundCondition();

        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    StartNewRound();
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    EndTheRound();
        //}


    }

    void checkRoundCondition()
    {
        bool allStarsAreActive = true;
        foreach(Star script in starScripts)
        {
            if (script.isActive() == false)
            {
                allStarsAreActive = false;
                break;
            }
        }

        if(allStarsAreActive)
        {
            currentState = GameState.EndingRound;
            EndTheRound();
        }
    }

    void StartNewRound()
    {
        StopAllCoroutines();
        StartCoroutine(BeginNewRound());
    }

    void EndTheRound()
    {
        StopAllCoroutines();
        StartCoroutine(EndRound());
    }


    void SetStarPositions()
    {
        float xBounds = 15.0f;
        float yBounds = 15.0f;


        for (int i = 0; i < 4; i++)
        {
            float randomX = Random.Range(1f, xBounds);
            float randomY = Random.Range(1f, yBounds);

            switch (i)
            {
                case 0:
                {
                    //Quad1
                    //Debug.Log("0");
                    randomX = xOffset + randomX;
                }
                break;

                case 1:
                {
                    //Quad2
                    //Debug.Log("1");
                    randomX = xOffset - randomX;
                }
                break;

                case 2:
                {
                    //Debug.Log("2");
                    //Quad3
                    randomX = xOffset - randomX;
                    randomY = -randomY;
                }
                break;

                case 3:
                {
                    //Debug.Log("3");
                    //Quad4
                    randomX = xOffset + randomX;
                    randomY = -randomY;
                }
                break;

                default:
                    Debug.Log("Default switch -> Should not happen!");
                    break;
            }
            //Debug.Log("i = " + i + " pos : " + randomX + ", "+ randomY);
            starObjects[i].transform.position = new Vector3(randomX,randomY,transform.position.z);
        }
    }

    IEnumerator BeginNewRound()
    {
        currentState = GameState.StartingNewRound;

        background.ActivateMoveBackground();
        yield return new WaitForSeconds(1.0f);
        SetStarPositions();
        foreach(Star script in starScripts)
        {
            script.ActivateAppearance();
            yield return new WaitForSeconds(0.5f);
        }

        currentState = GameState.Playing;
    }

    IEnumerator EndRound()
    {
        foreach (Star script in starScripts)
        {
            script.ActivateDisappearance();
            yield return new WaitForSeconds(0.5f);
        }

        StartNewRound();
    }

}
