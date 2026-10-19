using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class GuideTrail : MonoBehaviour
{
    public Transform player;
    public float refreshSeconds = 0.25f;
    public float scrollSpeed = 1.5f; // Makes the sparkles flow toward the goal
    public float height = 0.4f; // Lifts the line off the ground

    LineRenderer line;
    Transform target;
    float timer;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    public void SetTarget(Transform t)
    {
        target = t;
        line.enabled = (t != null);
        timer = 0f;
    }

    public void Clear() { SetTarget(null); }

    void Update()
    {
        if (target == null) return;

        // Animate the sparkles
        line.material.mainTextureOffset -= new Vector2(scrollSpeed * Time.deltaTime, 0f);

        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = refreshSeconds;

        NavMeshPath path = new NavMeshPath();
        Vector3[] points;
        if (NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, path) && path.corners.Length > 1)
            points = path.corners;
        else
            points = new Vector3[] { player.position, target.position };

        line.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
            line.SetPosition(i, points[i] + Vector3.up * height);
    }
}