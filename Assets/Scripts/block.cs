using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

public class block : MonoBehaviour {
    bool Active = true;
    public bool halfRotaion;
    bool gate;
    float moveLeftTime;
    float moveRightTime;
    float moveDownTime;
    float rotateTime;
    Grid grid;

    float dropDelay;


	// Use this for initialization
	void Start () {
        grid = GameObject.Find("Board").GetComponent<Grid>();
        Drop();
        
    }

    

    void Drop() {
        transform.position = new Vector3(transform.position.x,transform.position.y - 1,transform.position.z);
        List<Vector2> blocks = new List<Vector2> {
            new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y),
            new Vector2(transform.GetChild(1).position.x, transform.GetChild(1).position.y),
            new Vector2(transform.GetChild(2).position.x, transform.GetChild(2).position.y),
            new Vector2(transform.GetChild(3).position.x, transform.GetChild(3).position.y) };
        bool isValid = true;
        if (grid.checkGrid(blocks))
        {
            isValid = false;
            print("hit");

        }
        if (!isValid)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
            print("Hit below");

            bool gameover = false;
            Active = false;
            foreach (Transform t in transform)
            {
                if (t.position.y > 20) {
                    gameover = true;
                }
                int x = Convert.ToInt32(t.position.x - 0.5f);
                int y = Convert.ToInt32(t.position.y - 0.5f);
                grid.PlaceBlock(x,y,t);
            }

            if (gameover) {
                grid.GameOver();
            }

            grid.score += 50;
            grid.checkRow();
            grid.DropBlock();
        }
        else
        {
            StartCoroutine("Delay");
        }


        
    }

    IEnumerator Delay() {         
        yield return new WaitForSeconds(dropDelay);
        Drop();
    }

    void checkBlock() {

    }

    void moveLeft() {
        transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        bool isValid = true;
        List<Vector2> blocks = new List<Vector2> {
            new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y),
            new Vector2(transform.GetChild(1).position.x, transform.GetChild(1).position.y),
            new Vector2(transform.GetChild(2).position.x, transform.GetChild(2).position.y),
            new Vector2(transform.GetChild(3).position.x, transform.GetChild(3).position.y) };
        if (grid.checkGrid(blocks)) {
            isValid = false;
        }
        if (!isValid)
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            print("Invalid move");
        }
        else {
            moveLeftTime = Time.time;
        }
    }

    void moveRight() {
        transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        bool isValid = true;
        List<Vector2> blocks = new List<Vector2> {
            new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y),
            new Vector2(transform.GetChild(1).position.x, transform.GetChild(1).position.y),
            new Vector2(transform.GetChild(2).position.x, transform.GetChild(2).position.y),
            new Vector2(transform.GetChild(3).position.x, transform.GetChild(3).position.y) };
        if (grid.checkGrid(blocks))
        {
            isValid = false;
        }
        if (!isValid)
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            print("Invalid move");
        }
        else
        {
            moveRightTime = Time.time;
        }
    }

    void rotate() {
        if (halfRotaion) {
            float z;
            if (gate)
            {
                z = 90;
                gate = false;
            }
            else {
                z = -90;
                gate = true;
            }
            transform.Rotate(0, 0, z);
            bool isValid = true;
            List<Vector2> blocks = new List<Vector2> {
            new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y),
            new Vector2(transform.GetChild(1).position.x, transform.GetChild(1).position.y),
            new Vector2(transform.GetChild(2).position.x, transform.GetChild(2).position.y),
            new Vector2(transform.GetChild(3).position.x, transform.GetChild(3).position.y) };
            if (grid.checkGrid(blocks))
            {
                isValid = false;
                
            }
            if (!isValid)
            {
                transform.Rotate(0,0,z * -1);
                print("Invalid move");
            }
            else
            {
                foreach (Transform t in transform)
                {
                    t.Rotate(0, 0, z * -1);
                }
                rotateTime = Time.time;
            }
        }
        else {
            transform.Rotate(0, 0, -90);
            bool isValid = true;
            List<Vector2> blocks = new List<Vector2> {
            new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y),
            new Vector2(transform.GetChild(1).position.x, transform.GetChild(1).position.y),
            new Vector2(transform.GetChild(2).position.x, transform.GetChild(2).position.y),
            new Vector2(transform.GetChild(3).position.x, transform.GetChild(3).position.y) };
            if (grid.checkGrid(blocks))
            {
                isValid = false;

            }
            if (!isValid)
            {
                transform.Rotate(0, 0, 90);
                print("Invalid move");
            }
            else
            {
                foreach (Transform t in transform)
                {
                    t.Rotate(0, 0, 90);
                }
                rotateTime = Time.time;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        dropDelay = 0.5f;
        if (Active)
        {
            if (Input.GetKeyDown("down")&&Time.time - moveDownTime > .2f) {
                StopCoroutine("Delay");
                Drop();
                moveDownTime = Time.time;
            }
            if (Input.GetKey("down") )
            {
                dropDelay = 0.2f;
                moveDownTime = Time.time;
            }
            else {

            }

            if (Input.GetKey("left") && Time.time - moveLeftTime > .15) { moveLeft(); }

            else if (Input.GetKey("right") && Time.time - moveRightTime > .15) { moveRight(); }

            else if (Input.GetKey("up") && Time.time - rotateTime > .15) { rotate(); }
        }

    }
}
