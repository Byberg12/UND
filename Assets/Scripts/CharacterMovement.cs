using UnityEngine;

using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    public enum AnimState
    {
        idle = 0,
        walkForwards = 1,
        walkBackwards = 2,
        run = 3,
        jump = 4,
        strafeLeft = 5,
        strafeRight = 6

    }
    float speed = 6.0f;
    float rotSpeed = 90.0f; // rotate at 90 degrees/second
    float jumpSpeed = 8.0f;
    float gravity = 20.0f;
    float runSpeed = 8.0f;
    float walkSpeed = 6.0f;
    private Animator anim;
    public AnimState animState;

    Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ControllPlayer();
    }

    void ControllPlayer()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // convert direction from local to global space:
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            speed = walkSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            float h = Input.GetAxis("Horizontal") * speed;
            float v = Input.GetAxis("Vertical") * speed;
            if (animState != AnimState.idle)
                animState = AnimState.idle;
            if (h > 0 && animState != AnimState.strafeRight)
                animState = AnimState.strafeRight;
            if (h < 0 && animState != AnimState.strafeLeft)
                animState = AnimState.strafeLeft;
            if (v > 0 && animState != AnimState.walkForwards)
                animState = AnimState.walkForwards;
                if (runSpeed == speed && animState != AnimState.run)
                    animState = AnimState.run;
            if (v < 0 && animState != AnimState.walkBackwards)
                animState = AnimState.walkBackwards;

            anim.SetFloat("forward", v);
            anim.SetFloat("sideways", h);

            if (h != 0 || v != 0)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
            
        }
        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // convert velocity to displacement and move character:
        controller.Move(moveDirection * Time.deltaTime);

    }

}