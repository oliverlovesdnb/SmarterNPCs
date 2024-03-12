using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;

public class WaypointController : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    RaycastHit hit;

    public LayerMask Car;

    private bool roadCheckNext = false;
    private bool roadClear = false;
    private bool carHit = false;
    private bool rotating = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextPoint();
    }

    void Update()
    {
        if (rotating == true){
            return;
        }

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            if (roadCheckNext)
            {
                RoadCheckNext();
                return;
            }
            if (waypoints[currentWaypointIndex].transform.name.ToString().Contains("RoadCheck"))
            {
                Debug.Log(currentWaypointIndex);
                Debug.Log("Road Check Next!");
                roadCheckNext=true;

            }
            NextPoint();
        }
    }

    bool CheckCarHit()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 30f, Car))
        {
            if (hit.collider.CompareTag("Car")) carHit = true;
        }
        return carHit;
    }


    void RoadCheckNext()
    {
        Debug.Log("Road Check Now!");
        roadClear = false;
        agent.isStopped = true;

        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        RoadCheck();

        Debug.Log("Escaped RoadCheck");
        agent.isStopped = false;
        roadCheckNext = false;
    }

    void NextPoint()
    {
        if (waypoints.Length == 0) return;
        
        
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

       

    }

    void RoadCheck()
    {
        StartCoroutine(RotateLeftAndRight());
    }
    IEnumerator RotateLeftAndRight()
    {
        // Rotate left
        yield return StartCoroutine(Rotate(Quaternion.Euler(0, -90, 0)));

        // Rotate right
        yield return StartCoroutine(Rotate(Quaternion.Euler(0, 90, 0)));
        if (carHit) 
        {
            Debug.Log("AAH CAR!");
            RoadCheck();
        }
        else rotating = false;
    }

    IEnumerator Rotate(Quaternion targetRotation)
    {
        carHit = false;
        rotating = true;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0;
        float duration = 1f; // Adjust as needed

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            CheckCarHit();
            yield return null;
        }
        

        transform.rotation = targetRotation;
        
    }
}