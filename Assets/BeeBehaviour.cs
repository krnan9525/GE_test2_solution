using UnityEngine;
using System.Collections;

public class BeeBehaviour : MonoBehaviour {
    public enum State {Wander, Approach, Arrive, Gather, Return};
    public int curried_polen = 0;
    private Flower attached_flower;
    private GameObject attached_flower_object;
    public Vector3 target_pos;
    public float approach_range = 10.0f;
    public float max_speed = 5.0f;
    public float speed = 0.0f;
    
    public float radius = 50;
    public Vector3 center_pos;
    public float collection_range = 2.0f;
    public float safe_collect_distance = 3.0f;

    public State current_state;

    private float collect_eslaped = 0.0f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,target_pos);
    }

    // Use this for initialization
    void Start () {
        current_state = State.Wander;
        Vector3 random_point = Random.onUnitSphere * Random.Range(approach_range+1.0f, radius);
        target_pos = new Vector3(random_point.x, transform.position.y, random_point.z);
        StartCoroutine(BeeState());
    }
	
	// Update is called once per frame
	void Update () {
	    if(current_state == State.Wander)
        {
            speed = max_speed;
            //Face the target first
            if((transform.right - (target_pos - transform.position).normalized).magnitude > 0.04f)
            {
                this.transform.right = Vector3.Lerp(this.transform.right, (target_pos - transform.position).normalized, Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(speed* Time.deltaTime,0,0));
            }
        }
        if(current_state == State.Approach)
        {
            float dis = Vector3.Distance(transform.position, target_pos);
            speed = max_speed * (dis / approach_range);
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            if(dis < 0.5f)
            {
                Vector3 random_point = center_pos + Random.onUnitSphere * Random.Range(1.0f, radius);
                target_pos = new Vector3(random_point.x, transform.position.y, random_point.z);
                current_state = State.Wander;
            }
        }
        if(current_state == State.Arrive)
        {
            float dis = Vector3.Distance(transform.position, target_pos) - safe_collect_distance;
            if ((transform.right - (target_pos - transform.position).normalized).magnitude > 0.04f)
            {
                this.transform.right = Vector3.Lerp(this.transform.right, (target_pos - transform.position).normalized, Time.deltaTime);
            }
            else if(dis > 1.5f)
            {
                speed = max_speed * (dis / (collection_range - safe_collect_distance));
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            else if(dis <= 1.5f)
            {
                current_state = State.Gather;
            }
        }
        if(current_state == State.Gather)
        {
            attached_flower.is_attached();
            collect_eslaped += Time.deltaTime;
            if(collect_eslaped >= 1.0f)
            {
                collect_eslaped = 0;
                curried_polen++;
                attached_flower.polen--;
            }
            if(attached_flower.polen == 0)
            {
                GameObject.Destroy(attached_flower_object);
                current_state = State.Return;
                target_pos = center_pos;
            }
        }
        if(current_state == State.Return)
        {
            speed = max_speed;
            if ((transform.right - (target_pos - transform.position).normalized).magnitude > 0.04f)
            {
                this.transform.right = Vector3.Lerp(this.transform.right, (target_pos - transform.position).normalized, Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            if(Vector3.Distance(target_pos, this.transform.position) < 0.5f)
            {
                GameObject hive = GameObject.FindGameObjectWithTag("hive");
                Hive hive_component = hive.GetComponent<Hive>();
                hive_component.pollen_left += this.curried_polen;
                this.curried_polen = 0;
                current_state = State.Wander;
            }
        }
	}

    System.Collections.IEnumerator BeeState()
    {
        while (true)
        {
            //switch to approach
            if (current_state == State.Wander && Vector3.Distance(transform.position, target_pos) < approach_range)
                current_state = State.Approach;
            if((current_state == State.Wander || current_state == State.Approach))
            {
                //Determine if the flower is within collection range
                GameObject[] flowers = GameObject.FindGameObjectsWithTag("flower");
                for(int i = 0; i < flowers.Length; i++)
                {
                    Flower flower_behaviour = flowers[i].GetComponent<Flower>();
                    if ((!flower_behaviour.occupied) && Vector3.Distance(this.transform.position, flowers[i].transform.position) < collection_range)
                    {
                        flower_behaviour.occupied = true;
                        attached_flower = flower_behaviour;
                        attached_flower_object = flowers[i];
                        current_state = State.Arrive;
                        target_pos = new Vector3( flowers[i].transform.position.x, this.transform.position.y, flowers[i].transform.position.z);
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }

    }
}
