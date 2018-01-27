using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	// Use this for initialization

	Vector2 mouseLook;
	Vector2 smoothV;

	public float sensitivity = 5.0f;
	public float smoothing = 2.0f;
	Transform myCam;

	GameObject character;

	void Start () {
		character = this.transform.parent.gameObject;
		myCam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 mousePos = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));

		mousePos = Vector2.Scale (mousePos, new Vector2 (sensitivity * smoothing, sensitivity * smoothing));
		smoothV.x = Mathf.Lerp (smoothV.x, mousePos.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, mousePos.y, 1f / smoothing);
		mouseLook += smoothV;
		mouseLook.y = Mathf.Clamp (mouseLook.y, -90f, 90f);

		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
  		transform.localRotation = Quaternion.AngleAxis (mouseLook.x, Vector3.up);

//		Vector2 mousePos = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
//		mouseLook += mousePos;
//		myCam.Rotate (-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);
//		myCam.localEulerAngles = new Vector3 (myCam.localEulerAngles.x, myCam.localEulerAngles.y, 0f);
//		character = this.transform.parent.gameObject;
//		character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up);

	}
}
