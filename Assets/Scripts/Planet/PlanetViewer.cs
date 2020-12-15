using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlanetViewer : MonoBehaviour {
	
	public static Planet planet;

	private Sprite[][] tiles;
	private Material defaultMaterial;
	public Sprite sourceSprite;
	
	private void Awake()
	{
		tiles = new Sprite[1][];
		tiles[0] = Resources.LoadAll<Sprite>("Tiles/wall");
		defaultMaterial = Resources.Load<Material>("TileMaterial");

		Load(new Planet(128).Generate());
	}

	public void Load(Planet loadPlanet)
	{
		Unload();
		planet = loadPlanet;

		//Terrain
		for (int l = 0; l < tiles.Length; l++)
			for (int x = 0; x < planet.size - 1; x++) for (int y = 0; y < planet.size - 1; y++)
				if (planet.map[x, y] == l + 1)
				{
					int id = GetTileID(planet.map, x, y, l + 1);
					SpriteRenderer tile = new GameObject(id.ToString() + "(" + x + "," + y + ": " + planet.map[x, y] + ")").AddComponent<SpriteRenderer>();
					tile.material = defaultMaterial;
					tile.transform.parent = transform;
					tile.transform.localPosition = new Vector3(x, y, -1 - l);

					if (!tileVariants.ContainsKey(id))
						Debug.Log("Wrong tile id on " + x + "," + y + ": " + id, tile);
					else
						SetTileSprite(tile, id, tiles[l]);
				}

		//Sources
		for (int i = 0; i < planet.sources.Count; i++)
		{
			Planet.SourceData sourceData = planet.sources[i];
			GameObject go = new GameObject(sourceData.name + " (" + (int)sourceData.position.x + "," + (int)sourceData.position.y + ": " + planet.map[(int)sourceData.position.x, (int)sourceData.position.y] + ")");
			SpriteRenderer tile = go.AddComponent<SpriteRenderer>();
			tile.material = defaultMaterial;
			tile.transform.parent = transform;
			tile.transform.localPosition = new Vector3(sourceData.position.x, sourceData.position.y, -1 - planet.map[(int)sourceData.position.x, (int)sourceData.position.y]);
			tile.transform.localScale = new Vector2(4, 4);
			tile.sprite = sourceSprite;
			Source source = go.AddComponent<Source>();
			source.count = sourceData.count;
		}

		int size = planet.size;
		Texture2D txr = new Texture2D(size, size);
		Color[] colorMap = new Color[size * size];

		txr.filterMode = FilterMode.Point;
		txr.wrapMode = TextureWrapMode.Clamp;

		//Save map to PNG
		for (int y = 0; y < size; y++)
			for (int x = 0; x < size; x++)
				colorMap[y * size + x] = planet.map[x, y] == 0 ? Color.white : Color.gray;
		txr.SetPixels(colorMap);
		txr.Apply();

		var bytes = txr.EncodeToPNG();
		var file = File.Open(Application.dataPath + "/map.png", FileMode.Create);
		var binary = new BinaryWriter(file);
		binary.Write(bytes);
		file.Close();
	}

	public void Unload()
	{
		if (planet != null)
		{
			for (int i = 0; i < transform.childCount; i++)
				Destroy(transform.GetChild(i).gameObject);

			planet = null;
		}
	}

	class Variant
	{
		public int id;			//0-47
		public int rotation;    //-1, 0, 1 : -90, 0, 90
		public int flipx;       //1, 0
		public int flipy;       //1, 0

		public Variant(int id, int rotation = 0, int flipx = 0, int flipy = 0)
		{
			this.id = id;
			this.rotation = rotation;
			this.flipx = flipx;
			this.flipy = flipy;
		}
	}
	/*			
			2 = 1,
			8 = 2, 
			10 = 3, 
			11 = 4, 
			16 = 5, 
			18 = 6, 
			22 = 7, 
			24 = 8, 
			26 = 9, 
			27 = 10, 
			30 = 11, 
			31 = 12, 
			64 = 13, 
			66 = 14, 
			72 = 15, 
			74 = 16, 
			75 = 17, 
			80 = 18, 
			82 = 19, 
			86 = 20, 
			88 = 21, 
			90 = 22, 
			91 = 23, 
			94 = 24, 
			95 = 25, 
			104 = 26, 
			106 = 27, 
			107 = 28, 
			120 = 29, 
			122 = 30, 
			123 = 31, 
			126 = 32, 
			127 = 33, 
			208 = 34, 
			210 = 35, 
			214 = 36, 
			216 = 37, 
			218 = 38, 
			219 = 39, 
			222 = 40, 
			223 = 41, 
			248 = 42, 
			250 = 43, 
			251 = 44, 
			254 = 45, 
			255 = 46,
			0, 0, 0, 0 = 47
		*/
	Dictionary<int, Variant> tileVariants = new Dictionary<int, Variant>
	{
		//{ 0, new Variant(13) },
		{2, new Variant(1) },
		{8, new Variant(1, 1) },
		{10, new Variant(2) },
		{11, new Variant(3) },
		{16, new Variant(1, -1) },
		{18, new Variant(2, -1) },
		{22, new Variant(3, -1) },

		{24, new Variant(4) },
		{26, new Variant(5) },
		{27, new Variant(6) },
		{30, new Variant(6,0,1) },
		{31, new Variant(7) },
		{64, new Variant(1,0,0,1) },
		{66, new Variant(4,1) },
		{72, new Variant(2,1) },

		{74, new Variant(5,1) },
		{75, new Variant(6,1,1) },
		{80, new Variant(2,0,1,1) },
		{82, new Variant(5,-1) },
		{86, new Variant(6,-1) },
		{88, new Variant(5,0,0,1) },
		{90, new Variant(8) },
		{91, new Variant(10,1) },

		{94, new Variant(10) },
		{95, new Variant(11) },
		{104, new Variant(3,1) },
		{106, new Variant(6,1) },
		{107, new Variant(7,1) },
		{120, new Variant(6,0,0,1) },
		{122, new Variant(10,0,1,1) },
		{123, new Variant(11,1) },
		
		{126, new Variant(9,1) },
		{127, new Variant(12) },
		{208, new Variant(3,0,1,1) },
		{210, new Variant(6,1,0,1) },
		{214, new Variant(7,-1) },
		{216, new Variant(6,0,1,1) },
		{218, new Variant(10,-1) },
		{219, new Variant(9) },
		
		{222, new Variant(11,-1) },
		{223, new Variant(12,-1) },
		{248, new Variant(7,0,0,1) },
		{250, new Variant(11,0,0,1) },
		{251, new Variant(12,1) },
		{254, new Variant(12,0,1,1) },
		{255, new Variant(13) },
		{0, new Variant(0) },

	};

	private void SetTileSprite(SpriteRenderer tile, int id, Sprite[] sprites)
	{		
		tile.sprite = sprites[tileVariants[id].id];
		tile.transform.rotation = Quaternion.Euler(0, 0, tileVariants[id].rotation * 90);
		tile.flipX = tileVariants[id].flipx == 1;
		tile.flipY = tileVariants[id].flipy == 1;
	}

	private int GetTileID(byte[,] map, int mapx, int mapy, int id)
	{
		int value = 0;

		bool t1 = map[Mathf.Max(0, mapx - 1), Mathf.Min(map.GetLength(1), mapy + 1)] == id;
		bool t2 = map[mapx + 0, Mathf.Min(map.GetLength(1), mapy + 1)] == id;
		bool t4 = map[Mathf.Min(map.GetLength(0), mapx + 1), Mathf.Min(map.GetLength(1), mapy + 1)] == id;

		bool t8 = map[Mathf.Max(0, mapx - 1), mapy + 0] == id;
		bool t16 = map[Mathf.Min(map.GetLength(0), mapx + 1), mapy + 0] == id;

		bool t32 = map[Mathf.Max(0, mapx - 1), Mathf.Max(0, mapy - 1)] == id;
		bool t64 = map[mapx + 0, Mathf.Max(0, mapy - 1)] == id;
		bool t128 = map[Mathf.Min(map.GetLength(0), mapx + 1), Mathf.Max(0, mapy - 1)] == id;

		//Border tiles
		value += t64? 64 : 0;
		value += t16? 16 : 0;
		value += t8? 8 : 0;
		value += t2? 2 : 0;

		//Corner tiles
		if(t64 && t16)
			value += t128? 128 : 0;
		if (t8 && t64)
			value += t32? 32 : 0;
		if (t2 && t16)
			value += t4? 4 : 0;
		if (t8 && t2)
			value += t1? 1 : 0;
		
		return value;
	}

	/* Tile order
			1	2	4
			8	-	16
			32	64	128
		*/

	/* Value without Mathf.Max
	value += map[mapx + 1, mapy - 1] == id ? 128 : 0;
	value += map[mapx + 0, mapy - 1] == id ? 64 : 0;
	value += map[mapx - 1, mapy - 1] == id ? 32 : 0;

	value += map[mapx + 1, mapy + 0] == id ? 16 : 0;
	value += map[mapx - 1, mapy + 0] == id ? 8 : 0;

	value += map[mapx + 1, mapy + 1] == id ? 4 : 0;
	value += map[mapx + 0, mapy + 1] == id ? 2 : 0;
	value += map[mapx - 1, mapy + 1] == id ? 1 : 0;
	*/

}
