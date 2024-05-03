using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitboxArea : MonoBehaviour
{
    private bool ballInHitbox = false;
    private GameObject ballInside;
    public TextMeshProUGUI hitBallMsg;
    private float messageDuration = 2f;
    [SerializeField]
    private KeyCode desiredKey; 
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ball")
        {
            ballInHitbox = true;
            ballInside = other.gameObject;
            //Debug.Log("ball in");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ball")
        {
            ballInHitbox = false;
            ballInside = null;
            hitBallMsg.gameObject.SetActive(false);
            StartCoroutine(DisplayMessage("MISS", messageDuration));
            //Debug.Log("ball out");
        }
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(desiredKey)) { Debug.Log(desiredKey + " pressed!"); }
        if (ballInHitbox && Input.GetKeyDown(desiredKey))
        {
            //Debug.Log("Key " + desiredKey +  " pressed while a ball is inside the hitbox area");
            if (ballInside != null) 
            {
                ballInside.SetActive(false);
                StartCoroutine(DisplayMessage("PERFECT", messageDuration));
            }
            ballInside = null; 
        }
    }

    private IEnumerator DisplayMessage(string message, float duration)
    {
        hitBallMsg.text = message;
        hitBallMsg.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        hitBallMsg.gameObject.SetActive(false);
    }
}
