using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public int _id;
    public Transform Spelltarget;
    public Vector3 GroundTarget;
    public string _name;
    public string _class;
    public string _race;
    public string _gender;
    public int _level;
    public Vector3 _pos;
    public float _rot;
    private float tickTimer = 0.25f;
    private float currentTick;
    private bool inGame;
    public bool netCharacter = false;
    public CharacterMovement.AnimState animState;
    private Animator anim;
    private Vector3 lastPos;

    public void setInGame(bool status)
    {
        inGame = status;
    }

    internal void Clear()
    {
        this._id = 0;
        this._name = null;
        this._class = null;
        this._race = null;
        this._gender = null;
        this._level = 0;
        this._pos = Vector3.zero;
        this._rot = 0;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentTick = tickTimer;
        transform.FindChild("CharacterName").gameObject.SetActive(false);
        if (netCharacter)
        {
            transform.FindChild("CharacterName").gameObject.SetActive(true);
            anim = GetComponent<Animator>();
        }



    }

    public void Update()
    {

        if (!netCharacter)
        {
            //local Character
            if (inGame)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 50.0f))
                    {
                        GroundTarget = hit.point;
                    }  

                } 
                if (lastPos != transform.position && currentTick <= 0)
                {
                    int animState = (int)GetComponent<CharacterMovement>().animState;
                    JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
                    data.AddField("posX", transform.position.x);
                    data.AddField("posY", transform.position.y);
                    data.AddField("posZ", transform.position.z);
                    data.AddField("rot", transform.eulerAngles.y);
                    data.AddField("a", animState);
                    Events.Character.OnMove(data);
                    lastPos = transform.position;
                    currentTick = tickTimer;
                }
                currentTick -= Time.deltaTime;
            }
        }
        else
        {
            //remote Character
            if (animState == CharacterMovement.AnimState.idle)
            {
                anim.SetFloat("forward", 0);
                anim.SetFloat("sideways", 0);
            }
            if (animState == CharacterMovement.AnimState.walkForwards)
                anim.SetFloat("forward", 6);
            if (animState == CharacterMovement.AnimState.walkBackwards)
                anim.SetFloat("forward", -6);
            if (animState == CharacterMovement.AnimState.strafeRight)
                anim.SetFloat("sideways", 6);
            if (animState == CharacterMovement.AnimState.strafeLeft)
                anim.SetFloat("sideways", -6);

            if (animState == CharacterMovement.AnimState.run)
            {
                anim.SetFloat("forward", 8);
                transform.position = Vector3.MoveTowards(transform.position, _pos, 8 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _pos, 6 * Time.deltaTime);
            }

        }
    }
    public void OnLevelWasLoaded(int level)
    {
        //TODO Set correct bone as target
        if (!netCharacter)
            Camera.main.GetComponent<MouseLook>().target = transform.FindChild("CameraTarget");
    }

    internal void SetName(string name)
    {
        gameObject.name = name;

        transform.FindChild("CharacterName/NameText").GetComponent<Text>().text = name;
    }
}
