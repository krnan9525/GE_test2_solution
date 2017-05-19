using UnityEngine;
using System.Collections;

public class Hive : MonoBehaviour {
    public GameObject bee_perfeb;

    public int pollen_left = 10;
    public float current_cycle_time = 0.0f;
    public float flower_radius = 50.0f;
	// Use this for initialization
	void Start () {
        pollen_left = 10;
	}
	
	// Update is called once per frame
	void Update () {
        current_cycle_time += Time.deltaTime;
        GameObject[] bees = GameObject.FindGameObjectsWithTag("bee");
        if (current_cycle_time > 1.0 && pollen_left >=5 && bees.Length <= 10)
        {
            current_cycle_time = 0;
            //Create a bee
            current_cycle_time += Time.deltaTime;
            GameObject bee = GameObject.Instantiate<GameObject>(bee_perfeb);
            BeeBehaviour bee_behaviour = bee.GetComponent<BeeBehaviour>();
            bee_behaviour.center_pos = this.transform.position;
            bee_behaviour.radius = flower_radius;
            bee.transform.parent = this.transform;
            pollen_left -= 5;
        }
        else
        {
            current_cycle_time += Time.deltaTime;
        }
	}
}
