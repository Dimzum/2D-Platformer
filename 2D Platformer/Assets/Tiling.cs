using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tiling : MonoBehaviour {

    public int offsetX = 2; // offset to avoid errors

    // check if instantiation is needed
    public bool hasRightBuddy = false;
    public bool hasLeftBuddy = false;

    public bool reverseScale = false; // used if the object is not tilable

    private float spriteWidth = 0f; // width of the element
    private Camera cam;
    private Transform myTransform;

    private void Awake() {
        cam = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasLeftBuddy == false || hasRightBuddy == false) {
            // calculate the camera's extend (half the width) of what the camera can see in world coord
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
            // calcualte the x pos where the camera can see the edge of the sprite (element)
            float edgeVisiblePosRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePosLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            // checking if we can see the edge of the element
            // if edge is visible then call MakeNewBuddy
            if (cam.transform.position.x >= edgeVisiblePosRight - offsetX && hasRightBuddy == false) {
                MakeNewBuddy(1);
                hasRightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePosRight + offsetX && hasLeftBuddy == false) {
                MakeNewBuddy(-1);
                hasLeftBuddy = true;
            }
        }
	}

    // Creates a new buddy
    void MakeNewBuddy(int rightOrLeft) { // 1 or -1 value
        // calculating the new pos for the new buddy
        Vector3 newPos = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // instantiating a new buddy
        Transform newBuddy = Instantiate(myTransform, newPos, myTransform.rotation) as Transform;

        // if not tilable, reverse the x of the object
        if (reverseScale) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0) {
            newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
        }
        else {
            newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
        }
    }
}
