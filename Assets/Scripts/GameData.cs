using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class GameData {

	public static GameData current;

	public List<DataflowSave> dataflowSaves = new List<DataflowSave>();

	public GameData()
	{
		current = this;
	}

	public static void Save(string name)
	{
		current.dataflowSaves = DataflowSave.list;
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/" + name + ".sav", FileMode.Create);
		bf.Serialize(file, current);
		file.Close();
	}

	public static void Load(string name)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/" + name + ".sav", FileMode.Open);
		current = (GameData)bf.Deserialize(file);
		file.Close();
		
		DataflowSave.list = current.dataflowSaves;

		UIDroneEditor.instance.ReloadEditor();
		UIDataflowEditor.instance.ReloadEditor();
	}

}
