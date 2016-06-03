using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using SocketIO;
using System.Collections.Generic;
using System;

public struct client
{
    public static string socket_id;
    public static int account_id;
    public static string email;
    public static bool loggedIn = false;
}

public class character
{
    public int id;
    public int account_id;
    public string name;
    public string _class;
    public int level;
    public character(int id, int account_id, string name, int level, string _class)
    {
        this.id = id;
        this.account_id = account_id;
        this.name = name;
        this.level = level;
        this._class = _class;
    }
}
public class ClientManager : MonoBehaviour
{
    public SocketIOComponent socket;
    public string loginerror;
    public List<character> characters = new List<character>();



    public Text VersionText;
    public GameObject NewsWindow;
    public GameObject CharacterSelectWindow;
    public GameObject CreateCharacterBtn;
    public Button CharacterSelectBtn;
    public GameObject EnterWorldBtn;
    public GameObject LogoutBtn;
    public Button QuitBtn;

    public InputField UserInput;
    public InputField PassInput;
    public float heartbeat = 0.5f;
    private float curr_heartbeat;



    // Use this for initialization

    void OnLogin()
    {

        Dictionary<string, string> data = new Dictionary<string, string>();
        data["user"] = UserInput.text;
        data["pass"] = Utilities.CreateMD5(PassInput.text);
        socket.Emit("server:login", new JSONObject(data));



    }

    public void Start()
    {
       
        VersionText.text = "Version: 0.0.3";
        CharacterSelectWindow.SetActive(false);
        EnterWorldBtn.SetActive(false);
        LogoutBtn.SetActive(false);
        NewsWindow.SetActive(true);
        CreateCharacterBtn.SetActive(false);


        QuitBtn.onClick.AddListener(() => Application.Quit());

        curr_heartbeat = heartbeat;
        socket.Connect();
        socket.On("client:connect", Connect);
        socket.On("client:login", Login);
        socket.On("client:logout", Logout);
        socket.On("client:disconnect", Disconnect);
        socket.On("client:characters", Characters);
    }

    // Update is called once per frame
    void Update()
    {

   

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 pos = new Vector3(2.5f, 2.5f, 1.4f);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["posX"] = pos.x.ToString();
            data["posY"] = pos.y.ToString();
            data["posZ"] = pos.z.ToString();
            if (curr_heartbeat <= 0)
            {
                socket.Emit("server:moving", new JSONObject(data));
                curr_heartbeat = heartbeat;
            }

            curr_heartbeat -= Time.deltaTime;

        }

    }

    public void OnGUI()
    {

        if (!client.loggedIn)
        {
            GUILayout.Label(loginerror);

        }
        else
        {

            if (GUILayout.Button("Logout"))
            {
                socket.Emit("logout", socket.sid);

            }
        }

    }

    public void Connect(SocketIOEvent e)
    {
        client.socket_id = socket.sid;
        Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
    }

    private void Login(SocketIOEvent e)
    {
        System.Boolean.TryParse(e.data.GetField("loggedIn").ToString(), out client.loggedIn);
        Debug.Log(client.loggedIn);
        if (!client.loggedIn)
        {
  
          
        }
        else
        {
           

            NewsWindow.SetActive(false);
            CharacterSelectWindow.SetActive(true);
            CreateCharacterBtn.SetActive(true);
            LogoutBtn.SetActive(true);
            loginerror = "";
            socket.Emit("server:requestCharacters");
        }
        Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
    }

    private void Logout(SocketIOEvent e)
    {
        client.loggedIn = false;
        characters.Clear();
        Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
    }

    private void Disconnect(SocketIOEvent e)
    {
    }

    private void Characters(SocketIOEvent e)
    {
        JSONObject _characters = e.data.GetField("characters");
        for (int i = 0; i <= _characters.Count - 1; i++)
        {

            int id; System.Int32.TryParse(_characters[i].GetField("id").ToString(), out id);
            int account_id; System.Int32.TryParse(_characters[i].GetField("account_id").ToString(), out account_id);
            string name = _characters[i].GetField("name").ToString().Trim('"');
            string _class = _characters[i].GetField("class").ToString().Trim('"');
            int level; System.Int32.TryParse(_characters[i].GetField("level").ToString(), out level);
            characters.Add(new character(id, account_id, name, level,_class));
        }

        foreach (character character in characters)
        {
            Debug.Log("Clicked Character: " + character.name);

            // Dictionary<string, string> data = new Dictionary<string, string>();
            //   data["id"] = character.id.ToString();
            // socket.Emit("server:selectCharacter", new JSONObject(data));

            GameObject CharSelect = (GameObject)Instantiate(Resources.Load<GameObject>("GUI/CharacterSelectBtn"));
            CharSelect.transform.SetParent(CharacterSelectWindow.transform.FindChild("Background"));
            CharSelect.transform.FindChild("CharacterName").GetComponent<Text>().text = character.name;
            CharSelect.transform.FindChild("CharacterLevel").GetComponent<Text>().text =string.Format("Level {0} {1}", character.level.ToString(), character._class) ;

            CharSelect.GetComponent<Button>().onClick.AddListener(() => OnCharSelect(character.id));
        }


        Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
    }

    private void OnCharSelect(int id)
    {
        EnterWorldBtn.SetActive(true);
    }

    IEnumerator Wait(float duration)
    {
        //This is a coroutine
        Debug.Log("Start Wait() function. The time is: " + Time.time);
        Debug.Log("Float duration = " + duration);
        yield return new WaitForSeconds(duration);   //Wait
        Debug.Log("End Wait() function and the time is: " + Time.time);
    }

}