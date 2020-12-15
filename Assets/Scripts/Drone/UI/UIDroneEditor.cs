using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDroneEditor : MonoBehaviour {

	public static UIDroneEditor instance;

	public bool visible;

	public Drone dronePrefab;
	public Dropdown dataflowDropdown;
	public Dropdown chassisDropdown;

	public SpriteRenderer chassisSpriteRenderer;
	public Sprite wheelsSprite;
	public Sprite caterpillarSprite;
	public Sprite jetSprite;

	void Start () {
		instance = this;

		ReloadEditor();
	}

	public void ReloadEditor()
	{
		RefreshAll();

		if (visible)
			Show();
		else
			Hide();
	}
	
	public void SelectChassis() //Triggered by Chassis Dropdown Item onClick
	{
		switch ((Drone.ChassisType)chassisDropdown.value)
		{
			case Drone.ChassisType.Wheels: chassisSpriteRenderer.sprite = wheelsSprite; break;
			case Drone.ChassisType.Caterpillar: chassisSpriteRenderer.sprite = caterpillarSprite; break;
			case Drone.ChassisType.Jet: chassisSpriteRenderer.sprite = jetSprite; break;
		}
	}
	
	public void SpawnDrone()    //Triggered by SpawnButton onClick
	{
		Drone d = Instantiate(dronePrefab);
		Vector3 pos = Camera.main.transform.position + Vector3.forward * 10;
		d.transform.position = new Vector3((int)pos.x, (int)pos.y, (int)pos.z);
		d.SetChassis(chassisDropdown.value);
		if (dataflowDropdown.value > 0)
			d.SetDataflow(DataflowSave.list[dataflowDropdown.value - 1]);		
	}

	public void Spawn10Drones()    //Triggered by Spawn10Button onClick
	{
		for (int i = 0; i < 10; i++)
		{
			int x, y;
			do
			{
				x = Random.Range(0, PlanetViewer.planet.map.GetLength(0) - 1);
				y = Random.Range(0, PlanetViewer.planet.map.GetLength(1) - 1);
			} while (PlanetViewer.planet.map[x, y] == 1);

			Drone d = Instantiate(dronePrefab);
			Vector3 pos = new Vector3(x, y, 0);
			d.transform.position = new Vector3((int)pos.x, (int)pos.y, (int)pos.z);
			d.SetChassis(chassisDropdown.value);
			if (dataflowDropdown.value > 0)
				d.SetDataflow(DataflowSave.list[dataflowDropdown.value - 1]);
		}
	}

	public void RefreshChassisDropdown()
	{
		chassisDropdown.ClearOptions();
		List<string> options = new List<string>();
		foreach (var type in System.Enum.GetNames(typeof(Drone.ChassisType)))
			options.Add(type);
		chassisDropdown.AddOptions(options);
		chassisDropdown.RefreshShownValue();
	}

	public void RefreshDataflowDropdown()
	{
		dataflowDropdown.ClearOptions();
		List<string> options = new List<string> { "None" };
		foreach (var ds in DataflowSave.list)
			options.Add(ds.name);
		dataflowDropdown.AddOptions(options);
		dataflowDropdown.RefreshShownValue();
	}

	public void RefreshAll()
	{
		RefreshDataflowDropdown();
		RefreshChassisDropdown();
	}

	public void Show()
	{
		RefreshAll();
		visible = true;
		transform.localScale = Vector3.one;
	}

	public void Hide()
	{
		visible = false;
		transform.localScale = Vector3.zero;
	}

	public void ToggleVisibility()  //Triggered by DroneEditorButton onClick
	{
		if (visible)
			Hide();
		else
			Show();
	}
}
