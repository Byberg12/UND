using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {


	// Update is called once per frame
	void LateUpdate () {
        transform.LookAt(Camera.main.transform);
	}
}
