using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (GunController))]
[RequireComponent (typeof (TrailRenderer))]
[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour
{   [SerializeField]
    [Range(1f, 10f)]
    float speed = 5f;  

    [SerializeField]float dashSpeed = 30f;
    [SerializeField]float dashingTime = 0.05f;
    [SerializeField]float dashingCooldown = 3f;
    float startedDashTime;
    bool canDash = true;
    
    TrailRenderer tr;
    Camera viewCamera;
    PlayerController controller;
    GunController gunController;

    void Start()
    {   
        tr = GetComponent<TrailRenderer> ();
        controller = GetComponent<PlayerController> ();
        gunController = GetComponent<GunController> ();
        viewCamera = Camera.main;
    }

    void Update()
    {   
        
        moveInput();
        cursorInput();
        dashInput();
        gunInput();
    }

    Vector3 velocityCalculator(float _speed)
    {   
        Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * _speed;
        return velocity;
    }

    void moveInput()
    {
        controller.Move(velocityCalculator(speed));
    }

    void cursorInput()
    {
        
        Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
        Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        
        }
    }

    void dashInput()
    {   
        if(Input.GetKey(KeyCode.Space) &&  canDash)
        {   
             StartCoroutine(dashCoroutine());
    
        }
    }

    void gunInput()
    {
        if(Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }

    IEnumerator dashCoroutine()
    {
        controller.Move(velocityCalculator(dashSpeed));
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        canDash=false;
        yield return new WaitForSeconds(dashingCooldown); 
        canDash =true;        
    }
}
