using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public bool head;
    public bool upperBody;
    public bool lowerBody;
    public bool feet;

    public float angle;
    public float topAngle;
    public float bottomAngle;
    public float angleSpeed;
    public float walkSpeed;
    public float jumpSpeed;
    public float jumpHeight;
    public float superFallSpeed;
    public float originalSpeed;
    public float tempHeight = 0.0f;

    public float timer;
    public float timerRate;
    public float timer2;
    public float timerRate2;
    public float xPosition;
    public float xLBoundary;
    public float xRBoundary;

    public bool isWalking;
    public bool nodding = true;
    public bool downNod;
    public bool right;
    public bool jumping;
    public bool jumpingForwards;
    public bool falling;
    public bool fallingForwards;
    public bool superFalling;
    public bool stunned;
    public bool lookDown;


    public float camAngle = 0;


    // Use this for initialization
    void Start()
    {
        superFallSpeed = originalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Boundary();
        LifeSavers();
        Jumping();
        JumpingForwards();
        GroundPound();
        if (stunned)
        {
            Stunned();
        }
        if (isWalking)
        {

            if (nodding)
            {
                HeadNod();
            }

            Walking();

        }

    }

    public void HeadNod()
    {
        if (!right & !stunned)
        {
            if (angle < topAngle)
            {
                downNod = true;
            }
            else if (angle > bottomAngle)
            {
                downNod = false;
            }

            if (downNod)
            {

                angle = angle + angleSpeed * 0.01f;

            }
            else if (!downNod)
            {

                angle = angle - angleSpeed * 0.01f;

            }
        }
        else if (right && !stunned)
        {
            if (angle > topAngle)
            {
                downNod = true;
            }
            else if (angle < bottomAngle)
            {
                downNod = false;
            }

            if (downNod)
            {

                angle = angle + angleSpeed * 0.01f;

            }
            else if (!downNod)
            {

                angle = angle - angleSpeed * 0.01f;

            }
        }

    }

    public void Walking()
    {

        if (Input.GetKey("d") && !right && !jumpingForwards && !fallingForwards && !superFalling && !stunned)
        {
            DirectionChange();
            right = true;
        }
        else if (Input.GetKey("a") && right && !jumpingForwards && !fallingForwards && !superFalling && !stunned)
        {
            DirectionChange();
            right = false;
        }

        if (feet && !stunned)
        {

            if (right)
            {
                xPosition = xPosition + walkSpeed * 0.1f;
                gameObject.GetComponent<Limb>().MoveByOffset(new Vector3(walkSpeed * 0.1f, 0.0f, 0.0f));

            }
            else if (!right)
            {
                xPosition = xPosition - walkSpeed * 0.1f;
                gameObject.GetComponent<Limb>().MoveByOffset(new Vector3(-walkSpeed * 0.1f, 0.0f, 0.0f));

            }

        }

    }

    public void DirectionChange()
    {

        angle = -angle;
        topAngle = -topAngle;
        bottomAngle = -bottomAngle;
        angleSpeed = -angleSpeed;
    }

    public void Jumping()
    {

        if (Input.GetKey("w") && tempHeight <= 0.0f && !stunned)
        {

            jumping = true;

        }

        if (feet && !stunned)
        {

            if (jumping)
            {

                tempHeight = tempHeight + jumpSpeed * 0.01f;

                gameObject.GetComponent<Limb>().MoveByOffset(new Vector3(0.0f, jumpSpeed * 0.1f, 0.0f));
                if (tempHeight > jumpHeight)
                {

                    jumping = false;
                    falling = true;

                }
            }
            if (falling && !stunned)
            {

                tempHeight = tempHeight + -jumpSpeed * 0.01f;

                gameObject.GetComponent<Limb>().MoveByOffset(new Vector3(0.0f, -jumpSpeed * 0.1f, 0.0f));
                if (tempHeight <= 0.0f)
                {

                    falling = false;

                }
            }
        }
    }

    public void JumpingForwards()
    {

        if (Input.GetKey("s") && tempHeight <= 0.0f && !stunned)
        {

            jumpingForwards = true;

        }
        if (feet)
        {

            if (jumpingForwards && !stunned)
            {

                tempHeight = tempHeight + jumpSpeed * 0.01f;

                gameObject.GetComponent<Limb>().MoveByOffset(new Vector3(0.0f, jumpSpeed * 0.1f, 0.0f));
                if (tempHeight > jumpHeight)
                {

                    jumpingForwards = false;
                    fallingForwards = true;

                }
            }
            if (fallingForwards && !stunned)
            {

                tempHeight = tempHeight + -jumpSpeed * 0.01f;

                gameObject.GetComponent<Limb>().MoveByOffset(new Vector3(0.0f, -jumpSpeed * 0.1f, 0.0f));
                if (tempHeight <= 0.0f)
                {

                    fallingForwards = false;

                }
            }
        }

    }

    public void GroundPound()
    {

        if (Input.GetKey("z") && tempHeight > 0.1f && !stunned)
        {

            superFalling = true;
            jumping = false;
            falling = false;
            jumpingForwards = false;
            fallingForwards = false;
            isWalking = false;
        }

        if (feet)
        {

            if (superFalling)
            {

                tempHeight = tempHeight + -superFallSpeed * 0.01f;


                gameObject.GetComponent<Limb>().MoveByOffset(new Vector3(0.0f, -superFallSpeed * 0.1f, 0.0f));
                if (tempHeight <= 0.0f)
                {

                    superFalling = false;
                    stunned = true;
                    lookDown = true;
                    CameraShake();

                }
                superFallSpeed = superFallSpeed + 0.1f;
            }
            else
            {

                superFallSpeed = originalSpeed;

            }

        }
    }

    public void Stunned()
    {

        if (stunned)
        {
            if (upperBody)
            {
                if (!right && angle <= 2 && lookDown)
                {

                    angle = angle + 0.02f;

                }
                else if (right && angle >= -2 && lookDown)
                {

                    angle = angle - 0.02f;

                }
                else if (!right && angle >= 2)
                {

                    lookDown = false;

                }
                else if (right && angle <= -2)
                {

                    lookDown = false;

                }

                if (timer < Time.time)
                {
                    Invoke("Unstunned", 3);
                    timer = Time.time + timerRate;
                }
                //if(timer2 < Time.time) {
                Invoke("LookUp", 2);
                //timer2 = Time.time + timerRate2;
                //}


            }
        }
    }

    public void Unstunned()
    {

        Debug.Log("did it");
        GameObject.Find("Feet").GetComponent<Movement>().stunned = false;
        lookDown = true;

    }

    public void LookUp()
    {

        if (upperBody)
        {
            if (!right && angle >= 1)
            {

                angle = angle - 0.02f;

            }
            else if (right && angle <= -1)
            {

                angle = angle + 0.02f;

            }
        }

    }

    public void Boundary()
    {

        if (xPosition > xRBoundary)
        {

            DirectionChange();
            right = false;

        }
        else if (xPosition < xLBoundary)
        {

            DirectionChange();
            right = true;

        }

    }

    public void CameraShake()
    {

        if (xPosition > 0)
        {
            GameObject camera = GameObject.Find("Main Camera");
            
            if(camAngle < 45)
            {
                camera.transform.Rotate(0, 0, 2);
                camAngle += 2;
            }
            

        }

        else if (xPosition < 0)
        {

            GameObject camera = GameObject.Find("Main Camera");

            if (camAngle > -45)
            {
                camera.transform.Rotate(0, 0, -2);
                camAngle -= 2;
            }

        }
    }

    public void LifeSavers()
    {

        if (GameObject.Find("Feet").GetComponent<Movement>().jumping)
        {

            jumping = true;

        }
        else if (!GameObject.Find("Feet").GetComponent<Movement>().jumping)
        {

            jumping = false;

        }
        if (GameObject.Find("Feet").GetComponent<Movement>().falling)
        {

            falling = true;

        }
        else if (!GameObject.Find("Feet").GetComponent<Movement>().falling)
        {

            falling = false;

        }
        if (GameObject.Find("Feet").GetComponent<Movement>().jumpingForwards)
        {

            jumpingForwards = true;

        }
        else if (!GameObject.Find("Feet").GetComponent<Movement>().jumpingForwards)
        {

            jumpingForwards = false;

        }
        if (GameObject.Find("Feet").GetComponent<Movement>().fallingForwards)
        {

            fallingForwards = true;

        }
        else if (!GameObject.Find("Feet").GetComponent<Movement>().fallingForwards)
        {

            fallingForwards = false;

        }


        if (GameObject.Find("Feet").GetComponent<Movement>().superFalling)
        {

            superFalling = true;

        }
        else if (!GameObject.Find("Feet").GetComponent<Movement>().superFalling)
        {

            superFalling = false;

        }


        if (GameObject.Find("Feet").GetComponent<Movement>().stunned)
        {

            stunned = true;

        }
        else if (!GameObject.Find("Feet").GetComponent<Movement>().stunned)
        {

            stunned = false;

        }

        if (GameObject.Find("Upper Body").GetComponent<Movement>().lookDown)
        {

            lookDown = true;

        }
        else if (!GameObject.Find("Upper Body").GetComponent<Movement>().lookDown)
        {

            lookDown = false;

        }

        if (!feet)
        {

            xPosition = GameObject.Find("Feet").GetComponent<Movement>().xPosition;

        }

        if (jumping || falling || superFalling || stunned)
        {

            isWalking = false;

        }
        else
        {

            isWalking = true;
        }

    }

}
