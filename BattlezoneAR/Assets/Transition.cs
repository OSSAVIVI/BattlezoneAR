using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    
    
    void Start()
    {
        StartCoroutine(ChangeScreen2Seconds());

    }

   IEnumerator ChangeScreen2Seconds()
    {
        
        yield return new WaitForSeconds(2);
        

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
   
}
