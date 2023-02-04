using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PlayerState {idle, sniffing, rooting };


public class PlayerMovement : MonoBehaviour
{
    //used to get the direction the camer is facing
    public Transform cameraTransform;
    Vector3 controllerInput = Vector3.zero;
    public float moveSpeed = 10f;
    //the charactercontroller
    CharacterController cc;

    //gravity and yvelocity
    public float gravity = -15f;
    float yVelocity = 0f;

    public Animator animator;

    bool sniffing = false;
    bool rooting = false;

    GameManager gameManager;

    public GameObject closestTruffle = null;

    public ParticleSystem particleSystem;
    ParticleSystem.EmissionModule em;

    //the audio manager uses this to play the right sounds   
    public PlayerState playerState = PlayerState.idle;


    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        //cache it for later use
        cc = GetComponent<CharacterController>();
        em = particleSystem.emission;
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        controllerInput = new  Vector3(Input.GetAxis("Horizontal"), 0 ,Input.GetAxis("Vertical"));
        controllerInput = Vector3.ClampMagnitude(controllerInput, 1.0f);
        animator.SetFloat("speed", controllerInput.magnitude);
        //get the move relative to the camera rotation
        Vector3 move = GetCameraRotation() * (controllerInput * moveSpeed);
        //turn to face in the direction of move if we are moving
        if (move.sqrMagnitude>0.1f && !sniffing)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(move), 180f * Time.deltaTime);
        }
        
      /*  //deal with jumping and gravity
        if (cc.isGrounded)
        {
            yVelocity = 0;
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = 15f;
            }
        }*/
        //apply gravity
        yVelocity += gravity * Time.deltaTime;

        //add gravity to move vector
        move.y = yVelocity;

        if (!sniffing && !rooting)
        {
            //actually move based on movement and gravity
            cc.Move(move * Time.deltaTime);
        }
        

        if (Input.GetMouseButtonDown(1) && !sniffing)
        {
            sniffing = true;
            animator.SetBool("sniffing", sniffing);
            closestTruffle = gameManager.FindNearestTruffle(transform.position);
            playerState = PlayerState.sniffing;
        }
        if (Input.GetMouseButtonUp(1) && sniffing)
        {
            sniffing = false;
            animator.SetBool("sniffing", sniffing);
            playerState = PlayerState.idle;
        }
        if (Input.GetMouseButtonDown(0) && !sniffing && !rooting)
        {
            closestTruffle = gameManager.FindNearestTruffle(transform.position);
            rooting = true;
            animator.SetBool("rooting", rooting);
            playerState = PlayerState.rooting;
            em.rateOverTime = 40f;
            if (closestTruffle)
            {
                float d = Vector3.SqrMagnitude(closestTruffle.transform.position - transform.position);
                if (d < 5)
                {
                    //we are close enough to the closest truffle
                    closestTruffle.SendMessage("StartDigging");
                }
            }
        }
        if (rooting && Input.GetMouseButtonUp(0))
        {
            rooting = false;
            animator.SetBool("rooting", rooting);
            playerState = PlayerState.idle;
            if(closestTruffle)
            {
                closestTruffle.SendMessage("StopDigging");
            }            
            em.rateOverTime = 0;
        }
        if (sniffing)
        {
            Vector3 vectorToTruffle = (closestTruffle.transform.position - transform.position);
            vectorToTruffle.y = 0;
            vectorToTruffle.Normalize();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vectorToTruffle), 60f * Time.deltaTime);
        }
    }

    Quaternion GetCameraRotation()
    {
        //get forward of camera, zero the y component and return normalised vector
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return Quaternion.LookRotation(forward.normalized);
    }



}
