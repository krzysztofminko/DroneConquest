using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour {

	public static Bullets instance;

	public List<GameObject> active = new List<GameObject>();
	public List<GameObject> sleeping = new List<GameObject>();

	public GameObject prefab;
	public int prepareOnAwake;
	public float speed = 20.0f;

	private void Awake()
	{
		instance = this;
		for (int i = 0; i < prepareOnAwake; i++)
		{
			GameObject go = Instantiate(prefab, transform);
			go.SetActive(false);
			sleeping.Add(go);
		}
	}

	void Update () {
		for (int i = 0; i < active.Count; i++)
		{
			
			Vector3 end = active[i].transform.position + active[i].transform.forward * speed * Time.deltaTime;
			RaycastHit hit;			
			if (Physics.Linecast(active[i].transform.position, end, out hit, LayerMask.GetMask("Drone")))
			{
				sleeping.Add(active[i]);
				active[i].SetActive(false);
				active.RemoveAt(i);
			}
			else
			{
				active[i].transform.position = end;
			}
		}
	}

	public void New(Vector3 position, Vector3 lookAtPosition)
	{
		if (sleeping.Count == 0)
			sleeping.Add(Instantiate(prefab, transform));

		GameObject go = sleeping[sleeping.Count - 1];
		sleeping.RemoveAt(sleeping.Count - 1);
		active.Add(go);
		go.SetActive(true);
		go.transform.position = position;
		go.transform.LookAt(lookAtPosition, Vector3.up);
	}
}
