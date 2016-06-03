using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System.Collections;
using System.Collections.Generic;

public class GuiRegisterWindow : MonoBehaviour
{
    
    #region Private
    private GuiManager guiManager;
    #endregion

    #region Public
    public Text textHeader;
    public Text textStatus;
    public InputField inputUsername;
    public InputField inputPassword;
    public InputField inputFirstName;
    public InputField inputLastName;
    public Button buttonRegister;
    public Button buttonCancel;
    #endregion

    // Use this for initialization
    void Start()
    {
        print(this.name + ".Start");
        guiManager = GameObject.FindObjectOfType<GuiManager>();

        buttonRegister.onClick.AddListener(() => OnRegisterClick());
        buttonCancel.onClick.AddListener(() => OnCancelClick());

        Network.AddListener("client:register", Register);
    }

    // Cleanup
    void OnDestroy()
    {
        buttonRegister.onClick.RemoveAllListeners();
        buttonCancel.onClick.RemoveAllListeners();
        Network.RemoveListener("client:register", Register);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnRegisterClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancelClick();
        }
    }   

    void OnRegisterClick()
    {
        print(this.name + ".OnRegisterClick");
        List<string> missingFields = new List<string>();
        missingFields.ToArray();
        if (inputUsername.text == "")
            missingFields.Add("username");
        if (inputPassword.text == "")
            missingFields.Add("password");
        if (inputFirstName.text == "")
            missingFields.Add("first name");
        if (inputLastName.text == "")
            missingFields.Add("last name");

        textStatus.text = "Please provide \n" + string.Join(", ", missingFields.ToArray());

        if (textStatus.text != "" && inputPassword.text != "" && inputFirstName.text != "" && inputLastName.text != "")
        {
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            data.AddField("username", inputUsername.text);
            data.AddField("password", Utilities.CreateMD5(inputPassword.text));
            data.AddField("firstname", inputFirstName.text);
            data.AddField("lastname", inputLastName.text);

            // Try it on server
            Network.Send("server:register", data);
        }
    }

    void OnCancelClick()
    {
        GuiManager.activeGUI = GuiManager.GUIs.LoginWindow;
        Destroy(gameObject);
    }

    void Register(SocketIOEvent e)
    {
        Debug.Log(e);
        if (e.data["accountExists"].b == true)
        {
            textStatus.text = "Username already taken, try another";
        }
        else
        {
            textStatus.text = "Registration Successfull";
            GuiManager.activeGUI = GuiManager.GUIs.LoginWindow;
            guiManager.tmpUser = inputUsername.text;
            guiManager.tmpPass = inputPassword.text;
            Destroy(gameObject);
        }
    }
}
