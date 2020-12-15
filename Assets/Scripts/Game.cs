using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour {
		
	void Awake () {
		new GameData();


		MainCamera.active = true;		
	}
		
	void Update () {

	}

	public void Load()
	{
		GameData.Load("save1");
	}

	public void Save()
	{
		GameData.Save("save1");
	}
}
