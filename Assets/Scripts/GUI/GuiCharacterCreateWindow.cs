using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuiCharacterCreateWindow : GuiBase
{
    #region Private
    private GuiManager guiManager;
    private string preGender, preRace;
    bool previewReady = false;
    Character character;
    GameObject PreviewCharacter;
    #endregion

    #region Public
    public Text textHeader;
    public Text textStatus;
    public InputField inputName;
    public ToggleGroup classes;
    public ToggleGroup gender;
    public ToggleGroup race;
    public Button buttonCreate;
    public Button buttonCancel;

    //these needed but without some other name? perhaps?




    #endregion

    // Use this for initialization
    void Start()
    {
        previewReady = false;
        CameraPos = GameObject.Find("CameraPos2").transform;
        CameraViewPos = CameraPos.FindChild("View");

        print(this.name + ".Start");
        guiManager = GameObject.FindObjectOfType<GuiManager>();

        buttonCreate.onClick.AddListener(() => OnCreateClick());
        buttonCancel.onClick.AddListener(() => OnCancelClick());

        Events.Account.OnRequestClassesRaces();

        base.Start();
    }

    // Cleanup
    void OnDestroy()
    {
        if (PreviewCharacter != null)
        {
            Destroy(PreviewCharacter);
        }
        buttonCreate.onClick.RemoveAllListeners();
        buttonCancel.onClick.RemoveAllListeners();
    }

    public void SetupClassesRaces(List<JSONObject> races, List<JSONObject> classes)
    {
      
        int i = 0;

        foreach (JSONObject _race in races)
        {
            GameObject toggleRace = Instantiate<GameObject>(Resources.Load<GameObject>("Gui/ToggleRace"));
            toggleRace.transform.SetParent(this.race.transform);
            toggleRace.transform.FindChild("Label").GetComponent<Text>().text = _race["name"].str;
            toggleRace.transform.FindChild("Background").FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + _race["icon"].str);
            toggleRace.transform.FindChild("Background").FindChild("Checkmark").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + _race["icon"].str);
            toggleRace.GetComponent<Toggle>().group = this.race.GetComponent<ToggleGroup>();
            if (i == 0)
                toggleRace.GetComponent<Toggle>().isOn = true;
            i++;
        }


        i = 0;
        foreach (JSONObject _class in classes)
        {
            GameObject toggleClass = Instantiate<GameObject>(Resources.Load<GameObject>("Gui/ToggleClass"));
            toggleClass.transform.SetParent(this.classes.transform);
            toggleClass.transform.FindChild("Label").GetComponent<Text>().text = _class["name"].str;
            toggleClass.transform.FindChild("Background").FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + _class["icon"].str);
            toggleClass.transform.FindChild("Background").FindChild("Checkmark").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + _class["icon"].str);
            toggleClass.GetComponent<Toggle>().group = this.classes.GetComponent<ToggleGroup>();

            if (i == 0)
                toggleClass.GetComponent<Toggle>().isOn = true;
            i++;
        }

        previewReady = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (previewReady && ((gender.GetActive().transform.FindChild("Label").GetComponent<Text>().text != preGender) || (race.GetActive().transform.FindChild("Label").GetComponent<Text>().text != preRace)))
        {
            preGender = gender.GetActive().transform.FindChild("Label").GetComponent<Text>().text;
            preRace = race.GetActive().transform.FindChild("Label").GetComponent<Text>().text;
            UpdatePreview();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnCreateClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancelClick();
        }
        base.Update();
    }

    void OnCreateClick()
    {
        print(this.name + ".OnRegisterClick");

        List<string> missingFields = new List<string>();
        missingFields.ToArray();

        if (inputName.text == "")
            missingFields.Add("name");

        textStatus.text = "Please provide \n" + string.Join(", ", missingFields.ToArray());

        if (inputName.text != "")
        {
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            data.AddField("name", inputName.text);
            data.AddField("class", classes.GetActive().transform.FindChild("Label").GetComponent<Text>().text);
            data.AddField("gender", gender.GetActive().transform.FindChild("Label").GetComponent<Text>().text);
            data.AddField("race", race.GetActive().transform.FindChild("Label").GetComponent<Text>().text);

            // Try it on server
            Events.Account.OnCreateCharacter(data);
        }
    }

    void OnCancelClick()
    {
        if (PreviewCharacter != null)
        {
            Destroy(PreviewCharacter);
        }
        GuiManager.activeGUI = GuiManager.GUIs.CharacterSelectWindow;
        Destroy(gameObject);
    }

    public void CreateCharacter(bool exist)
    {

        if (exist)
        {
            textStatus.text = "Character name taken, try another";
        }
        else
        {
            if (PreviewCharacter != null)
            {
                Destroy(PreviewCharacter);
            }
            textStatus.text = "Character Creation Successfull";

            GuiManager.activeGUI = GuiManager.GUIs.CharacterSelectWindow;

            Destroy(gameObject);
        }
    }



    void UpdatePreview()
    {

        //TODO add rotation and zoom!
        if (PreviewCharacter != null)
        {
            Destroy(PreviewCharacter);
        }

        GameObject characterPrefab = Instantiate<GameObject>(Resources.Load<GameObject>(string.Format(
            "Characters/{0}{1}",
            preRace,
            preGender
            )));

        characterPrefab.transform.position = GameObject.Find("CharacterCreateSpawn").transform.position;
        PreviewCharacter previewCharacter = characterPrefab.AddComponent<PreviewCharacter>();

        Vector3 lookPos = CameraPos.position - characterPrefab.transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        characterPrefab.transform.rotation = rotation;
       
        PreviewCharacter = characterPrefab;
        Debug.Log(preRace + preGender);
    }
}
