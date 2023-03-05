using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
 private void OnCollisionEnter(Collision col)
 {
    if(col.gameObject.name == "Boat")
    {
        SceneManager.LoadScene(5);
        Debug.Log("Win");
    }
 }
}
 

