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
        Vector3 snipeSpotMid = new Vector3(-7.8f, 1.5f, 17f);
        Vector3 snipeSpotMidRight = new Vector3(7.8f, 1.5f, 17f);
        Vector3 snipeSpotRight = new Vector3(56f, 1.5f, 27f);
        Vector3 snipeSpotLeft = new Vector3(-50f, 1.5f, -31f);
        Vector3[] spots = { snipeSpotMid, snipeSpotMidRight, snipeSpotRight, snipeSpotLeft };
        //Easy vector3 of objectives
        Vector3 midPoint = middleObjective.transform.position;
        midPoint = new Vector3(midPoint.x - .2f, 1.5f, midPoint.z + 1f);
        Vector3 leftPoint = leftObjective.transform.position;
        Vector3 rightPoint = rightObjective.transform.position;

        //Create array of dudes
        CharacterScript[] squad = { character1, character2, character3 };
        ObjectiveScript[] points = { middleObjective, leftObjective, rightObjective };

        loadoutSet();

        //If we don't have the mid point
        if (middleObjective.getControllingTeam() != character1.getTeam())
        {
            // place sniper in position, run to cover if attacked and low on HP
            if (character1.getHP() > 50)
            {
                if (Vector3.Distance(character1.transform.position, midPoint) < 3)
                {
                    character1.rotateAngle(75f);
                }
                else
                {
                    character1.MoveChar(midPoint);
                    character1.SetFacing(midPoint);
                }
            }
            //After the sniper takes some damage
            else
            {
                //Alternate poke points
                if (Vector3.Distance(character1.transform.position, snipeSpotMid) < 1f)
                    character1.MoveChar(snipeSpotMidRight);
                else
                    character1.MoveChar(snipeSpotMid);
            }

        }
        else
        {
            // place sniper in position, run to cover if attacked and low on HP
            if (character1.getHP() > 50)
            {
                if (Vector3.Distance(character1.transform.position, midPoint) < 3)
                {
                    character1.rotateAngle(75f);
                }
                else
                {
                    character1.MoveChar(midPoint);
                    character1.SetFacing(midPoint);
                }
            }
            //After the sniper takes some damage
            else
            {
                //Alternate poke points
                if (Vector3.Distance(character1.transform.position, snipeSpotMid) < 1f)
                    character1.MoveChar(snipeSpotMidRight);
                else
                    character1.MoveChar(snipeSpotMid);
            }

        }
        //Starter positions for the people (Guy 2 to Right, Guy 3 to Left)
        if(rightObjective.getControllingTeam()!=character1.getTeam())
        {
            character2.MoveChar(new Vector3(rightPoint.x + 3f, 1.5f, rightPoint.z + 3f));
            character2.SetFacing(new Vector3(midPoint.x, 1.5f, midPoint.z));
        }
        else
        {
            character2.MoveChar(snipeSpotRight);
        }
        if(leftObjective.getControllingTeam()!=character1.getTeam())
        {
            character3.MoveChar(leftPoint);
            character3.SetFacing(new Vector3(midPoint.x, 1.5f, midPoint.z));
        }
        else
        {
            character3.MoveChar(snipeSpotLeft);
        }


        
    }

    // a simple function to track game time
    public void gameTimer()
    {
        timer += 1;
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
