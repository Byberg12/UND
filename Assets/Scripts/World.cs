using UnityEngine;
using System.Collections.Generic;
using SocketIO;
using Assets.Scripts.Network.Structures;
using System;

public class World : MonoBehaviour
{

    public List<Transform> characters = new List<Transform>();

    void Start()
    {
        Network.AddListener("world:movementTick", OnMovementTick);
        Network.AddListener("client:playerEnteredWorld", OnPlayerEnteredWorld);

    }

    void OnDestroy()
    {
        Network.RemoveListener("world:tick", OnWorldTick);
        Network.RemoveListener("world:movementTick", OnMovementTick);
    }

    public void OnWorldTick(SocketIOEvent e)
    {
        if (characters.Count > 0)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                // Update Net Character
                // characters[i].Update();
            }
        }
    }

    public void OnMovementTick(SocketIOEvent e)
    {
        JSONObject json = e.data;
        foreach (JSONObject netCharacter in json["characters"].list)
        {
            if (netCharacter["name"].str != GameObject.FindObjectOfType<Character>()._name)
            {
                Transform character = FindCharacter(netCharacter["name"].str);
                character.position = new Vector3(netCharacter["posX"].f, netCharacter["posY"].f, netCharacter["posZ"].f);
                character.eulerAngles = new Vector3(0, netCharacter["rot"].f, 0);
            }
        }
    }

    public void OnPlayerEnteredWorld(SocketIOEvent e)
    {
        JSONObject json = e.data;
        GameObject characterPrefab = Instantiate<GameObject>(Resources.Load<GameObject>(string.Format(
            "Characters/{0}{1}",
            json["race"].str,
            json["gender"].str
            )));

        characterPrefab.transform.position = new Vector3(json["posX"].f, json["posY"].f, json["posZ"].f);
        characterPrefab.transform.eulerAngles = new Vector3(0, json["rot"].f, 0);

        characterPrefab.name = json["name"].str;
        AddCharacter(characterPrefab.transform);

    }

    public void AddCharacter(Transform character)
    {
        characters.Add(character);
    }

    public void RemoveCharacter(Transform character)
    {
        characters.Remove(character);
    }

    public Transform FindCharacter(string sid)
    {

        return GameObject.Find(sid).transform;
    }

    public void SpawnNetCharacter(string name, string race, string _class, int level, string gender, Vector3 pos, float rot )
    {
        GameObject characterPrefab = Instantiate<GameObject>(Resources.Load<GameObject>(string.Format(
             "Characters/{0}{1}",
             race,
             gender
             )));

        characterPrefab.transform.position = pos;
        characterPrefab.transform.eulerAngles = new Vector3(0, rot, 0);
        Character character = characterPrefab.AddComponent<Character>();
        character.netCharacter = true;
       
        character.SetName(name);
        AddCharacter(characterPrefab.transform);
    }
}