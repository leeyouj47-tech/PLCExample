using UnityEngine;

public class GP50OfflineExample : OfflineProgram
{
    private const float PRECISION_JOINT_SPEED = 20f;        //조인트 정밀 회전 속도  [rad/s]
    private const float FAST_JOINT_SPEED = 60f;                 //조인트 고속 회전 속도[rad/s]
    private const float PRESICSION_TCP_SPEED = 100f;        //TCP 정밀 이동 속도 [mm/s]
    private const float PRESICSION_TCP_ANGULAR = 30f;       //TCP 정밀 회전 속도[rad/s]
    private const float FAST_TCP_SPEED = 300f;                      //TCP 고속 이동 속도[mm/s]
    private const float FAST_TCP_ANGULAR = 300f;                      //TCP 고속 회전 속도[rad/s]
    public override void Home()
    {
        //가감속 시간 설정
        this.AccTime(0.2f, 0.2f);

        //원점 복귀 명령 - 
        this.MoveJ(waypoints[0], PRESICSION_TCP_ANGULAR);
    }

    public void Program1()
    {
        //지령 속도 지정
        this.Speed(FAST_TCP_SPEED, FAST_TCP_ANGULAR, FAST_JOINT_SPEED);

        //1번 툴 위치로 이동
        this.MoveJ(waypoints[1]);
        
        //1번 툴 부착
        this.Connect(true);

        //0.5초 대기
        this.Wait(500);

        //4번 위치로 이동
        this.MoveJ(waypoints[1]);

        //5번 위치로 이동
        this.MoveJ(waypoints[1]);

        //1번 툴 해제
        this.Connect(false);

        //원점으로 이동
        this.MoveJ(waypoints[2]);
    }
}
