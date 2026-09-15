using UnityEngine;
using UnityEngine.UI;

public class InverterDisplayer : MonoBehaviour
{
    public InverterController controller;
    public Text hzText;
    public void OnClickSTF()
    {
        controller.IsOnSTF = !controller.IsOnSTF;
    }
    public void OnClickSTR()
    {
        controller.IsOnSTR = !controller.IsOnSTR;
    }

    public void OnClickIncrease()
    {
        controller.ChangeFrequency(controller.targetHz + 10f);
    }

    public void OnClickDecrease()
    {
        controller.ChangeFrequency(controller.targetHz - 10f);
    }
    private void Start()
    {
        controller.onChangedHz.AddListener(DisplayCurrentHz);
    }

    public void DisplayCurrentHz(float hz)
    {
        //D: 자연수, F: 는 실수 / F1은 소수점 첫째자리까지 쓰겠다
        hzText.text = $"{hz:f1}Hz";
    }
}
