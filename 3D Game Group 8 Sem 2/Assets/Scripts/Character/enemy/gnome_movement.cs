using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important


public class gnome_movement : MonoBehaviour 
{
    public NavMeshAgent agent;
    public float range; //radius of sphere

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    [Header("Animation")]
    [SerializeField] private Animator animator;
    private CharacterController controller;
    private Vector2 moveInput;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();

        if (animator == null) animator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }
        if (animator != null)
        {
            bool isMoving = moveInput.sqrMagnitude > 0.01f;
            animator.SetBool("IsWalking", isMoving);
        }

    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }


}