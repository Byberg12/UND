using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GuiCharacterWindow : GuiBase
{
    #region Private
    private GuiManager guiManager;
    private Button buttonToRemove;
    public Character character;
    public Image ClassIcon;
    GameObject lastSelectedCharacter;
    #endregion

    #region Public
    public Text textTotal;
    public Text textHeader;
    public Button buttonEnterWorld;
    public Button buttonCreateCharacter;
    public List<Button> buttons = new List<Button>();
    public int maxCharacters = 7;
    #endregion

    // Use this for initialization
    void Start()
    {
        guiManager = GameObject.FindObjectOfType<GuiManager>();

        buttonEnterWorld.gameObject.SetActive(false);
        buttonCreateCharacter.onClick.AddListener(() => OnCharacterCreate());

        CameraPos = GameObject.Find("CameraPos1").transform;
        CameraViewPos = CameraPos.FindChild("View");

        Events.Account.OnRequestCharacters();

        base.Start();

    }
    // Cleanup
    void OnDestroy()
    {
        buttonCreateCharacter.onClick.RemoveAllListeners();
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnLogout();
        }
    }

    void OnLogout()
    {
        Events.Account.OnLogout();
    }

    public void SetupCharacters(List<JSONObject> characters)
    {
        if (lastSelectedCharacter != null)
        {
            Destroy(lastSelectedCharacter);
        }
        foreach (JSONObject character in characters)
        {
            int id = (int)character["id"].n;
            string name = character["name"].str;
            int level = (int)character["level"].n;
            string _class = character["class"].str;

            GameObject buttonCharacter = Instantiate(Resources.Load<GameObject>("Gui/ButtonCharacter"));
            buttonCharacter.name = buttonCharacter.name.Replace("(Clone)", "").Trim();
            buttonCharacter.transform.SetParent(gameObject.transform.FindChild("Panel"));
            buttonCharacter.transform.FindChild("TextCharacter").GetComponent<Text>().text = name;
            buttonCharacter.transform.FindChild("TextInfo").GetComponent<Text>().text = string.Format("Level {0} {1}", level, _class);
            buttonCharacter.transform.FindChild("ClassIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + _class);

            buttons.Add(buttonCharacter.GetComponent<Button>());
            buttonCharacter.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelect(id, buttonCharacter.GetComponent<Button>()));
        }

        int totalCharacters = characters.Count;
        textTotal.text = string.Format("Characters: {0} / {1}", totalCharacters, maxCharacters);

        if (totalCharacters >= maxCharacters)
        {
            buttonCreateCharacter.gameObject.SetActive(false);
        }
    }

    public void SelectCharacter(JSONObject netChar)
    {


        buttonEnterWorld.onClick.RemoveAllListeners();

        if (lastSelectedCharacter != null)
        {
            Destroy(lastSelectedCharacter);
        }

        GameObject characterPrefab = Instantiate<GameObject>(Resources.Load<GameObject>(string.Format(
            "Characters/{0}{1}",
            netChar["race"].str,
            netChar["gender"].str
            )));

        characterPrefab.transform.position = GameObject.Find("CharacterSelectSpawn").transform.position;

        character = characterPrefab.AddComponent<Character>();
        PreviewCharacter previewCharacter = characterPrefab.AddComponent<PreviewCharacter>();

        character._id = (int)netChar["id"].n;
        character._name = netChar["name"].str;
        character._class = netChar["class"].str;
        character._race = netChar["race"].str;
        character._gender = netChar["gender"].str;
        character._level = (int)netChar["level"].n;
        character._pos = new Vector3(netChar["posX"].f, netChar["posY"].f, netChar["posZ"].f);
        character._rot = netChar["rot"].f;
        characterPrefab.name = character._name;
        Vector3 lookPos = CameraPos.position - characterPrefab.transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        characterPrefab.transform.rotation = rotation;

        DontDestroyOnLoad(characterPrefab);

        lastSelectedCharacter = characterPrefab;

        buttonEnterWorld.gameObject.SetActive(true);

        buttonEnterWorld.onClick.AddListener(() => OnWorldEnter());
    }

    public void EnterWorld()
    {
        GuiManager.activeGUI = GuiManager.GUIs.Game;
        GuiManager guiManager = FindObjectOfType<GuiManager>();
        guiManager.ResetGuis();
        guiManager.CreateGui("Spellbook");
        guiManager.CreateGui("Hotkeys");
        guiManager.CreateGui("CursorSlot");

        //TODO: add loading screen

        SceneManager.LoadSceneAsync("testZone");

        lastSelectedCharacter.transform.position = character._pos;
        CharacterController cc = lastSelectedCharacter.AddComponent<CharacterController>();
        lastSelectedCharacter.AddComponent<CharacterMovement>();
        lastSelectedCharacter.transform.eulerAngles = new Vector3(0, character._rot, 0);
        cc.height = 2.0f;
        cc.center = new Vector3(0, 1, 0);
        character.setInGame(true);
        Destroy(lastSelectedCharacter.GetComponent<PreviewCharacter>());

        Destroy(gameObject);
    }

   

    public void DeleteCharacter()
    {
        buttons.Remove(buttonToRemove);
        Destroy(buttonToRemove.transform.parent.gameObject);
        //character.Clear();
        buttonEnterWorld.gameObject.SetActive(false);

    }

    private void OnCharacterCreate()
    {
        if (lastSelectedCharacter != null)
        {
            Destroy(lastSelectedCharacter);
        }
        GuiManager.activeGUI = GuiManager.GUIs.CharacterCreateWindow;
        buttonEnterWorld.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnCharacterSelect(int id, Button button)
    {
        foreach (Button otherbutton in buttons)
        {
            if (otherbutton != null)
            {
                if (otherbutton.name != "ButtonCreateCharacter" && otherbutton.name != "ButtonEnterWorld")
                {
                    otherbutton.transform.FindChild("ButtonDelete").gameObject.SetActive(false);
                    otherbutton.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.RemoveAllListeners();
                }
            }
        }
        button.transform.FindChild("ButtonDelete").gameObject.SetActive(true);
        Button deleteButton = button.transform.FindChild("ButtonDelete").GetComponent<Button>();

        deleteButton.onClick.AddListener(() => OnDelete(id, deleteButton));

        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("id", id);
        Events.Account.OnSelectCharacter(data);
    }

    private void OnDelete(int id, Button button)
    {
        buttonToRemove = button;
        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("id", id);
        Events.Account.OnDeleteCharacter(data);
        button.onClick.RemoveListener(() => OnDelete(id, button));
    }

    private void OnWorldEnter()
    {
        Events.Account.OnEnterWorld();
    }

    private void Logout()
    {
        if (lastSelectedCharacter != null)
        {
            Destroy(lastSelectedCharacter);
        }
        character.Clear();
        buttonEnterWorld.gameObject.SetActive(false);
        GuiManager.activeGUI = GuiManager.GUIs.LoginWindow;
        Destroy(gameObject);
    }

}
