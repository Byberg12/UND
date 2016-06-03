using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class GuiNewsWindow : MonoBehaviour
{
    #region Private
    private GuiManager guiManager;
    #endregion

    // Use this for initialization
    void Start () {

        guiManager = GameObject.FindObjectOfType<GuiManager>();

        Network.AddListener("client:news", News);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void News(SocketIOEvent e)
    {
      
       
    }
}
