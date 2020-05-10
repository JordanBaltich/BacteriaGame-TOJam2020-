using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //we need a game manager to track whether the there are no player units left, to end the game as a loss,
    //and track all the control points and end the game if they are all under the players control, as a win

    public int amountOfTowersToWin;
    public int amountOfPlayerUnitsLeft;
    private GameObject[] playerUnitsLeft;

    public int amountOfControlTowersOwnedByPlayer;
    public List<GameObject> playerControlledTowers;
    private GameObject[] controlTowers;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        DecectPlayerUnits();
        DecectControlTowers();

        WinCondition();
        LoseCondition();

    }

    void DecectPlayerUnits()
    {
        playerUnitsLeft = GameObject.FindGameObjectsWithTag("PlayerControlled");
        amountOfPlayerUnitsLeft = playerUnitsLeft.Length;
        Debug.Log(amountOfPlayerUnitsLeft);
    }

    void DecectControlTowers()
    {
        controlTowers = GameObject.FindGameObjectsWithTag("ControlTower");

        for(int i = 0; i < controlTowers.Length; i++)
        {
            if(controlTowers[i].GetComponent<ControlNode>().myTeam == ControlNode.Team.blue)
            {
                if (!playerControlledTowers.Contains(controlTowers[i]))
                {
                    playerControlledTowers.Add(controlTowers[i]);
                    amountOfControlTowersOwnedByPlayer = playerControlledTowers.Count;
                }
                
            }

        }
    }


    void WinCondition()
    {
        if(amountOfControlTowersOwnedByPlayer >= amountOfTowersToWin)
        {
            Debug.Log("YouWin");
        }

    }


    void LoseCondition()
    {

        if (amountOfPlayerUnitsLeft <= 0)
        {
            //call code for what to do when player wins.
        }
    }






}
