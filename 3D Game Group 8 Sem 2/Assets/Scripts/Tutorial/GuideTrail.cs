using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class GuideTrail : MonoBehaviour
{
    public Transform player;
    public float refreshSeconds = 0.25f;
    public float scrollSpeed = 1.5f;   // Makes a sparkly texture flow toward the goal

    [Header("Look")]
    public float width = 1f;
    public float height = 1f;          // Lifts the line off the ground. Raise it if the line sinks into the terrain.
    public Color color = new Color(1f, 0.85f, 0.3f, 1f);

    LineRenderer line;
    Transform target;
    float timer;
    float nextLog;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        ApplyLook();

        if (line.sharedMaterial == null)
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader != null)
            {
                line.material = new Material(shader);
                Debug.LogWarning("[Trail] The Line Renderer had no material, so a plain one is being used. " +
                                 "Assign a material to the Line Renderer for a sparkle look.");
            }
        }

        line.enabled = false;
    }

    void ApplyLook()
    {
        line.widthCurve = AnimationCurve.Constant(0f, 1f, 1f);
        line.widthMultiplier = width;
        line.startColor = color;
        line.endColor = color;
    }

    public void SetTarget(Transform t)
    {
        target = t;
        line.enabled = (t != null);
        timer = 0f;
        Debug.Log("[Trail] " + (t != null ? "Leading to: " + t.name + " at " + t.position : "Cleared"));
    }

    public void Clear() { SetTarget(null); }

    void Update()
    {
        if (target == null) return;

        if (player == null)
        {
            if (Time.time >= nextLog)
            {
                nextLog = Time.time + 3f;
                Debug.LogError("[Trail] No player assigned to the GuideTrail (and the Director couldn't fill it in).");
            }
            return;
        }

        // Animate a scrolling texture, if the material has one
        if (line.sharedMaterial != null)
            line.material.mainTextureOffset -= new Vector2(scrollSpeed * Time.deltaTime, 0f);

        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = refreshSeconds;
        ApplyLook(); // able to tweak Width / Color live while testing

        NavMeshPath path = new NavMeshPath();
        Vector3[] points;
        bool usedNavMesh = false;
        if (NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, path) && path.corners.Length > 1)
        {
            points = path.corners;
            usedNavMesh = true;
        }
        else
        {
            points = new Vector3[] { player.position, target.position };
        }

        line.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
            line.SetPosition(i, points[i] + Vector3.up * height);

        if (Time.time >= nextLog)
        {
            nextLog = Time.time + 5f;
            Debug.Log("[Trail] Drawing " + points.Length + " points" + (usedNavMesh ? " (NavMesh path)" : " (straight line: no NavMesh path found)") +
                      ", width " + width + ", from " + points[0] + " to " + points[points.Length - 1]);
        }
    }
}