using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemyGunRotation : MonoBehaviour
{

    public GameObject target;
    //private Vector3 targetVectorARGround;
    private Vector3 targetCameraVector;
    //public float speed = 1.0f;

   // [SerializeField]
    //private ARPlaneManager arPlaneManager;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("MainCamera");
        camera = target.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        target = GameObject.FindWithTag("MainCamera");
        targetCameraVector = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);

        Quaternion targetRotation = Quaternion.LookRotation(transform.position - targetCameraVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.25f * Time.deltaTime);
        //Quaternion relativeRotation = Quaternion.Inverse(gunTransform.rotation) * transform.rotation;
        /*        Quaternion desiredRotation = Quaternion.LookRotation(lookDirection, transform.up) * relativeRotation;
                Vector3 rotationEuler = desiredRotation.eulerAngles;
                //rotationEuler.z = 0;
                rotationEuler.x = 90;
                desiredRotation = Quaternion.Euler(rotationEuler);

                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed);

                Quaternion gunRotation = Quaternion.LookRotation(target.transform.position - gunTransform.position, gunTransform.up);
                gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, gunRotation, rotateSpeed);
         */
        //target.transform.LookAt(new Vector3(target.transform.position.x, 0, 0));
        //Vector3 relativePos = target.transform.position - gunTransform.position;
        //gunTransform.rotation = Quaternion.LookRotation(target.transform.position - gunTransform.position);
        // Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //transform.rotation = newRotation;
        // if (Vector3.Distance(targetVectorARGround, newTransform.position) > 0.5f && (Physics.Raycast(newTransform.position, Vector3.down, 0.1f)))
        //{
        // NEED TO ADD IN DETECTION FOR WHEN THE ENEMY SPAWNS SLIGHTLY INSIDE PLANE
        // Can't just look up, because it hits itself
        // Need to check what it is hitting slightly both up and down and confirm that it is an AR plane, not itself
        //    newTransform.position += newTransform.forward * 0.1f * Time.deltaTime;
        // }
    }
}

