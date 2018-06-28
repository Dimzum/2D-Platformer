using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LifeCounterUI : MonoBehaviour {

    [SerializeField] private Text lifeText;

	// Use this for initialization
	void Start () {
        //lifeText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        lifeText.text = "x " + GameMaster.NumLives.ToString();
	}
}
