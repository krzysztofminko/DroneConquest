using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public enum State { Idle};

	public Drone drone;
	public Source source;
	//Alien
	//Construction

	public State state;

	public float health;
	public float healthMax;
	public float energy;
	public float energyMax;
	public float damage;

	private void Awake()
	{
		drone = GetComponent<Drone>();
	}
}
