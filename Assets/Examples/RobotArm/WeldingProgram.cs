using UnityEditor.Purchasing;
using UnityEngine;

public class WeldingProgram : OfflineProgram
{
    //원점 복귀 함수
    public override void Home()
    {
        this.AccTime(0.2f, 0.2f);
        this.Speed(200f, 60f, 30f);
        this.MoveL(waypoints[0]);
    }
    public void Program1()
    {
        //1. 속도설정
        this.Speed(200f, 60f, 30f);

        //2. 용접툴 위치로 이동(1번)
        this.MoveL(waypoints[1]);

        //3. 용접툴 부착
        this.Connect(true);

        //4. 1초 대기
        this.Wait(1000);

        //5. 작업대 상단 위치로 이동( > 3 > 4 > 5)
        this.MoveL(waypoints[3]);
        this.MoveL(waypoints[4]);
        this.MoveL(waypoints[5]);

        //5-1. 융접툴 켜기
        this.Action(true, 100);

        //6. 실린더 4개의 꼭지점을 시계방향으로 한번씩 찍으면서 이동(우 > 하 > 좌 > 상)
        this.MoveL(waypoints[6]);
        this.MoveL(waypoints[8]);
        this.MoveL(waypoints[7]);
        this.MoveL(waypoints[5]);
        this.Wait(500);

        this.MoveL(waypoints[6], 200f, 60f, Pattern.Continue);
        this.MoveL(waypoints[8], 200f, 60f, Pattern.Continue);
        this.MoveL(waypoints[7], 200f, 60f, Pattern.Continue);
        this.MoveL(waypoints[5]);

        this.MoveC(waypoints[6], waypoints[8], 200f, 60f, Pattern.Continue);
        this.MoveC(waypoints[7], waypoints[5]);
        //6-1. 융접툴 끄기
        this.Action(false);

        //7. 작업대 상단 위치로 이동(> 4 > 3)
        this.MoveL(waypoints[4]);
        this.MoveL(waypoints[3]);

        //8. 용접툴 거치대 위치로 이동(2번)
        this.MoveL(waypoints[2]);

        //9. 용접툴 연결 해제
        this.Connect(false);

        //10. 원점으로 복귀
        this.MoveL(waypoints[0]);
    }

    public void Program2()
    {
        //1. 속도 설정

        //2. 용접률 위치로 이동 > 부착

        //3. 3초 대기

        //4. 작업대 상단위치(3 > 4 > 5)

        //5. 시계 반대방향(상 > 좌 > 하 > 우)
    }
}
