using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace path2
{

    static public class Direction
    {
        public const int N = 0, NE = 1, E = 2, SE = 3, S = 4, SW = 5, W = 6, NW = 7;

        public static readonly int[] All = new int[] { N, NE, E, SE, S, SW, W, NW };

        static public int getXVector(int direction)
        {
            switch (direction)
            {
                case Direction.E:
                case Direction.NE:
                case Direction.SE:
                    return 1;
                case Direction.W:
                case Direction.NW:
                case Direction.SW:
                    return -1;
                default:
                    return 0;
            }
        }

        static public int getZVector(int direction)
        {
            switch (direction)
            {
                case Direction.N:
                case Direction.NE:
                case Direction.NW:
                    return 1;
                case Direction.S:
                case Direction.SW:
                case Direction.SE:
                    return -1;
                default:
                    return 0;
            }
        }
    }

    public class Position
    {

        static private readonly float StraightBaseCost = 1;
        static private readonly float DiagonalBaseCost = Mathf.Sqrt(2);

        public float x, z;
        public int xIndex, zIndex;
        public bool invalid = false;
        private float[] moveCost = new float[8];
        public PathInfo pathInfo;

        public long key { get { return getKey(xIndex, zIndex); } }
        static public long getKey(int xIndex, int zIndex)
        {
            return zIndex * 1000000 + xIndex;
        }

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

        static public float calculateBaseDistanceCost(Position p0, Position p1)
        {
            var xD = Mathf.Abs(p0.xIndex - p1.xIndex);
            var zD = Mathf.Abs(p0.zIndex - p1.zIndex);
            var diagonal = Mathf.Min(xD, zD);
            var straight = xD - diagonal + zD - diagonal;
            return diagonal * DiagonalBaseCost + straight * StraightBaseCost;
        }
    }


    public class PathInfo
    {
        public Position parent;
        public float gCost, hCost; // distance from start, distance from end
        public float cost { get { return gCost + hCost; } }
        public bool expanded = false;
    }

    public class PositionGrid
    {
        private float unitSize;

        private int xPositions, zPositions;

        private List<Position> unexplored;
        private Dictionary<long, Position> positions = new Dictionary<long, Position>();

        public PositionGrid(float xSize, float zSize, float unitSize)
        {
            this.unitSize = unitSize;

            xPositions = (int)Mathf.Ceil(xSize / unitSize);
            zPositions = (int)Mathf.Ceil(zSize / unitSize);

            // make sure its odd
            if (xPositions % 2 == 0) xPositions += 1;
            if (zPositions % 2 == 0) zPositions += 1;

        }

        /*
        private void initGrid()
        {

            for (int x = 0; x < xPositions; x++)
            {
                for (int z = 0; z < zPositions; z++)
                {
                    var p = positions[x, z] = new Position()
                    {
                        x = x * unitSize - (xPositions - 1) * unitSize * 0.5f,
                        z = z * unitSize - (zPositions - 1) * unitSize * 0.5f,
                        xIndex = x,
                        zIndex = z
                    };
                    //Debug.Log(x + "," + z + " : " + p.x + ", " + p.z);
                }
            }
        }
        */

        public Position getClosestPosition(float x, float z)
        {
            var xSizeReal = xPositions * unitSize;
            var zSizeReal = zPositions * unitSize;

            var xIndex = Mathf.RoundToInt(((x + xSizeReal * 0.5f) / unitSize) - unitSize * 0.5f);
            var zIndex = Mathf.RoundToInt(((z + zSizeReal * 0.5f) / unitSize) - unitSize * 0.5f);

            /* keep within bounds?
            if (xIndex >= xPositions) xIndex = xPositions - 1;
            if (zIndex >= zPositions) zIndex = zPositions - 1;
            if (xIndex < 0) xIndex = 0;
            if (zIndex < 0) zIndex = 0;
            */

            return getPositionByIndex(xIndex, zIndex);
        }

        public Position getPositionByIndex(int xIndex, int zIndex)
        {
            if (xIndex >= xPositions) return null;
            if (zIndex >= zPositions) return null;
            if (xIndex < 0) return null;
            if (zIndex < 0) return null;

            var key = Position.getKey(xIndex, zIndex);

            if (positions.ContainsKey(key)) return positions[key];

            Position p = new Position()
            {
                x = xIndex * unitSize - (xPositions - 1) * unitSize * 0.5f,
                z = zIndex * unitSize - (zPositions - 1) * unitSize * 0.5f,
                xIndex = xIndex,
                zIndex = zIndex
            };

            positions[key] = p;

            return p;
        }

        public Position getPositionByDirection(Position p, int direction)
        {
            int x = Direction.getXVector(direction);
            int z = Direction.getZVector(direction);
            return getPositionByIndex(p.xIndex + x, p.zIndex + z);
        }

        public List<Position> findPath(float xStart, float zStart, float xEnd, float zEnd)
        {
            var start = getClosestPosition(xStart, zStart);
            var end = getClosestPosition(xEnd, zEnd);
            var unexplored = new List<Position>();

            // create the initial pathinfo
            start.pathInfo = new PathInfo() { parent = null, gCost = 0, hCost = Position.calculateBaseDistanceCost(start, end) };
            unexplored.Add(start);

            bool found = false;
            while (!found)
            {


                unexplored = unexplored.OrderBy(o => o.pathInfo.cost).ToList();

                Position p = null;

                while (p == null || p.pathInfo.expanded)
                {
                    p = unexplored[0];
                    unexplored.RemoveAt(0);
                }

                //Debug.Log("expanding " + p.xIndex + "," + p.zIndex);

                found = expand(p, end, unexplored);
            }

        

            var list = new List<Position>();
            getShortestPathBack(end, list);

            return list;
        }

        private void getShortestPathBack(Position p, List<Position> path)
        {
            Position best = null;

            foreach (int d in Direction.All)
            {
                var pos = getPositionByDirection(p, d);
                if (pos == null || pos.pathInfo == null) continue;
                if (best == null || best.pathInfo.gCost > pos.pathInfo.gCost) best = pos;
            }

            if (best.pathInfo.gCost > 0) getShortestPathBack(best, path);
            path.Add(best);
        }

        // attempts to create pathInfo for all connected nodes
        private bool expand(Position p, Position end, List<Position> unexplored)
        {
            p.pathInfo.expanded = true;
            var found = false;
            foreach (var d in Direction.All)
            {
                found = found || attemptPath(d, p, end, unexplored);
            }
            return found;
        }

        private bool attemptPath(int direction, Position parent, Position end, List<Position> unexplored)
        {
            var p = getPositionByDirection(parent, direction);

            // no need to process parent
            //if (p == parent) return false;

            // can't move that direction
            if (p == null || p.invalid) return false;

            if (p == end) return true;

            var info = new PathInfo()
            {
                parent = parent,
                gCost = parent.pathInfo.gCost + parent.getMoveCost(direction),
                hCost = Position.calculateBaseDistanceCost(p, end)
            };

            // if this is more expensive than the existing path info, discard it
            if (p.pathInfo != null && p.pathInfo.cost <= info.cost) return false;

            // otherwise save and add to queue
            p.pathInfo = info;

            unexplored.Add(p);

            return false;
        }

    }





    public class Pathfind2 : MonoBehaviour {

        private float speed = 1;
        private PositionGrid grid;

        private List<Position> path;

	    // Use this for initialization
	    void Start () {
            grid = new PositionGrid(12, 12, 0.1f);
        }

        public void calculatePath(float x, float z)
        {
            path = grid.findPath(transform.position.x, transform.position.z, x, z);

            foreach (var p in path)
            {
                //Debug.Log(p.x + ", " + p.z);
            }

            //var p = grid.getClosestPosition(x, z);
            //transform.position = new Vector3(p.x, 0, p.z);
        }

        // Update is called once per frame
        void Update()
        {

            if (path == null) calculatePath(5, 3);

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

}