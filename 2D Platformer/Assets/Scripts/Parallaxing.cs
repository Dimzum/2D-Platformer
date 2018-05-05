using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {
    
    public Transform[] backgrounds; // Array of all the back and foregrounds to be parallaxed
    private float[] parallaxScales; // The Proportion of the camera's movement to move the background by
    public float smoothing = 1f; // How smooth the parallax is going to be. Set above 0.

    private Transform cam; // Reference to the main camera's transform
    private Vector3 previousCamPos; // The position of the camera in the previous frame

    void Awake () {
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start () {
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
            // the parallax is the opposite of the camera movement because the previous frame multiplied by teh scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            // set a target x pos which is the current pos + the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            // create a target pos which is the background's current pos with it's target x pos
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            // fade between curr pos and the target pos using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
	}
}
