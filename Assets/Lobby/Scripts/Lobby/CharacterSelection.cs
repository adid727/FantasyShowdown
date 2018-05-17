using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelection : NetworkBehaviour {

    [SyncVar]
    public Color colour;
    [SyncVar]
    public GameObject selectedCharacter;

    public GameObject[] roster;

    void Start () {

        if (colour == Color.red)
        {
            selectedCharacter = (GameObject)Instantiate(roster[0], transform);
        }
        if (colour == Color.cyan)
        {
            selectedCharacter = (GameObject)Instantiate(roster[1], transform);
        }
        if (colour == Color.blue)
        {
            selectedCharacter = (GameObject)Instantiate(roster[2], transform);
        }
        if (colour == Color.magenta)
        {
            selectedCharacter = (GameObject)Instantiate(roster[3], transform);
        }

        if (!isLocalPlayer)
        {
            transform.GetChild(0).GetComponent<ThirdPersonController>().enabled = false;
        }
    }

}
