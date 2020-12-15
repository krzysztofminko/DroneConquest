using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIDataflowEditor : MonoBehaviour, IPointerClickHandler
{
	public static UIDataflowEditor instance;

	public bool visible;

	public float nodeOutlineTime;

    public Dataflow dataflow;
	public Drone drone;

	public GameObject linkPrefab;
    public GameObject nodePrefab;
    public GameObject inputPrefab;
    public GameObject outputPrefab;
	public GameObject configPrefab;
	public GameObject addNodePrefab;
	public GameObject nodeCategoryPrefab;

	public RectTransform cursor;
	public RectTransform panel;
	public Transform addNodeList;
	public Dropdown dataflowDropdown;
	public Dropdown droneDropdown;
	public Button saveButton;
	public Button deleteButton;
	public GameObject lockPanel;

	public DataflowOutput newLinkOutput;
	public DataflowInput newLinkInput;

	UINodeLink uiLinkTmp;
	List<string> categories = new List<string>();
	
	void Start () {
		instance = this;

		//Add Node Menu
		foreach (var nt in System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(DataflowNode)))
		{
			string[] path = nt.ToString().Split('.');
			if (!categories.Contains(path[path.Length - 2]))
				categories.Add(path[path.Length - 2]);
		}
		foreach(var c in categories)
		{
			GameObject go;
			go = Instantiate(nodeCategoryPrefab, addNodeList);
			go.name = c;
			go.transform.GetChild(1).GetComponent<Text>().text = c;
			go.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SelectCategory(c, go.GetComponent<Toggle>()); });
			go.GetComponent<Toggle>().group = addNodeList.GetComponent<ToggleGroup>();
		}
		foreach (var nt in System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(DataflowNode)))
		{
			GameObject go;
			string[] path = nt.ToString().Split('.');
			go = Instantiate(addNodePrefab, addNodeList);			
			go.name = path[path.Length - 2] + "." + path[path.Length - 1];
			go.transform.GetChild(0).GetComponent<Text>().text = path[path.Length - 1];
			go.GetComponent<Button>().onClick.AddListener(delegate { DrawNode(dataflow.AddNode(go.name)); });
			go.SetActive(false);
		}

		ReloadEditor();
	}

	private void Update()
	{	
		for (int i = 0; i < dataflow.nodes.Count; i++)
			dataflow.nodes[i].transform.GetComponent<Outline>().enabled = (dataflow != null && drone != null) && (dataflow.nodes[i].running || (Time.time - dataflow.nodes[i].lastRun < nodeOutlineTime));
	}

	private void SelectCategory(string name, Toggle toggle)		//Triggered by node category toggle
	{
		for(int i = categories.Count; i < addNodeList.childCount; i++)
		{
			string[] path = addNodeList.GetChild(i).name.ToString().Split('.');
			if(toggle.isOn)
				addNodeList.GetChild(i).gameObject.SetActive(path[path.Length - 2] == name);
			else
				addNodeList.GetChild(i).gameObject.SetActive(false);
		}
	}

	public void ReloadEditor()
	{
		//Open first/New
		if (DataflowSave.list.Count > 0)
			OpenDataflow(DataflowSave.list[0]);
		else
			NewDataflow();

		//Show/Hide
		if (visible)
			Show();
		else
			Hide();
	}

	public void RefreshDataflowDropdown()
	{
		dataflowDropdown.ClearOptions();
		List<string> options = new List<string>();
		foreach (var ds in DataflowSave.list)
			options.Add(ds.name);
		dataflowDropdown.AddOptions(options);
		dataflowDropdown.value = DataflowSave.list.FindIndex(ds => ds.name == dataflow.name);
		dataflowDropdown.RefreshShownValue();
	}

	public void RefreshDroneDropdown()
	{
		droneDropdown.ClearOptions();
		List<string> options = new List<string> { "<i>Select drone</i>" };
		foreach (var dr in Drone.list.Where(d => d.dataflow != null))
			options.Add(dr.name);
		droneDropdown.AddOptions(options);
		droneDropdown.value = drone? Drone.list.Where(d => d.dataflow != null).ToList().FindIndex(d => d.name == drone.name) + 1 : 0;
		droneDropdown.RefreshShownValue();
	}

	public void OpenDrone()	//Triggered by DroneDropdown Item
	{
		if (droneDropdown.value > 0)
			OpenDrone(Drone.list.Find(d => d.name == droneDropdown.options[droneDropdown.value].text));
		else
			CloseDrone();
	}

	public void OpenDrone(Drone drone)
	{
		if (dataflow != null || drone != null)
			CloseDataflow();
		
		this.drone = drone;

		SetReadonly(true);

		OpenDataflow(drone.dataflow);
	}

	public void CloseDrone()
	{
		if (drone)
		{
			CloseDataflow();
			OpenDataflow(drone.dataflow.save);
			drone = null;
		}
	}

	public void OpenDataflow()  //Triggered by DataflowDropdown Item
	{
		OpenDataflow(DataflowSave.list[dataflowDropdown.value]);
		drone = null;
	}
	
	public void OpenDataflow(DataflowSave ds)
	{
		Dataflow d = ds.ToDataflow();
		d.save = ds;
		
		if (dataflow != null && dataflow.save != ds)
			CloseDataflow();

		SetReadonly(false);

		OpenDataflow(d);
	}

	public void OpenDataflow(Dataflow d)
	{
		//Nodes
		List<DataflowNode> nodes = new List<DataflowNode>();
		for (int n = 0; n < d.nodes.Count; n++)
			DrawNode(d.nodes[n]);
		
		//Links
		for (int n = 0; n < d.nodes.Count; n++)
		{
			DataflowNode node = d.nodes[n];
			for (int i = 0; i < node.outputs.Count; i++)
				for (int l = 0; l < node.outputs[i].links.Count; l++)
				{
					DataflowOutput output = node.outputs[i];
					DataflowInput input = node.outputs[i].links[l];
					UINodeLink uiNodeLink = Instantiate(Resources.Load<UINodeLink>("Link"), panel);
					uiNodeLink.outputAnchor = output.transform.GetChild(1).GetComponent<RectTransform>();
					uiNodeLink.inputAnchor = input.transform.GetChild(1).GetComponent<RectTransform>();
					input.uiLink = uiNodeLink;
				}
		}

		dataflow = d;
		
		RefreshDataflowDropdown();
		RefreshDroneDropdown();
	}

	public void CloseDataflow()
	{
		for (int i = 0; i < panel.childCount; i++)
			Destroy(panel.GetChild(i).gameObject);

		dataflow = null;
	}

	public void SaveDataflow()
	{
		DataflowSave newSave = new DataflowSave(dataflow);
		foreach (var drone in Drone.list.FindAll(dr => dr.dataflow != null && dr.dataflow.save == dataflow.save))
			drone.SetDataflow(newSave);
		DataflowSave.list[DataflowSave.list.IndexOf(dataflow.save)] = newSave;
		dataflow.save = newSave;
	}

	public void DeleteDataflow()
	{
		//TODO: DeleteDataflow()

	}

	public void DeleteNode(DataflowNode node, GameObject go)
	{
		for (int i = 0; i < node.inputs.Count; i++)
			if (node.inputs[i].link != null)
				if (node.inputs[i].uiLink)
					Destroy(node.inputs[i].uiLink.gameObject);
		for (int i = 0; i < node.outputs.Count; i++)
			for (int l = 0; l < node.outputs[i].links.Count; l++)
			{
				if (!node.outputs[i].links[l].noDefaultValue)
					node.outputs[i].links[l].transform.GetChild(0).gameObject.SetActive(true);
				if (node.outputs[i].links[l].uiLink)
					Destroy(node.outputs[i].links[l].uiLink.gameObject);
			}
		dataflow.DeleteNode(node);
		Destroy(go);
	}

	public void DrawNode(DataflowNode node)
	{
		RectTransform nodeRT = Instantiate(nodePrefab, panel).GetComponent<RectTransform>();
		nodeRT.anchoredPosition = new Vector2(node.posx, node.posy);
		nodeRT.GetChild(0).GetChild(0).GetComponent<Text>().text = node.name.Split('.')[1];
		nodeRT.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { DeleteNode(node, nodeRT.gameObject); });		
		node.transform = nodeRT;

		//Activator
		DrawInput(node.activator, nodeRT, nodeRT.GetChild(0).GetChild(0));

		//Inputs		
		for (int i = 0; i < node.inputs.Count; i++)
			DrawInput(node.inputs[i], nodeRT, nodeRT.GetChild(1).GetChild(0));

		//Outputs
		for (int i = 0; i < node.outputs.Count; i++)
		{
			RectTransform outputRT = Instantiate(outputPrefab, nodeRT.GetChild(1).GetChild(1)).GetComponent<RectTransform>();
			outputRT.GetChild(0).GetComponent<Text>().text = node.outputs[i].name;
			//outputRT.GetComponent<BoundTooltipTrigger>().text = node.outputs[i].name;
			outputRT.GetComponent<UINodeOutput>().output = node.outputs[i];
			node.outputs[i].transform = outputRT;
			//Icon type
			Image icon = outputRT.GetChild(1).GetComponent<Image>();
			switch (node.outputs[i].type)
			{
				case Dataflow.IOType.Number: icon.color = Color.green; break;
				case Dataflow.IOType.Boolean: icon.color = Color.yellow; break;
				case Dataflow.IOType.Activator: icon.color = Color.red; break;
				case Dataflow.IOType.TargetsList: icon.color = Color.gray; break;
				case Dataflow.IOType.CoordinatesList: icon.color = Color.white; break;
				case Dataflow.IOType.Target: icon.color = Color.blue; break;
			}
		}

		//Configs
		foreach (var c in node.configs)
		{
			RectTransform configRT = Instantiate(configPrefab, nodeRT.GetChild(0).GetChild(1)).GetComponent<RectTransform>();
			configRT.GetChild(0).GetComponent<Text>().text = c.Key;

			if (c.Value.type == Dataflow.ConfigType.Boolean)
			{
				Destroy(configRT.GetChild(1).GetChild(0).gameObject);
				Destroy(configRT.GetChild(1).GetChild(1).gameObject);
				Toggle toggle = configRT.GetChild(1).GetChild(2).GetComponent<Toggle>();
				if (c.Value.value.ToLower() == "true")
					toggle.isOn = true;
				if (c.Value.value.ToLower() == "false")
					toggle.isOn = false;
				toggle.onValueChanged.AddListener(delegate { c.Value.value = toggle.isOn ? "true" : "false"; });
			}
			else if (c.Value.values == null)
			{
				Destroy(configRT.GetChild(1).GetChild(1).gameObject);
				Destroy(configRT.GetChild(1).GetChild(2).gameObject);
				InputField inputField = configRT.GetChild(1).GetChild(0).GetComponent<InputField>();
				switch (c.Value.type)
				{
					case Dataflow.ConfigType.Float: inputField.contentType = InputField.ContentType.DecimalNumber; inputField.text = "0.0"; break;
					case Dataflow.ConfigType.Integer: inputField.contentType = InputField.ContentType.IntegerNumber; inputField.text = "0"; break;
					case Dataflow.ConfigType.String: inputField.contentType = InputField.ContentType.Standard; break;
				}
				if (c.Value.value.Length > 0)
					inputField.text = c.Value.value;
				inputField.onEndEdit.AddListener(delegate { c.Value.value = inputField.text; });
			}
			else
			{
				Destroy(configRT.GetChild(1).GetChild(0).gameObject);
				Destroy(configRT.GetChild(1).GetChild(2).gameObject);
				Dropdown dropdown = configRT.GetChild(1).GetChild(1).GetComponent<Dropdown>();
				dropdown.ClearOptions();
				dropdown.AddOptions(c.Value.values);
				dropdown.value = c.Value.values.IndexOf(c.Value.value);
				dropdown.onValueChanged.AddListener(delegate { c.Value.value = c.Value.values[dropdown.value]; });
			}

			c.Value.transform = configRT;
		}

	} 

	public void DrawInput(DataflowInput input, RectTransform nodeRT, Transform parent)
	{
		RectTransform inputRT = Instantiate(inputPrefab, parent).GetComponent<RectTransform>();
		inputRT.GetComponent<BoundTooltipTrigger>().text = input.name;
		//Input Field for default value
		inputRT.GetChild(0).GetComponent<InputField>().text = input.ValueToString();
		InputField ipf = inputRT.GetChild(0).GetComponent<InputField>();
		switch (input.type)
		{
			case Dataflow.IOType.Number: ipf.contentType = InputField.ContentType.DecimalNumber; break;
			case Dataflow.IOType.Boolean: ipf.contentType = InputField.ContentType.Standard; break;
		}
		if (input.link != null || input.noDefaultValue || input.type == Dataflow.IOType.Activator)
			inputRT.GetChild(0).gameObject.SetActive(false);
		//Icon type
		Image icon = inputRT.GetChild(1).GetComponent<Image>();
		switch (input.type)
		{
			case Dataflow.IOType.Number: icon.color = Color.green; break;
			case Dataflow.IOType.Boolean: icon.color = Color.yellow; break;
			case Dataflow.IOType.Activator: icon.color = Color.red; break;
			case Dataflow.IOType.TargetsList: icon.color = Color.gray; break;
			case Dataflow.IOType.CoordinatesList: icon.color = Color.white; break;
			case Dataflow.IOType.Target: icon.color = Color.blue; break;
		}
		inputRT.GetComponent<UINodeInput>().input = input;
		input.transform = inputRT;
	}
	
	public void StartNewLink(DataflowOutput output)
	{
		newLinkOutput = output;
		uiLinkTmp = Instantiate(linkPrefab, instance.panel).GetComponent<UINodeLink>();
		uiLinkTmp.outputAnchor = newLinkOutput.transform.GetChild(1).GetComponent<RectTransform>();
		uiLinkTmp.inputAnchor = instance.cursor;
	}

	public void EndNewLink(DataflowInput input)
	{
		newLinkInput = input;
		uiLinkTmp.inputAnchor = input.transform.GetChild(1).GetComponent<RectTransform>();

		//Check if the same or if type matches
		if (newLinkOutput.transform.parent.parent == newLinkInput.transform.parent.parent || (newLinkOutput.type != newLinkInput.type && newLinkInput.type != Dataflow.IOType.Activator))
		{
			newLinkOutput = null;
			newLinkInput = null;
			Destroy(uiLinkTmp.gameObject);
			return;
		}

		//Remove already existing link
		if (newLinkInput.link != null)
		{
			if(!newLinkInput.noDefaultValue)
				newLinkInput.transform.GetChild(0).gameObject.SetActive(true);
			dataflow.DeleteLink(newLinkInput);
			Destroy(newLinkInput.uiLink.gameObject);
		}

		dataflow.AddLink(newLinkOutput, newLinkInput);
		newLinkInput.uiLink = uiLinkTmp;
		newLinkInput.transform.GetChild(0).gameObject.SetActive(false);

		newLinkOutput = null;
		newLinkInput = null;
	}

	public void DeleteLinkFromInput(DataflowInput input)
	{
		if (input.link != null)
		{
			if (!input.noDefaultValue)
				input.transform.GetChild(0).gameObject.SetActive(true);
			dataflow.DeleteLink(input);
			Destroy(input.uiLink.gameObject);
		}
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		//Cancel linking nodes
		if (newLinkOutput != null && newLinkInput == null)
		{
			newLinkOutput = null;
			Destroy(uiLinkTmp.gameObject);
		}
	}
	
	public void NewDataflow() //Triggered by NewButton
	{
		if (dataflow != null)
			CloseDataflow();

		DataflowSave ds = new DataflowSave(new Dataflow("dataflow" + DataflowSave.list.Count));
		DataflowSave.list.Add(ds);
		OpenDataflow(ds);	
	}
	
	public void Hide()
	{
		MainCamera.active = true;
		GetComponent<RectTransform>().localScale = Vector3.zero;
		visible = false;
	}

	public void Show()
	{
		RefreshDataflowDropdown();
		RefreshDroneDropdown();
		MainCamera.active = false;
		GetComponent<RectTransform>().localScale = Vector3.one;
		visible = true;
	}

	public void ToggleVisibility()
	{
		visible = !visible;
		if (visible)
			Show();
		else
			Hide();
	}

	public void SetReadonly(bool value)
	{		
		if (value)
		{
			addNodeList.parent.parent.parent.gameObject.SetActive(false);
			saveButton.interactable = false;
			deleteButton.interactable = false;
			//lockPanel.SetActive(true);
		}
		else
		{
			addNodeList.parent.parent.parent.gameObject.SetActive(true);
			saveButton.interactable = true;
			deleteButton.interactable = true;
			//lockPanel.SetActive(false);
		}	

	}

	public void ToggleDebugFlow()
	{
		Dataflow.debugFlow = !Dataflow.debugFlow;
	}
}
