using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera[] _cameras;
    private int _currentCameraNumber = 0;

    void LateUpdate() 
    {   
        for (int i = 0; i < _cameras.Length; i++) {
            if (Input.GetKey(KeyCode.P)) {
                Debug.Log("checking " + i.ToString());

            }
            if (Input.GetKeyUp((i + 1).ToString())) {
                Debug.Log("Switch camera to " + i.ToString());
                SwitchCamera(i);
                return;
            }
        }
    }

    private void SwitchCamera(int number) {
        if (_currentCameraNumber == number)
            return;

        _cameras[_currentCameraNumber].enabled = false;
        _cameras[number].enabled = true;
        _currentCameraNumber = number;
    }
}
