using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Drone : MonoBehaviour, IPointerClickHandler
{
	public enum ChassisType : int { Wheels, Caterpillar, Jet}

	public float gathering;
	public float building;
	public float capacity;
	public float capacityMax;
	public float ammo;
	public float ammoMax;

	public float moveSpeed = 4;
	public float rotateSpeed = 180;

	public Sprite wheelsSprite;
	public Sprite caterpillarSprite;
	public Sprite jetSprite;

	public static List<Drone> list = new List<Drone>();
	
	public Planet planet;   //TODO: Add to save

	public List<Vector2> path;

	public ChassisType chassisType;
	private SpriteRenderer chassisSpriteRenderer;

	public Dataflow dataflow;
	[SerializeField]
	private string dataflowName;

	public GameObject bulletPrefab;

	[HideInInspector]
	public Target target;

	private bool positionChangeStarted;
	public bool obstacleAhead;

	private bool movingStarted;
	private bool moving;
	private Vector3 destination;

	private static int lastId;

	public Dictionary<string, float> memory = new Dictionary<string, float>();
	

	private void Awake()
	{
		lastId++;
		name = "Drone " + lastId;
		list.Add(this);
		chassisSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		target = GetComponent<Target>();

		SetChassis(chassisType);

		planet = PlanetViewer.planet;
	}

	public void Update()
    {
		if (dataflow != null)
			dataflow.Run();

		if (moving)
		{
			if (planet.walkable[(int)(transform.position.x + transform.up.x), (int)(transform.position.y + transform.up.y)])
				transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
			if (transform.position == destination)
			{
				moving = false;
				movingStarted = false;
			}
		}

		if (path != null && path.Count > 0)
		{
			if (!positionChangeStarted)
			{							
				if (planet.walkable[(int)path[path.Count - 1].x, (int)path[path.Count - 1].y])  //TODO: Untested: send drone invalid path through obstacles
				{					
					positionChangeStarted = true;
					planet.walkable[(int)transform.position.x, (int)transform.position.y] = true;
					planet.walkable[(int)path[path.Count - 1].x, (int)path[path.Count - 1].y] = false;
					obstacleAhead = false;
				}
				else
				{
					obstacleAhead = true;
				}
			}
			if (positionChangeStarted)
			{
				Vector3 newPosition = Vector3.MoveTowards(transform.position, new Vector3(path[path.Count - 1].x, path[path.Count - 1].y, transform.position.z), moveSpeed * Time.deltaTime);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, newPosition - transform.position), rotateSpeed * Time.deltaTime);
				transform.position = newPosition;
				if (transform.position.x == path[path.Count - 1].x && transform.position.y == path[path.Count - 1].y)
				{
					positionChangeStarted = false;
					path.RemoveAt(path.Count - 1);
				}
			}
		}
    }

	public bool Move(float distance)
	{
		if (!movingStarted)
		{
			movingStarted = true;
			moving = true;
			destination = transform.position + transform.up * distance;
		}
		return transform.position == destination;
		/*
		Vector3 newPosition = transform.position + transform.up * distance;
		if (planet.walkable[(int)(transform.position.x + transform.up.x), (int)(transform.position.y + transform.up.y)])
			transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
			*/
	}

	public void SetDataflow(DataflowSave ds)
	{
		if (ds != null)
		{
			dataflow = ds.ToDataflow();
			dataflow.save = ds;
			dataflow.drone = this;
			dataflowName = dataflow.name;
		}
		else
		{
			if (dataflow != null)
				dataflow.drone = null;
			dataflow = null;
			dataflowName = "";
		}
	}

	public void SetChassis(int type)
	{
		SetChassis((ChassisType)type);
	}

	public void SetChassis(ChassisType type)
	{
		chassisType = type;
		switch (chassisType)
		{
			case ChassisType.Wheels: chassisSpriteRenderer.sprite = wheelsSprite; break;
			case ChassisType.Caterpillar: chassisSpriteRenderer.sprite = caterpillarSprite; break;
			case ChassisType.Jet: chassisSpriteRenderer.sprite = jetSprite; break;
		}
			
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		UIDronePanel.instance.Show(this);
	}

	public void Shoot(Target target)
	{/*
		GameObject bullet = Instantiate(bulletPrefab);
		bullet.transform.position = transform.position + Vector3.up;
		bullet.transform.LookAt(target.transform.position + Vector3.up, Vector3.up);		*/
		Bullets.instance.New(transform.position + Vector3.up, target.transform.position + Vector3.up);
	}
}
