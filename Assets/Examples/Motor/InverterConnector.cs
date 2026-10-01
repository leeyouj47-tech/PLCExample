using UnityEngine;
using UnityEngine.Events;

public class InverterConnector : MXObject
{
    public Rigidbody shaft;
    public ConfigurableJoint joint;

    [Header("필수 출력 디바이스")]
    public DeviceAddress STF_Address = new DeviceAddress("정회전 신호");
    public DeviceAddress STR_Address = new DeviceAddress("역회전 신호");
    public DeviceAddress MRS_Address = new DeviceAddress("비상정지 신호");
    public DeviceAddress RST_Address = new DeviceAddress("리셋 신호");

    [Header("필수 입력 디바이스")]
    public DeviceAddress RUN_Address = new DeviceAddress("운행중 피드백 신호");
    public DeviceAddress SU_Address = new DeviceAddress("목표Hz 도달 신호");
    public DeviceAddress ALERT_Address = new DeviceAddress("고장 신호");

    [Header("모터 기본 성능")]
    [Delayed] public int poleCount = 4;     //자석 극수가 몇개 있는지
    [Delayed] public float maxFrequency = 60f;      //최대 주파수
    [Delayed] public float maxRPM = 1800f;          //정격 회전수
    [Delayed] public float accelTime = 1.0f;        //가속 시간(0 -> Max까지 도달하는데 걸리는 시간)
    [Delayed] public float decelTime = 1.0f;        //감속 시간(Max -> 0까지 도달하는데 걸리는 시간)

    [Header("다단 속도 설정")]
    public bool useStep = false;        //다단 속도 제어. true로 변경하면 인버터 직접 제어가 우선순위에서 밀림
    public DeviceAddress RL_Adrress = new DeviceAddress("저단 속도 명령 신호");
    public DeviceAddress RM_Adrress = new DeviceAddress("중단 속도 명령 신호");
    public DeviceAddress RH_Adrress = new DeviceAddress("고단 속도 명령 신호");
    public float[] stepFrequencies = new float[8]
    {
        0f, 10f, 30f, 0f, 60f, 0f, 0f, 0f
    };

    [Header("아날로그 입력 설정")]
    public bool useAnalogInput = false;     //아날로그 신호로 제어하는 기능 On/Off
    public int analogMaxResolution = 4000;      //분해능(미쯔비시: 4000, LS: 16000)

    [Header("모니터링")]
    [Delayed, Range(0, 240f), SerializeField]
    private float targetHz = 0f;        //지령 주파수 -> PLC로부터 받은 명령에 의해 정해진 지령 주파수 확인용
    private float analogInputValue = 0f;        //아날로그 지령값

    [SerializeField] private float currentHz = 0f;      //현재 주파수
    [SerializeField] private float currentRPM = 0f;      //현재 RPM

    //PLC에 보내는 피드백 데이터
    private bool isRun = false;
    private bool emergencyStop = false;
    private bool isAlert = false;
    private bool isOnForward= false;
    private bool isOnReverse = false;
    private bool isOnLow = false;
    private bool isOnMiddle = false;
    private bool isOnHigh = false;
    private bool reachTargetHz = false;

    public UnityEvent<bool> onChangedRun;       //운전 상태 변화
    public UnityEvent<bool> onChangedEMS;       //긴급정지 상태 변화
    public UnityEvent<bool> onChangedAlert;       //고장 상태 변화
    public UnityEvent<bool> onChangedForward;       //정회전 상태 변화
    public UnityEvent<bool> onChangedReverse;       //역회전 상태 변화
    public UnityEvent<bool> onChangedRL;       //저단 상태 변화
    public UnityEvent<bool> onChangedRM;       //중단 상태 변화
    public UnityEvent<bool> onChangedRH;       //고단 상태 변화
    public UnityEvent<bool> onReachTargetHz;       //목표 주파수에 도달 여부
    public UnityEvent<bool> onChangedAnalog;       //아날로그 신호 변화
    public UnityEvent<bool> onChangedTargetHz;       //현재 목표 주파수 변화
    public UnityEvent<bool> onChangedCurrentHz;       //현재 주파수 변화
    public UnityEvent<bool> onChangedCurrentRPM;       //현재 RPM 변화

    public float GetCurrentHz => currentHz;     //현재 주파수 가져오기
    public float GetCurrentRPM => currentRPM;   //현재 RPM값 가져오기
    public bool IsRun
    {
        get => isRun;
        private set
        {
            if(isRun == value)
                return;

            isRun = value;
            onChangedRun?.Invoke(value);

            if(!RUN_Address.useDevice)
                return;

            if (string.IsNullOrEmpty(RUN_Address.address))
            {
                Debug.LogWarning("입력 디바이스 주소가 비어있어 신호르 보낼 수 없습니다. 주소를 채워주세요.");
                return;
            }

            //PLC에 변경된 상태 신호를 보내기
            MXRequester.Get.AddSetDeviceRequest(RUN_Address.address, (short)(value ? 1 : 0));
        }
    }

    public bool EStop
    {
        get => emergencyStop;

        private set
        {
            if (emergencyStop == value)
                return;

            emergencyStop = value;
            onChangedEMS?.Invoke(value);
        }
    }

    public bool IsAlert
    {
        get => isAlert;
        private set
        {
            if(isAlert == value)
                return;

            isAlert = value;
            onChangedAlert?.Invoke(value);

            if(!ALERT_Address.useDevice)
                return;

            if (string.IsNullOrEmpty(ALERT_Address.address))
            {
                Debug.LogWarning("입력 디바이스 주소가 비어있어 신호를 보낼 수 없습니다. 주소를 채워주세요.");
                return;
            }
            MXRequester.Get.AddSetDeviceRequest(ALERT_Address.address, (short)(value ? 1 : 0));
        }
    } 
    
    public bool ReachTargetHz
    {
        get => reachTargetHz;
        private set
        {
            if(reachTargetHz == value)
                return;

            reachTargetHz = value;
            onReachTargetHz?.Invoke(value);

            if(!SU_Address.useDevice)
                return;

            if (string.IsNullOrEmpty(SU_Address.address))
            {
                Debug.LogWarning("입력 디바이스 주소가 비어있어 신호를 보낼 수 없습니다. 주소를 채워주세요.");
                return;
            }
            MXRequester.Get.AddSetDeviceRequest(SU_Address.address, (short)(value ? 1 : 0));
        }
    }

    public bool STF
    {
        get => isOnForward;
        set
        {
            if(isOnForward == value) 
                return;

            if(isOnForward = value)
            {
                onChangedReverse?.Invoke(isOnReverse = false);
            }
            onChangedForward?.Invoke(value);
            //목표 주파수를 재설정
        }
    }

    public bool STR
    {
        get => isOnReverse;
        set
        {
            if (isOnReverse == value)
                return;

            if (isOnReverse = value)
            {
                onChangedForward?.Invoke(isOnForward = false);
            }
            onChangedReverse?.Invoke(value);
            //목표 주파수를 재설정
        }
    }
}
