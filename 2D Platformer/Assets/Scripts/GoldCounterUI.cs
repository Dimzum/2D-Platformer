using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GoldCounterUI : MonoBehaviour {

    [SerializeField] private Text goldText;

    // Use this for initialization
    void Start() {
        //goldText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        goldText.text = "x " + GameMaster.Gold.ToString();
    }
}
