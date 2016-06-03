using UnityEngine;
using System.Collections;

public class GuiBase : MonoBehaviour
{

    private bool UseCamera = false;
    protected Transform CameraPos;
    protected Transform CameraViewPos;
    private bool CameraMovement = false;
    public float CameraMoveSpeed = 40f;
    public float CameraLookSpeed = 800f;


    // Use this for initialization
    virtual public void Start()
    {
        if (CameraPos != null)
            UseCamera = true;

        if (UseCamera)
            CameraMovement = true;
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (UseCamera)
        {
            if (CameraMovement)
            {  
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CameraPos.position, Time.deltaTime * CameraMoveSpeed);
                if (Vector3.Distance(Camera.main.transform.position, CameraPos.position) <= 0.1f)
                {
                    CameraMovement = false;
                }
            }
            var targetRotation = Quaternion.LookRotation(CameraViewPos.position - Camera.main.transform.position);

            // Smoothly rotate towards the target point.
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, targetRotation, CameraLookSpeed * Time.deltaTime);
        }

    }
}
