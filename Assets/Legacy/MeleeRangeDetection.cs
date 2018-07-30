//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MeleeRangeDetection : MonoBehaviour {

//    AIController _ai = null;

//    private void Start()
//    {
//        _ai = GetComponentInParent<AIController>();

//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            _ai.TargetInMeleeRange = true;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            _ai.TargetInMeleeRange = false;
//        }
//    }
//}
