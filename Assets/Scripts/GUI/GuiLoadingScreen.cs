using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SocketIO;
using System;

public class GuiLoadingScreen : MonoBehaviour {

    public GameObject loadingImage;
    public Text TipText;
    public Slider progressBar;

	// Use this for initialization
	void Start () {

        TipText.text = "Hello";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
