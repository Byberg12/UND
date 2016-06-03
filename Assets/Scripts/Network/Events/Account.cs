using UnityEngine;
using SocketIO;
using System;

namespace Events
{
    public class Account : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Network.AddListener("account:onLogin", OnLogin);
            Network.AddListener("account:onRequestCharacters", OnRequestCharacters);
            Network.AddListener("account:onSelectCharacter", OnSelectCharacter);
            Network.AddListener("account:onDeleteCharacter", OnDeleteCharacter);
            Network.AddListener("account:onEnterWorld", OnEnterWorld);
            Network.AddListener("account:onGetCharactersConnected", OnGetCharactersConnected);
            Network.AddListener("account:onLogout", OnLogout);
            Network.AddListener("account:onRequestClassesRaces", OnRequestClassesRaces);
            Network.AddListener("account:onCreateCharacter", OnCreateCharacter);
            Network.AddListener("account:onCharacterExit", OnCharacterExit);
        }

        #region OnLogin Handlers
        public static void OnLogin(JSONObject data = null)
        {
            string cmd = "account:onLogin";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnLogin(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);

            FindObjectOfType<GuiLoginWindow>().Login((int)data["status"].n, data["loggedIn"].b);

        }
        #endregion

        #region OnRequestCharacters Handlers
        public static void OnRequestCharacters(JSONObject data = null)
        {
            string cmd = "account:onRequestCharacters";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnRequestCharacters(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            FindObjectOfType<GuiCharacterWindow>().SetupCharacters(data["characters"].list);
        }
        #endregion

        #region OnSelectCharacter Handlers
        public static void OnSelectCharacter(JSONObject data = null)
        {
            string cmd = "account:onSelectCharacter";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnSelectCharacter(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            FindObjectOfType<GuiCharacterWindow>().SelectCharacter(data);
        }
        #endregion

        #region OnDeleteCharacter Handlers
        public static void OnDeleteCharacter(JSONObject data = null)
        {
            string cmd = "account:onDeleteCharacter";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnDeleteCharacter(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            if (data["deleted"].b)
            {
                FindObjectOfType<GuiCharacterWindow>().DeleteCharacter();
            }
        }
        #endregion

        #region OnEnterWorld Handlers
        public static void OnEnterWorld(JSONObject data = null)
        {
            string cmd = "account:onEnterWorld";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnEnterWorld(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            if (data["isMine"].b)
            {
                OnGetCharactersConnected();
                FindObjectOfType<GuiCharacterWindow>().EnterWorld();
                Spellbook spellbook = FindObjectOfType<Spellbook>();
                Debug.Log(data["spells"].list);

                foreach (JSONObject spellData in data["spells"].list)
                {
                    GameObject characterSpell = Instantiate<GameObject>(Resources.Load<GameObject>("Spells/Spell"));
                    characterSpell.transform.SetParent(spellbook.transform.FindChild("page"));
                    Spell spell = characterSpell.GetComponent<Spell>();

                    spell.id = (int)spellData["id"].n;
                    spell.name = spellData["name"].str;
                    spell.icon = spellData["icon"].str;
                    spell.type = (Spell.Types)((int)spellData["type"].n);
                    spell.castTime = spellData["castTime"].f;
                    spell.resource = spellData["resource"].str;
                    spell.sParticles = spellData["sParticles"].str;
                    spell.dParticles = spellData["dParticles"].str;


                    spell.setupSpell();

                }

            }
            else
            {
                JSONObject netCharacter = data.GetField("character");
                string name = netCharacter["name"].str;
                string race = netCharacter["race"].str;
                string _class = netCharacter["class"].str;
                int level = (int)netCharacter["level"].n;
                string gender = netCharacter["gender"].str;
                Vector3 pos = new Vector3(netCharacter["posX"].f, netCharacter["posY"].f, netCharacter["posZ"].f);
                float rot = netCharacter["rot"].f;

                FindObjectOfType<World>().SpawnNetCharacter(name, race, _class, level, gender, pos, rot);
            }


        }
        #endregion

        #region OnGetCharactersConnected Handlers
        public static void OnGetCharactersConnected(JSONObject data = null)
        {
            string cmd = "account:onGetCharactersConnected";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnGetCharactersConnected(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            foreach (JSONObject netCharacter in data["characters"].list)
            {
                string name = netCharacter["name"].str;
                string race = netCharacter["race"].str;
                string _class = netCharacter["class"].str;
                int level = (int)netCharacter["level"].n;
                string gender = netCharacter["gender"].str;
                Vector3 pos = new Vector3(netCharacter["posX"].f, netCharacter["posY"].f, netCharacter["posZ"].f);
                float rot = netCharacter["rot"].f;

                FindObjectOfType<World>().SpawnNetCharacter(name, race, _class, level, gender, pos, rot);
            }
        }
        #endregion

        #region OnLogout Handlers
        public static void OnLogout(JSONObject data = null)
        {
            string cmd = "account:onLogout";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnLogout(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);

            GuiCharacterWindow guiCharacterWindow = FindObjectOfType<GuiCharacterWindow>();
            guiCharacterWindow.buttonEnterWorld.gameObject.SetActive(false);
            GuiManager.activeGUI = GuiManager.GUIs.LoginWindow;

            Destroy(guiCharacterWindow.gameObject);
        }
        #endregion

        #region OnRequestClassesRaces Handlers
        public static void OnRequestClassesRaces(JSONObject data = null)
        {
            string cmd = "account:onRequestClassesRaces";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnRequestClassesRaces(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            FindObjectOfType<GuiCharacterCreateWindow>().SetupClassesRaces(data["races"].list, data["classes"].list);
        }
        #endregion

        #region OnCreateCharacter Handlers
        public static void OnCreateCharacter(JSONObject data = null)
        {
            string cmd = "account:onCreateCharacter";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnCreateCharacter(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);

            FindObjectOfType<GuiCharacterCreateWindow>().CreateCharacter(data["characterExists"].b);
        }
        #endregion

        #region OnCharacterExit Handlers
        public static void OnCharacterExit(JSONObject data = null)
        {
            string cmd = "account:onCharacterExit";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else
            {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnCharacterExit(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);

            string name = data["name"].str;
            try {
                Destroy(GameObject.Find(name));
            }
            catch (Exception execption)
            {
                Debug.Log(execption);
            }



        }
        #endregion


    }
}