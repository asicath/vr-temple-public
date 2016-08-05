using UnityEngine;
using System.Collections.Generic;



public class Position
{

    static public class Direction
    {
        public const int N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7;
    }

    static public readonly float StraightBaseCost = 1;
    static public readonly float DiagonalBaseCost = Mathf.Sqrt(2);

    public float x, z;
    public int xIndex, zIndex;
    public bool invalid = false;
    public float[] moveCost = new float[8];

    public Position()
    {
        // default the moveCosts
        moveCost[Direction.N] = moveCost[Direction.E] = moveCost[Direction.S] = moveCost[Direction.W] = StraightBaseCost;
        moveCost[Direction.NW] = moveCost[Direction.NE] = moveCost[Direction.SW] = moveCost[Direction.SE] = DiagonalBaseCost;
    }

    public float getMoveCost(int direction)
    {
        return moveCost[direction];
    }

    public float calculateBaseDistanceCost(Position p)
    {
        var xD = Mathf.Abs(p.xIndex - this.xIndex);
        var zD = Mathf.Abs(p.zIndex - this.zIndex);
        var diagonal = Mathf.Min(xD, zD);
        var straight = xD - diagonal + zD - diagonal;
        return diagonal * DiagonalBaseCost + straight * StraightBaseCost;
    }
}

public class PathNode
{
    public Position position;
    public float gCost, hCost; // distance from start, distance from end
    public PathNode parent;
}

public class PositionGrid
{
    private float unitSize = 0.1f;
    public float xSize = 12, zSize = 12;

    private Position[,] positions;
    private float xSizeReal, ySizeReal;

    public PositionGrid()
    {
        initGrid();
    }

    private void initGrid()
    {


        int xPositions = (int)Mathf.Ceil(xSize / unitSize);
        int zPositions = (int)Mathf.Ceil(zSize / unitSize);
        // make sure its odd
        if (xPositions % 2 == 0) xPositions += 1;
        if (zPositions % 2 == 0) zPositions += 1;

        xSizeReal = xPositions * unitSize;
        ySizeReal = xPositions * unitSize;

        Debug.Log("Positions:" + xPositions + "," + zPositions);

        positions = new Position[xPositions, zPositions];

        for (int x = 0; x < xPositions; x++)
        {
            for (int z = 0; z < zPositions; z++)
            {
                var p = positions[x, z] = new Position()
                {
                    x = x * unitSize - (xPositions - 1) * unitSize * 0.5f,
                    z = z * unitSize - (zPositions - 1) * unitSize * 0.5f
                };
                //Debug.Log(x + "," + z + " : " + p.x + ", " + p.z);
            }
        }
    }

    public Position getClosestPosition(float x, float z)
    {
        var xSizeReal = positions.GetLength(0) * unitSize;
        var zSizeReal = positions.GetLength(1) * unitSize;

        var xIndex = Mathf.RoundToInt(((x + xSizeReal * 0.5f) / unitSize) - unitSize * 0.5f);
        var zIndex = Mathf.RoundToInt(((z + zSizeReal * 0.5f) / unitSize) - unitSize * 0.5f);

        if (xIndex > positions.GetLength(0)) xIndex = positions.GetLength(0);
        if (zIndex > positions.GetLength(1)) zIndex = positions.GetLength(1);
        if (xIndex < 0) xIndex = 0;
        if (zIndex < 0) zIndex = 0;

        Debug.Log(xIndex + "," + zIndex);

        return positions[xIndex, zIndex];
    }

    public List<Position> findPath(float xStart, float zStart, float xEnd, float zEnd)
    {
        var start = getClosestPosition(xStart, zStart);
        var end = getClosestPosition(xEnd, zEnd);

        var list = new List<Position>();
        list.Add(end);

        return list;
    }
}

public class Pathfind : MonoBehaviour {

    private float speed = 1;
    private PositionGrid grid;

    private List<Position> path;

	// Use this for initialization
	void Start () {
        grid = new PositionGrid() { xSize = 12, zSize = 12 };
    }

    public void calculatePath(float x, float z)
    {
        path = grid.findPath(transform.position.x, transform.position.z, x, z);

        foreach (var p in path)
        {
            Debug.Log(p.x + ", " + p.z);
        }

        //var p = grid.getClosestPosition(x, z);
        //transform.position = new Vector3(p.x, 0, p.z);
    }

    // Update is called once per frame
    void Update()
    {

        if (path == null) calculatePath(5, 0);

        if (path != null)
        {
            var maxMoveAmount = speed * Time.deltaTime;
            while (path.Count > 0 && maxMoveAmount > 0)
            {
                maxMoveAmount = moveTo(path[0], maxMoveAmount);
                if (maxMoveAmount > 0) path.RemoveAt(0);
            }

        }      
        
    }

    public float moveTo(Position p, float maxMoveAmount)
    {
        var d = new Vector3(p.x, 0, p.z) - new Vector3(transform.position.x, 0, transform.position.z);

        var remainingDistance = d.magnitude;
        

        if (remainingDistance >= maxMoveAmount)
        {
            var n = transform.position + d.normalized * maxMoveAmount;
            transform.position = new Vector3(n.x, transform.position.y, n.z);
            return 0; // none remaining
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            return maxMoveAmount - remainingDistance; // give back the remaining
        }
    }

}
