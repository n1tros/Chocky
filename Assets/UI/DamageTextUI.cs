using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextUI : MonoBehaviour {


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        var translation = 3f * Time.deltaTime;
        transform.Translate(Vector3.up * translation);
	}


}
