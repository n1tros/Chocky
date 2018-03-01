using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

    public Weapon myPistol;

	// Use this for initialization
	void Start () {
        myPistol = FindObjectOfType<PistolWeapon>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
