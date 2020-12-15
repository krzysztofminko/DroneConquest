using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Low fps goal missing
public class AStar : MonoBehaviour{

	public class Position
	{
		public int x;
		public int y;
		public float f;
		public float g;
		public Position parent;

		public Vector2 ToVector2()
		{
			return new Vector2(x, y);
		}
	}

	

	public static AStar instance;

	float normalCost = 1.0f;
	float diagonalCost = 1.4f;

	[Range(0.1f, 1.0f)]
	public float quality;
	[Range(1, 1000)]
	public int queriesPerFrame = 1;
	[Range(1, 1000)]
	public int queryStepsPerFrame = 1;
	public List<FindQuery> findQueries = new List<FindQuery>();
	private int lastQuery;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		int i;
		for (i = lastQuery; i < Mathf.Min(findQueries.Count, lastQuery + queriesPerFrame); i++)
		{
			if (findQueries[i].path == null)
				if (findQueries[i].Proceed(queryStepsPerFrame) || findQueries[i].open.Count == 0)
					findQueries.RemoveAt(i);
		}
		lastQuery = Mathf.Min(findQueries.Count, lastQuery + queriesPerFrame);
		if (lastQuery >= findQueries.Count)
			lastQuery = 0;
	}

	public static List<Vector2> PathToVectors(List<Position> path)
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < path.Count; i++)
			list.Add(new Vector2(path[i].x, path[i].y));
		return list;
	}
	
	/*
	public static FindQuery FindPath(Drone drone, byte[,] map, int endx, int endy, bool debug = false, bool simpleDebug = false)
	{
		int id = instance.findQueries.FindIndex(q => q.drone == drone);
		if (id > -1)
			instance.findQueries.RemoveAt(id);
		FindQuery query = FindPath(map, (int)drone.transform.position.x, (int)drone.transform.position.y, endx, endy, debug, simpleDebug);
		query.drone = drone;
		return query;
	}
	*/

	public static FindQuery FindPath(bool[,] map, int startx, int starty, int endx, int endy, bool debug = false, bool simpleDebug = false)
	{
		if (startx == endx && starty == endy)
			return null;

		FindQuery query = new FindQuery {
			map = map,
			startx = startx,
			starty = starty,
			endx = endx,
			endy = endy,
			mapSizeX = map.GetLength(0),
			mapSizeY = map.GetLength(1),
			open = new List<Position> { new Position { x = startx, y = starty } },
			closed = new List<Position>(),
			current = null,
			currentId = -1,
			totalSteps = 0,
			path = null,
			drone = null,
			debug = debug,
			simpleDebug = simpleDebug,
			stopwatch = new System.Diagnostics.Stopwatch()
		};

		instance.findQueries.Add(query);

		if (query.debug && !query.simpleDebug)
			query.stopwatch.Start();

		return query;
	}


	[System.Serializable]
	public class FindQuery
	{
		public bool[,] map;
		public int startx;
		public int starty;
		public int endx;
		public int endy;
		public int mapSizeX;
		public int mapSizeY;
		public List<Position> open;
		public List<Position> closed;
		public Position current;
		public int currentId;
		public int totalSteps;
		public List<Position> path;
		public Drone drone;
		public bool notFound;


		public bool debug;
		public bool simpleDebug;
		public System.Diagnostics.Stopwatch stopwatch;

		public bool Proceed(int steps)
		{
			int step = 0;
			do
			{
				step++;
				totalSteps++;
				// Get the square with the lowest F 			
				float minf = Mathf.Infinity;
				for (int i = 0; i < open.Count; i++)
					if (open[i].f < minf)
					{
						minf = open[i].f;
						current = open[i];
						currentId = i;
					}

				if (debug && !simpleDebug && current.parent != null)
					Debug.DrawLine(new Vector3(current.x, current.y, -5), new Vector3(current.parent.x, current.parent.y, -5), Color.red, 100);

				// add the current square to the closed list
				closed.Add(current);

				// remove it from the open list
				open.RemoveAt(currentId);

				// if we added the destination to the closed list, we've found a path
				Position goal = closed.Find(p => p.x == endx && p.y == endy);
				if (goal != null)
				{
					if (debug && !simpleDebug)
						Debug.Log("Found Path! " + totalSteps);
					path = new List<Position>();
					//Add goal if walkable
					if (map[goal.x, goal.y])
						path.Add(goal);
					Position tmp = goal;
					do
					{
						if (tmp.parent != null)
						{
							if (debug)
								Debug.DrawLine(new Vector3(tmp.x, tmp.y, -10), new Vector3(tmp.parent.x, tmp.parent.y, -10), Color.green, 100);
							path.Add(tmp.parent);
						}
						tmp = tmp.parent;
					} while (tmp != null);
					if (debug && !simpleDebug)
					{
						stopwatch.Stop();
						Debug.Log("Time elapsed (" + startx + "," + starty + ":" + endx + "," + endy + "): " + stopwatch.Elapsed.TotalMilliseconds);
					}

					return true;
				}

				// Retrieve all its walkable adjacent squares or unwalkable but goal
				List<Position> adjacent = new List<Position>();
				if (current.y + 1 < mapSizeY)
					if (map[current.x, current.y + 1] || (current.x == endx && current.y + 1 == endy)) adjacent.Add(new Position { x = current.x, y = current.y + 1, g = instance.normalCost });
				if (current.x - 1 >= 0)
					if (map[current.x - 1, current.y] || (current.x - 1 == endx && current.y == endy)) adjacent.Add(new Position { x = current.x - 1, y = current.y, g = instance.normalCost });
				if (current.y - 1 >= 0)
					if (map[current.x, current.y - 1] || (current.x == endx && current.y - 1 == endy)) adjacent.Add(new Position { x = current.x, y = current.y - 1, g = instance.normalCost });
				if (current.x + 1 < mapSizeX)
					if (map[current.x + 1, current.y] || (current.x + 1 == endx && current.y == endy)) adjacent.Add(new Position { x = current.x + 1, y = current.y, g = instance.normalCost });

				if (current.x + 1 < mapSizeX && current.y + 1 < mapSizeY)
					if (map[current.x + 1, current.y + 1] || (current.x + 1 == endx && current.y + 1 == endy)) adjacent.Add(new Position { x = current.x + 1, y = current.y + 1, g = instance.diagonalCost });
				if (current.x + 1 < mapSizeX && current.y - 1 >= 0)
					if (map[current.x + 1, current.y - 1] || (current.x + 1 == endx && current.y - 1 == endy)) adjacent.Add(new Position { x = current.x + 1, y = current.y - 1, g = instance.diagonalCost });
				if (current.x - 1 >= 0 && current.y + 1 < mapSizeY)
					if (map[current.x - 1, current.y + 1] || (current.x - 1 == endx && current.y + 1 == endy)) adjacent.Add(new Position { x = current.x - 1, y = current.y + 1, g = instance.diagonalCost });
				if (current.x - 1 >= 0 && current.y - 1 >= 0)
					if (map[current.x - 1, current.y - 1] || (current.x - 1 == endx && current.y - 1 == endy)) adjacent.Add(new Position { x = current.x - 1, y = current.y - 1, g = instance.diagonalCost });

				for (int i = 0; i < adjacent.Count; i++)
				{
					Position adj = adjacent[i];
					// if this adjacent square is already in the closed list ignore it
					if (closed.Find(p => p.x == adj.x && p.y == adj.y) != null)
						continue;

					// if its not in the open list compute its score, set the parent and add it to the open list
					if (open.Find(p => p.x == adj.x && p.y == adj.y) == null)
					{
						float g = current.g + adj.g;
						int dx = Mathf.Abs(endx - adj.x);
						int dy = Mathf.Abs(endy - adj.y);
						//int h = instance.normalCost * (dx + dy);	//Manhattan
						float h = instance.normalCost * (dx + dy) + (instance.diagonalCost - 2 * instance.normalCost) * Mathf.Min(dx, dy); //Diagonal
						adj.f = g + (10 * (1.1f - instance.quality)) * h;
						adj.g = g;
						adj.parent = current;
						open.Add(adj);
					}
					else
					{
						// test if using the current G score make the aSquare F score lower, if yes update the parent because it means its a better path
						if (current.g + 1 < adj.g)
							open.RemoveAt(open.FindIndex(p => p.x == adj.x && p.y == adj.y));
					}
				}
			} while (open.Count > 0 && step < steps);// Repeat until there is no more available square in the open list (which means there is no path)

			if (open.Count == 0)
			{
				notFound = true;
				if (debug && !simpleDebug)
				{
					Debug.Log("No Path. " + totalSteps);
					stopwatch.Stop();
					Debug.Log("Time elapsed (" + startx + "," + starty + ":" + endx + "," + endy + "): " + stopwatch.Elapsed.TotalMilliseconds);
				}
			}

			return false;
		}
	}

	
}
