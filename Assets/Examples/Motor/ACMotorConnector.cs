using UnityEngine;
using UnityEngine.Events;

public class ACMotorConnector : MXObject
{
    public ACMotorController controller;
    public DeviceAddress forwardAddress = new DeviceAddress("모터 정회전 신호");
    public DeviceAddress backwardAddress = new DeviceAddress("모터 역회전 신호");

    public UnityEvent<bool> onChangedForward;
    public UnityEvent<bool> onChangedBackward;

    void Start()
    {
        if (forwardAddress.useDevice)
            MXRequester.Get.AddDeviceAddress(forwardAddress.address, OnChangedForward);

        if (backwardAddress.useDevice)
            MXRequester.Get.AddDeviceAddress(backwardAddress.address, OnChangedBackward);

        if (controller == null)
            controller = GetComponent<ACMotorController>();
    }

    private void OnChangedBackward(short obj)
    {
        controller.IsOnBackward = obj != 0;
        onChangedBackward?.Invoke(obj != 0);
    }

    private void OnChangedForward(short obj)
    {
        controller.IsOnForward = obj != 0;
        onChangedForward?.Invoke(obj != 0);
    }
}
