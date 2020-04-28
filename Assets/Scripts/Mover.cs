using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _maxSpeed = 0.3f; // абсолютное значение максимальной скорости (при движкнии назад -_maxSpeed)
    [SerializeField] private float _acceleration = 0.05f; // ускорение
    [SerializeField] private float _friction = 0.01f; // трение. на каждом апдейте это значение отнимается от скорости (или прибавляется, если скорость отрицательна)
    // все то же, только для угла поворота
    [SerializeField] private float _stepRotation = 0.2f;
    [SerializeField] private float _frictionRotation = 0.1f;
    [SerializeField] private float _maxRotation = 3f;


    public float curSpeed { get; private set; } // текущая скорость
    public float curRotation { get; private set; }  // текущий угол поворота
    
    private int collisionsCount = 0; // колличество коллизий в данный момент времени
    
    // если я правильно понимаю, этот метод нужно использовать вместо конструктора для классов-наследников MonoBehaviour
    private void Awake() {
        curSpeed = 0f;
        curRotation = 0f;
    }

    private void FixedUpdate()
    {   
        // пока присутствует коллизия - игнорируем нажатия клавиш
        if (collisionsCount <= 0) {
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

    private void OnCollisionEnter(Collision collision)
    {
        // этот блок меняет направление движения в момент столкновения, чтоб машинка не заехала за препятствие
        // меняем только при первом столкновении т.к. может быть 2 одновременно и тогда направление скорости не 
        // поменяется и машинка "проскользнет"
        if (collisionsCount == 0) {
            curSpeed = -curSpeed;
            curRotation = -curRotation;
        }
        collisionsCount++;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionsCount--;
    }

    private void UpdateSpeed()
    {
        bool need_add_friction = true;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            curSpeed = Mathf.Clamp(curSpeed + _acceleration - _friction, 0, _maxSpeed);
            need_add_friction = false;

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("Can not move back with button W pressed");
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            curSpeed = Mathf.Clamp(curSpeed - _acceleration + _friction, -_maxSpeed, 0);
            need_add_friction = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (curSpeed > 0f)
                curSpeed = Mathf.Clamp(curSpeed - _acceleration - _friction, 0f, _maxSpeed);
            else if (curSpeed < 0f)
                curSpeed = Mathf.Clamp(curSpeed + _acceleration + _friction, -_maxSpeed, 0f);
            need_add_friction = false;
        }

        if (need_add_friction && curSpeed != 0f)
        {
            if (curSpeed > 0f)
                curSpeed = Mathf.Clamp(curSpeed - _friction, 0, _maxSpeed);
            else if (curSpeed < 0f)
                curSpeed = Mathf.Clamp(curSpeed + _friction, -_maxSpeed, 0);
        }
    }

    private void UpdateAngle()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            curRotation = Mathf.Clamp(curRotation + _stepRotation, 0, _maxRotation);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            curRotation = Mathf.Clamp(curRotation - _stepRotation, -_maxRotation, 0);
        }
        else
        {
            if (curRotation > 0f)
                curRotation = Mathf.Clamp(curRotation - _frictionRotation, 0, _maxRotation);
            else if (curRotation < 0f)
                curRotation = Mathf.Clamp(curRotation + _frictionRotation, -_maxRotation, 0);
        }
    }
}
