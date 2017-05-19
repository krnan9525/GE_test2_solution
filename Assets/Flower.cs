using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {
    public float polen = 0;
    private float polen_before;
    public bool occupied = false;
	// Use this for initialization
	void Start () {
        polen = Random.Range(1, 5);
        polen_before = polen;
        transform.localScale = new Vector3(1, 0, 1);
    }
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.Lerp(
            transform.localScale, new Vector3(1, polen, 1), Time.deltaTime);
        Vector3 pos = transform.position;
        pos.y = transform.localScale.y / 2;
        transform.position = pos;
	}

    public void is_attached()
    {
        transform.localScale = Vector3.Lerp(
              transform.localScale, new Vector3(1, 0, 1), Time.deltaTime/polen_before);
    }
}
