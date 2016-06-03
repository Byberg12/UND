using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GuiManager : MonoBehaviour
{
    public enum GUIs
    {
        LoginWindow,
        RegisterWindow,
        CharacterSelectWindow,
        CharacterCreateWindow,
        Game
    }

    public static GUIs activeGUI;

    EventSystem system;

    #region Public
    public GuiLoginWindow guiLoginWindow;
    public GuiRegisterWindow guiRegisterWindow;
    public GuiCharacterWindow guiCharacterWindow;
    public GuiCharacterCreateWindow guiCharacterCreateWindow;

    internal string tmpUser;
    internal string tmpPass;
    #endregion

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        system = EventSystem.current;
    }

    void Update()
    {
        #region tabsystem
        if (Input.GetKeyDown(KeyCode.Tab)
            && system.currentSelectedGameObject != null
            && system.currentSelectedGameObject.GetComponent<Selectable>() != null)
        {
            Selectable next = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ?
            system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp() :
            system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(system));
                }

                system.SetSelectedGameObject(next.gameObject);
            }

            //Here is the navigating back part:
            else
            {
                next = Selectable.allSelectables[0];
                system.SetSelectedGameObject(next.gameObject);
            }

        }
        #endregion

        #region Gui Switching
        switch (activeGUI)
        {
            case GUIs.LoginWindow:
                if (GameObject.FindObjectOfType<GuiLoginWindow>() == null)
                {
                    guiLoginWindow = CreateGui("LoginWindow").GetComponent<GuiLoginWindow>();
                }
                break;
            case GUIs.RegisterWindow:
                if (GameObject.FindObjectOfType<GuiRegisterWindow>() == null)
                {
                    guiRegisterWindow = CreateGui("RegisterWindow").GetComponent<GuiRegisterWindow>();
                }
                break;
            case GUIs.CharacterSelectWindow:
                if (GameObject.FindObjectOfType<GuiCharacterWindow>() == null)
                {
                    guiCharacterWindow = CreateGui("CharacterWindow").GetComponent<GuiCharacterWindow>();

                }
                break;
            case GUIs.CharacterCreateWindow:
                if (GameObject.FindObjectOfType<GuiCharacterCreateWindow>() == null)
                {
                    guiCharacterCreateWindow = CreateGui("CharacterCreateWindow").GetComponent<GuiCharacterCreateWindow>();
                }
                break;
            case GUIs.Game:
                break;
        }
        #endregion
    }

    public GameObject CreateGui(string gui)
    {
   
        GameObject Window = Instantiate<GameObject>(Resources.Load<GameObject>("Gui/" + gui));
        Window.name = Window.name.Replace("(Clone)", "").Trim();
        Window.transform.SetParent(gameObject.transform, false);
        return Window;
    }

    public void ResetGuis()
    {
        foreach (Transform gui in transform)
        {
            Destroy(gui.gameObject);
        }

    }
}
