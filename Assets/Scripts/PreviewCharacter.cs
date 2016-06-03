using UnityEngine;
using System.Collections;

public class PreviewCharacter: MonoBehaviour {

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

            if (Input.GetMouseButton(0))
            {
                transform.Rotate(Vector3.down, Input.GetAxis("Mouse X") * 6.0f, Space.World);
                
            }
      
    }
}
