using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class GuiLoginWindow : GuiBase
{

    #region Private
    private GuiManager guiManager;
    #endregion

    #region Public


    public Text textHeader;
    public Text textStatus;
    public InputField inputUser;
    public InputField inputPass;
    public Button buttonLogin;
    #endregion

    // Use this for initialization
    void Start()
    {
        guiManager = GameObject.FindObjectOfType<GuiManager>();
        if (guiManager.tmpUser != null && guiManager.tmpPass != null)
        {
            inputUser.text = guiManager.tmpUser;
            inputPass.text = guiManager.tmpPass;
            guiManager.tmpUser = null;
            guiManager.tmpPass = null;
        }

        buttonLogin.onClick.AddListener(() => OnLoginClick());

        CameraPos = GameObject.Find("CameraPos0").transform;
        CameraViewPos = GameObject.Find("CameraPos1").transform.FindChild("View");

        base.Start();
    }

    // Cleanup
    void OnDestroy()
    {
        buttonLogin.onClick.RemoveAllListeners();
    }

    void Update()
    {

        base.Update();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnLoginClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void OnLoginClick()
    {
        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("user", inputUser.text);
        data.AddField("pass", Utilities.CreateMD5(inputPass.text));
        Events.Account.OnLogin(data);
    }


    void OnRegisterClick()
    {
        GuiManager.activeGUI = GuiManager.GUIs.RegisterWindow;
        Destroy(gameObject);
    }

    public void Login(int status, bool loggedIn)
    {
        if (loggedIn)
        {
            textStatus.text = "Status: Logged In Successfully";
            GuiManager.activeGUI = GuiManager.GUIs.CharacterSelectWindow;
            Destroy(gameObject);
        }
        else
        {
            string error = null;
            switch (status)
            {
                case 1:
                    error = "Account Inactive";
                    break;
                case 2:
                    error = "Username / Password Incorrect";
                    break;
                case 3:
                    error = "Account Already Logged In";
                    break;
            }
            textStatus.text = "Status: " + error;
        }
    }
}