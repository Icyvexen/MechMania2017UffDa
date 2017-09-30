using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UffDa : MonoBehaviour {

    //private Vector3 position = new Vector3(20.0f, 0.0f, 20.0f);

    /// <summary>
    /// DO NOT MODIFY THIS! 
    /// vvvvvvvvv
    /// </summary>
    [SerializeField]
    public CharacterScript character1;
    [SerializeField]
    public CharacterScript character2;
    [SerializeField]
    public CharacterScript character3;
    /// <summary>
    /// ^^^^^^^^
    /// </summary>
    /// 


    // USEFUL VARIABLES
    private ObjectiveScript middleObjective;
    private ObjectiveScript leftObjective;
    private ObjectiveScript rightObjective;
    private float timer = 0;

    private team ourTeamColor;

    void Start()
    {
        // Set up code. This populates your characters with their controlling scripts
        character1 = transform.Find("Character1").gameObject.GetComponent<CharacterScript>();
        character2 = transform.Find("Character2").gameObject.GetComponent<CharacterScript>();
        character3 = transform.Find("Character3").gameObject.GetComponent<CharacterScript>();

        // populate the objectives
        middleObjective = GameObject.Find("MiddleObjective").GetComponent<ObjectiveScript>();
        leftObjective = GameObject.Find("LeftObjective").GetComponent<ObjectiveScript>();
        rightObjective = GameObject.Find("RightObjective").GetComponent<ObjectiveScript>();

        // save our team, changes every time
        ourTeamColor = character1.getTeam();
        //Makes gametimer call every second
        InvokeRepeating("gameTimer", 0.0f, 1.0f);

    }

    void Update()
    {
        int currentSnipe = 0;
        //Start of POIs on map
        Vector3 snipeSpotMid = new Vector3(-4.8f, 1.5f, 9.5f);
        Vector3 snipeSpotMidRight = new Vector3(4.8f, 1.5f, 9.5f);
        Vector3 snipeSpotRight = new Vector3(37f, 1.5f, -24f);
        Vector3 snipeSpotLeft = new Vector3(-38f, 1.5f, 25f);
        Vector3[] spots = { snipeSpotMid, snipeSpotMidRight, snipeSpotRight, snipeSpotLeft };
        //Easy vector3 of objectives
        Vector3 midPoint = middleObjective.transform.position;
        Vector3 leftPoint = leftObjective.transform.position;
        Vector3 rightPoint = rightObjective.transform.position;

        //Create array of dudes
        CharacterScript[] squad = { character1, character2, character3 };
        ObjectiveScript[] points = { middleObjective, leftObjective, rightObjective };

        loadoutSet();
        if(timer < 0.1)
        {
            character2.FaceClosestWaypoint();
            character2.MoveChar(midPoint);
        }
        // place sniper in position, run to cover if attacked and low on HP
        if (character1.getHP() > 50)
        {
            character1.MoveChar(midPoint - new Vector3(midPoint.x + 3,1.5f,midPoint.z+1.5f));
            character1.SetFacing(midPoint);
        }
        else
        {
            switch (currentSnipe)
            {
                case 0:
                    if(character1.isDoneMoving(1.3f))
                    {
                        currentSnipe++;
                        character1.MoveChar(spots[currentSnipe]);
                    }
                break;
                case 1:
                    if (character1.isDoneMoving(1.3f))
                    {
                        currentSnipe++;
                        character1.MoveChar(spots[currentSnipe]);
                    }
                    break;
                case 2:
                    if (character1.isDoneMoving(1.3f))
                    {
                        currentSnipe++;
                        character1.MoveChar(spots[currentSnipe]);
                    }
                    break;
                case 3:
                    if (character1.isDoneMoving(1.3f))
                    {
                        currentSnipe++;
                        character1.MoveChar(spots[currentSnipe]);
                    }
                    break;
                case 4:
                    character1.FindClosestItem();
                    currentSnipe++;
                    break;
                default:
                    currentSnipe = 0;
                    break;
            }
        }
        //If we don't have the mid point
        if (middleObjective.getControllingTeam() != character1.getTeam())
        {
            //If Guy 2 is on the point, send Guy 3 a non-team point or patrol
            if(character2.isDoneMoving(1.3f))
            {
                character2.rotateAngle(5f); //May not work
                character2.MoveChar(character2.transform.position);
                if(leftObjective.getControllingTeam() != character1.getTeam())
                {
                    character3.SetFacing(leftPoint);
                    character3.MoveChar(leftPoint);
                }
                else if (rightObjective.getControllingTeam() != character1.getTeam())
                {
                    character3.SetFacing(rightPoint);
                    character3.MoveChar(rightPoint);
                }
                else
                {
                    patrol();
                }
            }
            //If Guy 3 is on the point, send Guy 2 to non-team point or patrol
            else if(character3.isDoneMoving(1.3f))
            {
                character3.rotateAngle(5f); //May not work
                character3.MoveChar(character2.transform.position);
                if (leftObjective.getControllingTeam() != character1.getTeam())
                {
                    character2.SetFacing(leftPoint);
                    character2.MoveChar(leftPoint);
                }
                else if (rightObjective.getControllingTeam() != character1.getTeam())
                {
                    character2.SetFacing(rightPoint);
                    character2.MoveChar(rightPoint);
                }
                else
                {
                    patrol();
                }
            }
            character2.MoveChar(midPoint);
            character2.SetFacing(midPoint);
            //character3.MoveChar(midPoint-new Vector3(midPoint.x-3,1.5f));
            //character3.SetFacing(midPoint - new Vector3(midPoint.x - 3,1.5f));
        }
        else
        {
            // Then left
            if (leftObjective.getControllingTeam() != character1.getTeam())
            {
                character2.MoveChar(leftPoint);
                character2.SetFacing(leftPoint);
                character3.MoveChar(rightPoint);
                character3.SetFacing(rightPoint);
            }
            // Then RIght
            else if (rightObjective.getControllingTeam() != character1.getTeam())
            {
                character2.MoveChar(rightPoint);
                character2.SetFacing(rightPoint);
                character3.MoveChar(leftPoint);
                character3.SetFacing(leftPoint);
            }
        }

        foreach(CharacterScript x in squad)
        {
            if (x.getZone() == zone.Objective)
            {
                if(x.FindClosestObjective() == midPoint && middleObjective.getControllingTeam() != character1.getTeam() && Vector3.Distance(x.transform.position, x.FindClosestObjective()) < 4)
                {
                    x.MoveChar(x.transform.position);
                    x.rotateAngle(5f);
                }
                else if (x.FindClosestObjective() == leftPoint && leftObjective.getControllingTeam() != character1.getTeam() && Vector3.Distance(x.transform.position, x.FindClosestObjective()) < 4)
                {
                    x.MoveChar(x.transform.position);
                    x.rotateAngle(5f);
                }
                else if (x.FindClosestObjective() == rightPoint && rightObjective.getControllingTeam() != character1.getTeam() && Vector3.Distance(x.transform.position, x.FindClosestObjective()) < 4)
                {
                    x.MoveChar(x.transform.position);
                    x.rotateAngle(5f);
                }
            }
            {
                
            }
        }
    }

    // a simple function to track game time
    public void gameTimer()
    {
        timer += 1;
    }

    //Routine to go on patrol
    public void patrol()
    {

    }

    void loadoutSet()
    {
        //Set caracter loadouts, can only happen when the characters are at base.
        if (character1.getZone() == zone.BlueBase || character1.getZone() == zone.RedBase)
            character1.setLoadout(loadout.LONG);
        if (character2.getZone() == zone.BlueBase || character2.getZone() == zone.RedBase)
            character2.setLoadout(loadout.SHORT);
        if (character3.getZone() == zone.BlueBase || character3.getZone() == zone.RedBase)
            character3.setLoadout(loadout.SHORT);
    }
}
