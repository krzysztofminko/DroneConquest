using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : MonoBehaviour {

	public enum Type { Path}

	public class Query
	{
		public Type type;
	}

	public List<Query> queries = new List<Query>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
