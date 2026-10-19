using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuideNPC : MonoBehaviour
{
    public Transform player;
    public float waitDistance = 8f; // Tuna stops if player is too far behind
    public float resumeDistance = 5f; // Tuna carries on is player is closer
    public Animator animator; // Optional and needs a float parameter called "Speed"

    NavMeshAgent agent;

    void Awake() { agent = GetComponent<NavMeshAgent>(); }

    void Update()
    {
        if (animator) animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    // Walk to a spot, If waitForPlayer is true, pause whenever the player is too far away.
    public IEnumerator GoTo(Transform destination, bool waitForPlayer = false)
    {
        agent.isStopped = false;
        agent.SetDestination(destination.position);
        bool waiting = false;

        while (true)
        {
            if (waitForPlayer)
            {
                float d = Vector3.Distance(transform.position, player.position);
                if (d > waitDistance) waiting = true;
                else if (d < resumeDistance) waiting = false;

                agent.isStopped = waiting;
                if (waiting) FacePlayer();
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.3f) break;
            yield return null;
        }
        agent.isStopped = true;
    }

    // Walk a route of waypoints in order.
    public IEnumerator LeadAlong(Transform[] route)
    {
        foreach (Transform stop in route)
            yield return GoTo(stop, true);
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
    }
}