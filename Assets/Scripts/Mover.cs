using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float maxSpeed = 0.5f; // абсолютное значение максимальной скорости (при движкнии назад -maxSpeed)
    [SerializeField]
    private float acceleration = 0.05f; // ускорение
    [SerializeField]
    private float friction = 0.01f; // трение. на каждом апдейте это значение отнимается от скорости (или прибавляется, если скорость отрицательна)
    // все то же, только для угла поворота
    [SerializeField]
    private float stepRotation = 0.2f;
    [SerializeField]
    private float frictionRotation = 0.1f;
    [SerializeField]
    private float maxRotation = 3f;



    private float curSpeed = 0f; // текущая скорость
    private float curRotation = 0f; // текущий угол поворота
    
    private bool collisionExists = false; // присутствует ли столкновение в данный момент

    
    public void FixedUpdate()
    {   
        // пока присутствует коллизия - игнорируем нажатия клавиш
        if (!collisionExists) {
            UpdateSpeed();
            UpdateAngle();
        }

        // если машинка движется назад, стрелка влево поворачивает направо и наоборот (как колеса в машине)
        // + если скорость = 0 - машинка не может поворачивать
        float ang = curRotation;
        if (curSpeed != 0f) {
            ang *= curSpeed / System.Math.Abs(curSpeed);
            transform.Rotate(Vector3.up, ang);
        }
            
        transform.Translate(new Vector3(-curSpeed, 0f, 0f));
    }

    void OnCollisionEnter(Collision collision)
    {
        curSpeed = -curSpeed;
        curRotation = -curRotation;

        collisionExists = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionExists = false;
    }

    private void UpdateSpeed()
    {
        bool need_add_friction = true;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            curSpeed = MathClamp(curSpeed + acceleration - friction, 0, maxSpeed);
            need_add_friction = false;

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("Can not move back with button W pressed");
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            curSpeed = MathClamp(curSpeed - acceleration + friction, -maxSpeed, 0);
            need_add_friction = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (curSpeed > 0f)
                curSpeed = MathClamp(curSpeed - acceleration - friction, 0f, maxSpeed);
            else if (curSpeed < 0f)
                curSpeed = MathClamp(curSpeed + acceleration + friction, -maxSpeed, 0f);
            need_add_friction = false;
        }

        if (need_add_friction && curSpeed != 0f)
        {
            if (curSpeed > 0f)
                curSpeed = MathClamp(curSpeed - friction, 0, maxSpeed);
            else if (curSpeed < 0f)
                curSpeed = MathClamp(curSpeed + friction, -maxSpeed, 0);
        }
    }

    private void UpdateAngle()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            curRotation = MathClamp(curRotation + stepRotation, 0, maxRotation);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            curRotation = MathClamp(curRotation - stepRotation, -maxRotation, 0);
        }
        else
        {
            if (curRotation > 0f)
                curRotation = MathClamp(curRotation - frictionRotation, 0, maxRotation);
            else if (curRotation < 0f)
                curRotation = MathClamp(curRotation + frictionRotation, -maxRotation, 0);
        }
    }

    private float MathClamp(float value, float min, float max)
    {
        if (value < min)
            return min;
        else if (value > max)
            return max;
        else
            return value;
    }
}
