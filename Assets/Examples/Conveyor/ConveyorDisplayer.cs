using UnityEngine;
using UnityEngine.UI;

public class ConveyorDisplayer : MonoBehaviour
{
    public ConveyorController controller;
    public InputField speedField;

    public void ChangeSpeed(string speed)
    {
        if(float.TryParse(speedField.text, out float result))
        {
            controller.SetTargetSpeed(result);
        }
    }
}
