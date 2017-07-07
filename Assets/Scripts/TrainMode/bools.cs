using UnityEngine;
using System.Collections;

public class bools : MonoBehaviour {

    public float time;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime; //時間增加
        if (time > 0.1f) //如果時間大於0.1(秒)
            Destroy(gameObject);
    }
}
