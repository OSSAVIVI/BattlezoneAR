using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	//public GameObject player;
    public GameObject target;
    // Start is called before the first frame update
    public Rigidbody[] projectile;
    //public Transform shotSpawn;
    bool shotFired = false;
    void Start()
    {
        //target = GameObject.FindWithTag("Player");
    }

    public void Shoot()
    {
        Rigidbody shot = Instantiate(projectile[0], transform.position, transform.rotation);
        shot.AddForce(transform.forward * 1500f);
        SoundManagerScript.playShotSound();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("MainCamera");

        if (Vector3.Distance(transform.position, target.transform.position) > 1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
            transform.position += transform.forward * 1f * Time.deltaTime;
        }

        // This tells us if the two objects are facing one another

        Vector3 dirFromAtoB = (target.transform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
 
        if(dotProd > 0.95 && !shotFired){
            shotFired = true;
            Debug.Log("looking at");
            // ObjA is looking mostly towards ObjB
            //Shoot();
        }
    }
}
