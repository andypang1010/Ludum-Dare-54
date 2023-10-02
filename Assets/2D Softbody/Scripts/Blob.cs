using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using System;
using System.Collections.Generic;

public class Blob : MonoBehaviour
{
    private class PropagateCollisions : MonoBehaviour
    {
        GameObject zPlayer;

        private void Start()
        {
            zPlayer = GameObject.FindGameObjectWithTag("PlayerMain");
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Sticky")
            {
                gameObject.GetComponent<Rigidbody2D>().constraints =
                    RigidbodyConstraints2D.FreezePositionY
                    | RigidbodyConstraints2D.FreezePositionX
                    | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.tag == "DetachOne")
            {
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }
            if (collision.transform.tag == "DetachAll")
            {
                zPlayer.GetComponent<Blob>().TrigThis();
            }
            if (collision.transform.tag == "Virus")
            {
                Blob blob = zPlayer.GetComponent<Blob>();
                Destroy(collision.gameObject);
                blob.Grow();
                StatsManager.Instance.IncVirusCount();
            }

            if (collision.transform.tag == "Medicine")
            {
                Blob blob = zPlayer.GetComponent<Blob>();
                Destroy(collision.gameObject);
                blob.Shrink();
                StatsManager.Instance.IncVirusCount();
            }
        }
    }

    public int width = 5;
    public int height = 5;
    public int referencePointsCount = 12;
    public float referencePointRadius = 0.25f;
    public float mappingDetail = 10;
    public float springDampingRatio = 0;
    public float springFrequency = 2;
    public float scaleGrowth = 0.5f;
    public float fartForce = 250;
    public float fartCooldown = 5;
    public PhysicsMaterial2D surfaceMaterial;
    public Rigidbody2D[] allReferencePoints;
    public Image coolDown;
    public GameObject fartPrefab;
    public AudioSource audioSource;
    public AudioClip fartSound;
    public AudioClip eatSound;
    public Material blobbyMat;
    public List<Texture> blobbyTextures;
    public GameObject[] referencePoints { private set; get; }
    public GameObject centerPoint { private set; get; }
    int vertexCount;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;
    Vector3[,] offsets;
    float[,] weights;
    float lastFartTime;

    void Start()
    {
        CreateReferencePoints();
        CreateCenterPoint();
        IgnoreCollisionsBetweenReferencePoints();
        CreateMesh();
        MapVerticesToReferencePoints();

        ChangeScale(-2.5f);
        lastFartTime = -fartCooldown;
        blobbyMat.mainTexture = blobbyTextures[0];
        GameManager.Instance.state = GameState.IN_GAME;
    }

    void CreateReferencePoints()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        referencePoints = new GameObject[referencePointsCount];
        Vector3 offsetFromCenter = (0.5f - referencePointRadius) * Vector3.up;
        float angle = 360.0f / referencePointsCount;

        for (int i = 0; i < referencePointsCount; i++)
        {
            referencePoints[i] = new GameObject { tag = gameObject.tag };
            referencePoints[i].AddComponent<PropagateCollisions>();
            referencePoints[i].transform.parent = transform;

            Quaternion rotation = Quaternion.AngleAxis(angle * (i - 1), Vector3.back);
            referencePoints[i].transform.localPosition = rotation * offsetFromCenter;

            Rigidbody2D body = referencePoints[i].AddComponent<Rigidbody2D>();
            body.constraints = RigidbodyConstraints2D.None;
            body.mass = 0.5f;
            body.interpolation = rigidbody.interpolation;
            body.collisionDetectionMode = rigidbody.collisionDetectionMode;
            body.drag = 0.5f;
            body.angularDrag = 0.5f;
            body.gravityScale = 0;
            allReferencePoints[i] = body;

            CircleCollider2D collider = referencePoints[i].AddComponent<CircleCollider2D>();
            collider.radius = referencePointRadius * (transform.localScale.x / 2);
            if (surfaceMaterial != null)
            {
                collider.sharedMaterial = surfaceMaterial;
            }

            AttachWithSpringJoint(referencePoints[i], gameObject, false);
            if (i > 0)
            {
                AttachWithSpringJoint(referencePoints[i], referencePoints[i - 1], false);
            }
        }
        AttachWithSpringJoint(referencePoints[0], referencePoints[referencePointsCount - 1], false);
    }

    void CreateCenterPoint()
    {
        // Create center point
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        centerPoint = new GameObject();
        centerPoint.tag = gameObject.tag;
        centerPoint.AddComponent<PropagateCollisions>();
        centerPoint.transform.parent = transform;

        centerPoint.transform.localPosition = Vector3.zero;
        Rigidbody2D centerBody = centerPoint.AddComponent<Rigidbody2D>();
        centerBody.constraints = RigidbodyConstraints2D.None;
        centerBody.mass = 0.5f;
        centerBody.interpolation = rigidbody.interpolation;
        centerBody.collisionDetectionMode = rigidbody.collisionDetectionMode;
        centerBody.gravityScale = 0;

        CircleCollider2D centerCollider = centerPoint.AddComponent<CircleCollider2D>();
        centerCollider.radius = 3;
        if (surfaceMaterial != null)
        {
            centerCollider.sharedMaterial = surfaceMaterial;
        }

        for (int i = 0; i < referencePointsCount; i++)
        {
            AttachWithSpringJoint(centerPoint, referencePoints[i], true);
        }
    }

    void AttachWithSpringJoint(GameObject referencePoint, GameObject connected, bool isCenter)
    {
        SpringJoint2D springJoint = referencePoint.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = connected.GetComponent<Rigidbody2D>();
        springJoint.connectedAnchor = LocalPosition(referencePoint) - LocalPosition(connected);
        springJoint.distance = isCenter ? width / 2 : 0;
        springJoint.dampingRatio = springDampingRatio;
        springJoint.frequency = springFrequency;
        springJoint.autoConfigureDistance = false;
    }

    void IgnoreCollisionsBetweenReferencePoints()
    {
        int i;
        int j;
        CircleCollider2D a;
        CircleCollider2D b;
        CircleCollider2D c = null;
        if (centerPoint != null)
            c = centerPoint.GetComponent<CircleCollider2D>();

        for (i = 0; i < referencePointsCount; i++)
        {
            for (j = i; j < referencePointsCount; j++)
            {
                a = referencePoints[i].GetComponent<CircleCollider2D>();
                b = referencePoints[j].GetComponent<CircleCollider2D>();
                Physics2D.IgnoreCollision(a, b, true);
            }
            if (c != null)
            {
                a = referencePoints[i].GetComponent<CircleCollider2D>();
                Physics2D.IgnoreCollision(a, c, true);
            }
        }
    }

    void CreateMesh()
    {
        vertexCount = (width + 1) * (height + 1);

        int trianglesCount = width * height * 6;
        vertices = new Vector3[vertexCount];
        triangles = new int[trianglesCount];
        uv = new Vector2[vertexCount];
        int t;

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                int v = (width + 1) * y + x;
                vertices[v] = new Vector3(x / (float)width - 0.5f, y / (float)height - 0.5f, 0);
                uv[v] = new Vector2(x / (float)width, y / (float)height);

                if (x < width && y < height)
                {
                    t = 3 * (2 * width * y + 2 * x);

                    triangles[t] = v;
                    triangles[++t] = v + width + 1;
                    triangles[++t] = v + width + 2;
                    triangles[++t] = v;
                    triangles[++t] = v + width + 2;
                    triangles[++t] = v + 1;
                }
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    void MapVerticesToReferencePoints()
    {
        offsets = new Vector3[vertexCount, referencePointsCount];
        weights = new float[vertexCount, referencePointsCount];

        for (int i = 0; i < vertexCount; i++)
        {
            float totalWeight = 0;

            for (int j = 0; j < referencePointsCount; j++)
            {
                offsets[i, j] = vertices[i] - LocalPosition(referencePoints[j]);
                weights[i, j] = 1 / Mathf.Pow(offsets[i, j].magnitude, mappingDetail);
                totalWeight += weights[i, j];
            }

            for (int j = 0; j < referencePointsCount; j++)
            {
                weights[i, j] /= totalWeight;
            }
        }
    }

    void Update()
    {
        UpdateVertexPositions();
        CheckFart();
        CheckSize();
    }

    private void CheckSize()
    {
        if (gameObject.transform.localScale.x >= StatsManager.maxSize)
        {
            GameManager.Instance.GoLost();
        }
    }

    void UpdateVertexPositions()
    {
        Vector3[] vertices = new Vector3[vertexCount];

        for (int i = 0; i < vertexCount; i++)
        {
            vertices[i] = Vector3.zero;

            for (int j = 0; j < referencePointsCount; j++)
            {
                vertices[i] += weights[i, j] * (LocalPosition(referencePoints[j]) + offsets[i, j]);
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }

    Vector3 LocalPosition(GameObject obj)
    {
        return transform.InverseTransformPoint(obj.transform.position);
    }

    public void TrigThis()
    {
        int z = 0;
        foreach (Rigidbody2D child in allReferencePoints)
        {
            allReferencePoints[z].constraints = RigidbodyConstraints2D.None;
            z++;
        }
    }

    private void CheckFart()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastFartTime + fartCooldown && GameManager.Instance.GetGameStates() == GameState.IN_GAME)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;

            Vector2 moveDir = -(mousePos - transform.position).normalized;
            centerPoint.GetComponent<Rigidbody2D>().velocity += moveDir * fartForce;

            Vector3 fartPos = transform.position - (Vector3)moveDir * (transform.localScale.x / 2);
            Quaternion fartDir = Quaternion.LookRotation(Vector3.back, moveDir);
            Instantiate(fartPrefab, fartPos, fartDir);

            audioSource.PlayOneShot(fartSound);

            Shrink();

            lastFartTime = Time.time;
        }
        if (Time.time <= lastFartTime + fartCooldown)
        {
            float percentage = ((Time.time - lastFartTime) / fartCooldown);
            coolDown.fillAmount = percentage;
        }
        else
        {
            coolDown.fillAmount = 1;
        }
    }

    public void Grow()
    {
        audioSource.PlayOneShot(eatSound);
        ChangeScale(scaleGrowth);
    }

    public void Shrink()
    {
        ChangeScale(-scaleGrowth);
    }

    public void ChangeScale(float scaleChange)
    {
        if (transform.localScale.x + scaleChange < 3)
            return;
        transform.localScale += new Vector3(scaleChange, scaleChange, 0);
        if (transform.localScale.x < 12)
            blobbyMat.mainTexture = blobbyTextures[0];
        else if (transform.localScale.x >= 12 && transform.localScale.x < 30)
            blobbyMat.mainTexture = blobbyTextures[1];
        else if (transform.localScale.x >= 30)
            blobbyMat.mainTexture = blobbyTextures[2];
        foreach (GameObject obj in referencePoints)
        {
            SpringJoint2D[] joints = obj.GetComponents<SpringJoint2D>();
            joints[1].distance = transform.localScale.x / 7.5f;
        }
        foreach (SpringJoint2D joint in centerPoint.GetComponents<SpringJoint2D>())
        {
            joint.distance = transform.localScale.x / 3.8f;
        }
    }
}
