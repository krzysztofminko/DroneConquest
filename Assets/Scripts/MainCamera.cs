using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	public static bool active;
	public static MainCamera instance;

    public float movementSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float zoomSpeed = 1.0f;

    public new Camera camera;

    void Awake() {
        camera = transform.GetChild(0).GetComponent<Camera>();
		instance = this;
	}
	
	void LateUpdate () {
		if (active)
		{
			transform.Rotate(Vector3.up, -Input.GetAxis("Rotation") * rotationSpeed * Time.deltaTime);
			transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * movementSpeed * (camera.orthographicSize / 8) * Time.deltaTime;
			camera.orthographicSize -= Input.mouseScrollDelta.y * zoomSpeed * (camera.orthographicSize / 8) * Time.deltaTime;
		}
	}
}
