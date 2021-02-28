using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    List<GameObject> starObjects = new List<GameObject>();
    List<Star> starScripts = new List<Star>();
    public GameObject starPrefab;

    public BackgroundMover background;
    float xOffset = 4.0f;
    float yOffset = 0.75f;

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
            GameObject temp = Instantiate(starPrefab, new Vector3(100, 100, 0), Quaternion.identity);
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
        float randomX = 0;
        float randomY = 0;

        for (int i = 0; i < 4; i++)
        {

            switch (i)
            {
                case 0:
                    {
                        //Quad1
                        int randomAngle = Random.Range(0, 60);
                        float xPos = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * 2.5f;
                        float yPos = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * 2.5f;

                        randomX = xPos + 4.0f + 0.2f;
                        randomY = yPos + 0.15f;
                    }
                    break;

                case 1:
                    {
                        //Quad2
                        int randomAngle = Random.Range(120, 180);
                        float xPos = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * 2.5f;
                        float yPos = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * 2.5f;

                        randomX = xPos + 4.0f - 0.2f;
                        randomY = yPos + 0.15f;

                    }
                    break;

                case 2:
                    {
                        //Quad3
                        int randomAngle = Random.Range(210, 260);
                        float xPos = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * 2.5f;
                        float yPos = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * 2.5f;

                        randomX = xPos + 4.0f - 0.3f;
                        randomY = yPos - 1.6f;

                    }
                    break;

                case 3:
                    {
                        //Quad4
                        int randomAngle = Random.Range(280, 330);
                        float xPos = Mathf.Cos(randomAngle * Mathf.Deg2Rad) * 2.5f;
                        float yPos = Mathf.Sin(randomAngle * Mathf.Deg2Rad) * 2.5f;

                        randomX = xPos + 4.0f + 0.3f;
                        randomY = yPos - 1.6f;

                    }
                    break;

                default:
                    Debug.Log("Default switch -> Should not happen!");
                    break;
            }
            //Debug.Log("i = " + i + " pos : " + randomX + ", "+ randomY);
            starObjects[i].transform.position = new Vector3(randomX, randomY, transform.position.z);
        }
    }

    IEnumerator BeginNewRound()
    {
        currentState = GameState.StartingNewRound;

        background.ActivateMoveBackground();
        yield return new WaitForSeconds(3.0f);
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
