using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class playerMovement : MonoBehaviour
{

    [SerializeField]private float speed = 10f;

    [SerializeField] private float gravity = -10f;

    [SerializeField] private float jumpforce = 10f;

    private CharacterController characterController;

    float yvelocity;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = (transform.right*horizontal+transform.forward*vertical).normalized;
        movement*=speed;

        if (characterController.isGrounded &&  yvelocity < 1)
        {
            yvelocity = 0f; 
        }
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            yvelocity = gravity;
            yvelocity = jumpforce;
        }
        yvelocity += gravity * Time.deltaTime;
        movement.y= yvelocity;

        characterController.Move(movement * Time.deltaTime);
    }
}
