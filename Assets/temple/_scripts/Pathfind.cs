using UnityEngine;
using System.Collections;

public class Position
{
    public float x, z;
    public bool invalid = false;
}

public class Pathfind : MonoBehaviour {

    private float time = 0;

    private float unitSize = 0.111f;
    private float xSize = 12, zSize = 12;

    private Position[,] positions;
    private float xSizeReal, ySizeReal;

	// Use this for initialization
	void Start () {

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
                var p = positions[x, z] = new Position() {
                    x = x * unitSize - (xPositions - 1) * unitSize * 0.5f,
                    z = z * unitSize - (zPositions - 1) * unitSize * 0.5f
                };
                //Debug.Log(x + "," + z + " : " + p.x + ", " + p.z);
            }
        }
    }

    private Position getPosition(float x, float z)
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

        return positions[xIndex, xIndex];
    }

    public void moveTo(float x, float z)
    {
        var p = getPosition(x, z);
        transform.position = new Vector3(p.x, 0, p.z);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2 && time < 3)
        {
            moveTo(-6, 6);
            time = 4;
        }
    }

}
