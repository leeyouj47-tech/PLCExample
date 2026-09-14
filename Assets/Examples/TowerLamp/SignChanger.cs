using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class SignChanger : MonoBehaviour
{
    //신호등 색
    public SignController red;
    public SignController yellow;
    public SignController green;

    public MeshRenderer meshRenderer;
    public UnityEvent<bool> onChangedValue;
    private bool isOn = false;
    //재생 버튼을 누른 이후부터 몇초가 흘렀는지 알 수 있는 프로퍼티
    //Time.time;
    //한 프레임당 얼마나 시간이 지났는지 알 수 있는 프로퍼티
    //Time.deltaTime;

    public float current = 0f;

    public bool IsOn
    {
        get => isOn;
        set
        {
            if (isOn == value)
                return;

            isOn = value;
            TurnOn(value);
            onChangedValue?.Invoke(value);
        }
    }

    private void TurnOn(bool isOn)
    {
        if (meshRenderer == null)
            return;
        if (isOn)
            meshRenderer.material.EnableKeyword("_EMISSION");
        else
            meshRenderer.material.DisableKeyword("_EMISSION");

    }

    void Update()
    {
        /*
         1. 처음 10초 동안 녹색 램프가 켜져 있음
        2. 그 후에 10초 동안 노란 램프가 1초 간격으로 켜졌다 꺼졌다 반복함
        3. 그 후에 10초 동안 빨간 랜프가 켜짐.
        4. 이후 반복
         */
        //켜기
        /*
        red.IsOn = true;
        yellow.IsOn = true;
        green.IsOn = true;*/

        //끄기
        /*red.IsOn = false;
        yellow.IsOn = false;
        green.IsOn = false;*/

        //time에 30초로 구간 나누기
        /*        float time = Time.time % 30;

                //기본 세팅 false(꺼진상태)
                red.IsOn = false;
                yellow.IsOn = false;
                green.IsOn = false;

                //만약, 10초 전이면 초록불만 켜지도록
                if(time < 10f)
                {
                    green.IsOn = true;
                }
                //20초 미만인경우 
                else if(time < 20f)
                {
                    //2초에 한번씩 쳐지고 꺼지기
                    if((int)time %2 == 0)
                    {
                        yellow.IsOn = true;
                    }
                }
                //10초, 20초 벗어나면 빨간불만 켜지기
                else
                {
                    red.IsOn = true;
                }*/

        current += Time.deltaTime;

        red.IsOn = false;
        yellow.IsOn = false;
        green.IsOn = false;

        if (current >= 30f)
            current = 0f;

        if (current < 10f)
        {
            green.IsOn = true;
        }
        else if(current < 20f)
        {
            if((int)current % 2 == 0)
            {
                yellow.IsOn = true;
            }
        }
        else
        {
            red.IsOn = true;
        }
    }
}
