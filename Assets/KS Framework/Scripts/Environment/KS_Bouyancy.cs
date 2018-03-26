using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Bouyancy : MonoBehaviour {

    public float objectDensity = 500;
    public int slicesPerAxis = 3;  

    private List<Vector3> voxelPoints;
    private List<Vector3> outSidePoints = new List<Vector3>();

    private Rigidbody rb;
    private Collider collider;

    public Vector3 force;

    public float waterDensity = 1000f;
    public float damper = 0.1f;

    public bool isInWater = false;
    private float waterLevel;
    private float voxelHalfHeight;

    private Quaternion startRot;
    private Vector3 startPos;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        startPos = transform.position;
        startRot = transform.rotation;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        voxelPoints = GetVoxelPoints();

        transform.position = startPos;
        transform.rotation = startRot;

        var bounds = collider.bounds;
        if (bounds.size.x < bounds.size.y)
        {
            voxelHalfHeight = bounds.size.x;
        }
        else
        {
            voxelHalfHeight = bounds.size.y;
        }
        if (bounds.size.z < voxelHalfHeight)
        {
            voxelHalfHeight = bounds.size.z;
        }
        voxelHalfHeight /= 2 * slicesPerAxis;

        rb.centerOfMass = new Vector3(0, -bounds.extents.y * 0f, 0) + transform.InverseTransformPoint(bounds.center);
        float volume = rb.mass / objectDensity;

        float magnitude = waterDensity * Mathf.Abs(Physics.gravity.y) * volume;
        force = new Vector3(0, magnitude, 0) / voxelPoints.Count;

        Debug.Log("Voxel points: " + voxelPoints.Count);
	}

    private List<Vector3> GetVoxelPoints()
    {
        List<Vector3> points = new List<Vector3>(slicesPerAxis * slicesPerAxis * slicesPerAxis);
       
        Bounds bounds = collider.bounds;

        for (int ix = 0; ix < slicesPerAxis; ix++)
        {
            for (int iy = 0; iy < slicesPerAxis; iy++)
            {
                for (int iz = 0; iz < slicesPerAxis; iz++)
                {
                    float x = bounds.min.x + bounds.size.x / slicesPerAxis * (0.5f + ix);
                    float y = bounds.min.y + bounds.size.y / slicesPerAxis * (0.5f + iy);
                    float z = bounds.min.z + bounds.size.z / slicesPerAxis * (0.5f + iz);

                    Vector3 point = transform.InverseTransformPoint(new Vector3(x, y, z));
                    points.Add(point);

                    Debug.Log("addded");
                }
            }
        }

        return points;
    }

    public void EnterWater(float waterHeight)
    {
        isInWater = true;
        waterLevel = waterHeight;
    }

    public void LeaveWater()
    {
        isInWater = false;
    }


    private void FixedUpdate()
    {
        if (voxelPoints != null && waterLevel != null)
        {
            foreach (Vector3 point in voxelPoints)
            {
                Vector3 worldPoint = transform.TransformPoint(point);

                if (worldPoint.y - voxelHalfHeight < waterLevel)
                {
                    float k = (waterLevel - worldPoint.y) / (2 * voxelHalfHeight) + 0.5f;

                    if (k > 1)
                    {
                        k = 1f;
                    }
                    else if (k < 0)
                    {
                        k = 0f;
                    }

                    Vector3 velocity = rb.GetPointVelocity(worldPoint);
                    Vector3 damping = -velocity * damper * rb.mass;
                    Vector3 addForce = damping + Mathf.Sqrt(k) * force;

                    rb.AddForceAtPosition(addForce, worldPoint);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        float size = 0.1f;
        Gizmos.color = Color.cyan;

        if (voxelPoints != null || voxelPoints.Count > 0)
        {

            foreach (Vector3 point in voxelPoints)
            {
                Gizmos.DrawCube(transform.TransformPoint(point), new Vector3(size, size, size));
            }
        }
    }
}