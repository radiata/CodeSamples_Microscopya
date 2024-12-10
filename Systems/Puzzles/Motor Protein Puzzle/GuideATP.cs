using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideATP : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public Vector3 target;
    [SerializeField] public List<Transform> targetList;

    private int iter = 0;

    private void Start()
    {
        if (targetList.Count > 0)
        {
            target = targetList[0].position;
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) <= .2f)
        {
            iter += 1;
            if (iter >= targetList.Count)
            {
                Destroy(transform.parent.gameObject);
                return;
            }

            target = targetList[iter].position;
        }
    }
}
