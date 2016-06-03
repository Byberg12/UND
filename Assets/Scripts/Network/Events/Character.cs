using UnityEngine;
using System.Collections;
using SocketIO;
using System;

namespace Events
{
    public class Character : MonoBehaviour
    {
        
        // Use this for initialization
        void Start()
        {
            Network.AddListener("character:onMove", OnMove);
        }

        #region OnMove Handlers
        public static void OnMove(JSONObject data = null)
        {
            string cmd = "character:onMove";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnMove(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);

            Transform netCharacter = GameObject.Find(data["name"].str).transform;
            netCharacter.position = new Vector3(data["posX"].f, data["posY"].f, data["posZ"].f);
            netCharacter.eulerAngles = new Vector3(0, data["rot"].f, 0);
            netCharacter.GetComponent<global::Character>().animState = (CharacterMovement.AnimState)((int)data["a"].n);
        }
        #endregion
    }
}
