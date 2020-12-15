using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Planet {

	public class SourceData {
		public string name;
		public short count;
		public Vector2 position;
	}

	public static List<Planet> list = new List<Planet>();

	public readonly int size;
	public byte[,] map;
	public bool[,] walkable;
	public List<SourceData> sources = new List<SourceData>();

	public Planet(int size)
	{
		list.Add(this);
		this.size = size;
		map = new byte[size, size];
		walkable = new bool[size, size];
	}

	public Planet Generate()
	{
		for (int x = 0; x < size; x++) for (int y = 0; y < size; y++)
			{
				if (PerlinLayer(x, y, 2) > 0.4f)
				{
					if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.001f)
					{
						map[x, y] = 0;
						walkable[x, y] = false;
						sources.Add(new SourceData { name = "Resource", count = (short)UnityEngine.Random.Range(100, 1000), position = new Vector2(x, y) });
					}
					else
					{
						map[x, y] = 0;
						walkable[x, y] = true;
					}
				}
				else
				{
					map[x, y] = 1;
					walkable[x, y] = false;
				}
			}
		return this;
	}

	private float PerlinLayer(int x, int y, int layer)
	{
		float d;
		float perlinOffset = 0.0f;
		float perlinScale = 512.0f / size;
		switch (layer){
			case 0 :
			//Square falloff
			d = 1.0f - Mathf.Min(1.0f, Mathf.Max(Mathf.Abs(x - size * 0.5f), Mathf.Abs(y - size * 0.5f)) / (size * 0.5f));
			//Circle falloff: 
			//d=1.0-Mathf.Min(1.0,Vector2(x-size*0.5,z-size*0.5).magnitude/(size*0.5));
			//d=1.0;
			return (
					(Mathf.PerlinNoise(perlinOffset + perlinScale * x * 0.01f, perlinOffset + perlinScale * y * 0.01f) * 0.7f
					+ Mathf.PerlinNoise(perlinOffset + perlinScale * x * 0.05f, perlinOffset + perlinScale * y * 0.05f) * 0.2f
					+ Mathf.PerlinNoise(perlinOffset + perlinScale * x * 0.1f, perlinOffset + perlinScale * y * 0.1f) * 0.1f
					) * d);

			case 1 :
			//Linear falloff
			d = y * 1.0f / size;
			return (
				(Mathf.PerlinNoise(perlinOffset + perlinScale * x * 0.02f, perlinOffset + perlinScale * y * 0.02f) * 0.07f
				+ Mathf.PerlinNoise(perlinOffset + perlinScale * x * 0.04f, perlinOffset + perlinScale * y * 0.04f) * 0.03f
				+ d * 0.9f
				)
				);
			case 2 :
			d = 1.0f;
			return (
				(Mathf.PerlinNoise(perlinOffset + perlinScale * x * 0.05f, perlinOffset + perlinScale * y * 0.05f) * 0.05f
				+ Mathf.PerlinNoise(perlinOffset + perlinScale * x * 0.04f, perlinOffset + perlinScale * y * 0.04f) * 0.95f
				) * d);
			default: return 0;
		}
	}
}
