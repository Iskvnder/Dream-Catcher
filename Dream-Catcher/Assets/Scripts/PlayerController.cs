using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

   
    private CharacterController characterController;
    public float mouseSensivity = 2f;
    private float characterRotationLeftRight;
    private float vertInput;
    private float horizInput;
    private Camera cam;
    public int dreamCatched = 0;
    private Animator playerAnimator;
    public ParticleSystem particles;
    
    [SerializeField]
    private float movementSpeed = 2.5f;
    private bool isMovementEnabled = true;
    private string[] AnimationsArray = {"Idle", "Right", "Left", "WalkForward","WalkBackward", "Run", "RightRun", "LeftRun", "Punch"};
    public AudioSource audioSource;
    public AudioClip audioClip;    
    


    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }


    void Update() {
        MovingCharacter();
        AnimationOn();
    }

    private void MovingCharacter() {

        if (!isMovementEnabled) {
        return; // Do nothing if movement is disabled
        }

        if(Time.timeScale == 0){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }else {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        }

        if(Time.timeScale != 0f){
        characterRotationLeftRight = Input.GetAxis("Mouse X") * mouseSensivity;
        transform.Rotate(0, characterRotationLeftRight, 0); //Rotate whole character with camera view.
        }
        vertInput = Input.GetAxis("Vertical"); // W, S
        horizInput = Input.GetAxis("Horizontal"); // A, D

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 sideMovement = transform.right * horizInput;

        characterController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + sideMovement, 1.0f) * movementSpeed);


        // speed = new Vector3.ClampMagnitude(sideSpeed, 1.0f, forwardSpeed) * movementSpeed;
        // speed = transform.rotation * speed; //  Moving towards camera view.
        // characterController.Move(speed * Time.deltaTime);

    }

   


    void AnimationOn(){
        if(playerAnimator != null){
            
            movementSpeed = 2.5f;

            AnimationCheck(0);

            if(Input.GetKey(KeyCode.W))
            {
                //Run
                if(Input.GetKey(KeyCode.LeftShift)){
                movementSpeed = 6f;
                AnimationCheck(5);
                }else{

                //Move
                AnimationCheck(3);
                }
                
            }

            if(Input.GetKey(KeyCode.S))
            {
                //MoveBackward
                AnimationCheck(4);
            }

            if(Input.GetKey(KeyCode.A))
            {
                //RunLeft
                if(Input.GetKey(KeyCode.LeftShift)){

                movementSpeed = 4f;
                AnimationCheck(7);
                }else{

                //MoveLeft
                AnimationCheck(2);
                }
            }

            
            if(Input.GetKey(KeyCode.D))
            {
                //RunRight
                if(Input.GetKey(KeyCode.LeftShift)){

                movementSpeed = 4f;
                AnimationCheck(6);
                }else{

                //MoveRight
                AnimationCheck(1);
                }
        }
        
    }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BadDream") || other.CompareTag("Boss"))
        {
            //Sound
            audioSource.PlayOneShot(audioClip);

            //Effect
            if (particles != null) {
                particles.Play();
            }
            StartCoroutine(ParticleCoroutine(particles, 0.7f));
            //Punch
            AnimationCheck(8);

            isMovementEnabled = false; // Disable movement when the player collides with the dream
            StartCoroutine(EnableMovementAfterDelay(0.8f)); // Re-enable movement after a delay of 0.7 seconds
    
            Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
            if (otherRigidbody != null) {
            otherRigidbody.velocity = Vector3.zero;
            }

            // Destroy the dream and increase score
            StartCoroutine(PunchCoroutine(other.gameObject, 0.7f));
            if (other.CompareTag("Boss")){dreamCatched = dreamCatched + 50;}else dreamCatched++;
            
        }
    }

    IEnumerator PunchCoroutine(GameObject objectToDestroy, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }

    IEnumerator ParticleCoroutine(ParticleSystem particle, float delay) {
        yield return new WaitForSeconds(delay);
        particle.Stop();
    }

    IEnumerator EnableMovementAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        isMovementEnabled = true;
    }

    private void AnimationCheck(int numOfAnimation){

            for(int i = 0; i < AnimationsArray.Length; i++){
                if(i == numOfAnimation){
                    playerAnimator.SetTrigger(AnimationsArray[numOfAnimation]);
                    
                } else if(i!= 8)
                playerAnimator.ResetTrigger(AnimationsArray[i]);
            }
    }

}