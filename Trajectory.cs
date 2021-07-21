using Systemusing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    // 팅기는거 까지 보이지는 않는 탄도학

    public int _lineSegmentCount;
    public int _linePointCount;
    public List<Vector3> _linepoints = new List<Vector3>();

    LineRenderer _lineRenderer;

    public Vector3 premousePosition;
    public float power;

    Rigidbody rigidbody;


    RuntimePlatform currentPlatform;
    public Transform shotPos;
    private void Start()
    {
        rigidbody = gameObject.AddComponent<Rigidbody>();
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        //   _lineRenderer = GetComponent<LineRenderer>();
        rigidbody.useGravity = false;

        // _lineRenderer.useWorldSpace = false;

    }


    public void UpdateTranjectory(Vector3 forceVector, Rigidbody rigidbody, Vector3 startingPoint)
    {

        Vector3 velocity = (forceVector / rigidbody.mass) * Time.fixedDeltaTime; // 계산방법인데 뭘까
        float FlightDuration = (2 * velocity.y) / Physics.gravity.y; // 나는시간 위로 가는 벨로시티에 /중력 

        //방향만 바꿔주면 끝인데 // 절대좌표로만 가고있다 내위치 기준으로 앞이 아님 tranformz 설정도 해주면 꺽이는건 보이지 않는다
        float stepTime = FlightDuration / _lineSegmentCount; // 나느 시간에 / 라인카운트수 

        _linepoints.Clear();
        for (int i = 0; i < _linePointCount; i++)
        {
            float stepTimePassed = stepTime * i; //
            // 절대좌표로 계산이 되고있다 // 월드좌표 해제후 가능
            //날아가면서 위치 
            Vector3 MovementVector = new Vector3(
                x: velocity.x * stepTimePassed,
                y: velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                z: velocity.z * stepTimePassed);


            // Debug.Log(MovementVector);
            RaycastHit hit;
            if (Physics.Raycast(origin: startingPoint, direction: -MovementVector, out hit, MovementVector.magnitude)) // 라인에 부딪혔을때 라인 짤림
            {
                continue;
                //break;
            }
            _linepoints.Add(item: -MovementVector); // + startingPoint); // 스타팅 포인트 위치 기준으로시작 //  + startingPoint이거를 빼고 월드스페이스를 뺴니까 라인렌더러가 나름 정상으로 작동
        }

        _lineRenderer.positionCount = _linepoints.Count;
        _lineRenderer.SetPositions(_linepoints.ToArray()); // 라인렌더러에 집어넣어준다

    }

    public void HideLine()
    {
        _lineRenderer.positionCount = 0; // 라인렌더러 초기화
    }

    public void Shoot(Vector3 Force)
    {
        Vector3 forceV = (new Vector3(0, Force.y < 0f ? 0f : Force.y, z: Force.y)); // 날아가는힘 // 제자리에서만 날아감

        if (forceV.y > 0) //슛 제한
        {
            rigidbody.AddRelativeForce(forceV); // addforce에서 addrealativeforce 로 변경 내자리에서 발사
            rigidbody.useGravity = true;
        }
    }


    private void Update()
    {
        if (currentPlatform != RuntimePlatform.Android)
        {
            MouseTrans();
        }
        else
        {
            TouchTrans();
        }
    }

    public void TouchTrans()
    {
        Touch touch;

        touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            premousePosition = Input.GetTouch(0).position;
            transform.position = shotPos.position;
            rigidbody.Sleep();
            rigidbody.velocity = Vector3.zero;
            transform.rotation = shotPos.rotation;
            rigidbody.useGravity = false;
            rigidbody.WakeUp();
        }
        if (touch.phase == TouchPhase.Moved)
        {
            Vector3 forceInit = ((Vector2)premousePosition - Input.GetTouch(0).position); // 당기는힘
            Vector3 forceV = (new Vector3(forceInit.x - forceInit.x, forceInit.y < 0f ? 0f : forceInit.y, z: forceInit.y)) * power; // x가 0이니까좌우로 안가짐
            UpdateTranjectory(forceV, rigidbody, shotPos.position);
        }
        if (touch.phase == TouchPhase.Ended)
        {
            HideLine();
            Vector3 mouseReleasePos = Input.GetTouch(0).position;
            Shoot(premousePosition - mouseReleasePos);
        }
    }

    public void MouseTrans()
    {
        if (Input.GetMouseButtonDown(0))
        {
            premousePosition = Input.mousePosition;
            transform.localPosition = shotPos.position;
            rigidbody.Sleep();
            rigidbody.velocity = Vector3.zero;
            transform.localRotation = shotPos.rotation;
            rigidbody.useGravity = false;
            rigidbody.WakeUp();
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 forceInit = (premousePosition - Input.mousePosition); // 당기는힘
            Vector3 forceV = (new Vector3(forceInit.x - forceInit.x, forceInit.y < 0f ? 0f : forceInit.y, z: forceInit.y)) * power;

            //forceV 가 내위치에서 가야함
            UpdateTranjectory(forceV, rigidbody, shotPos.position);
        }

        if (Input.GetMouseButtonUp(0))
        {
            HideLine();
            Vector3 mouseReleasePos = Input.mousePosition;
            Vector3 forceInit = (premousePosition - mouseReleasePos); // 당기는힘


            Shoot(forceInit); // shoot에 힘을 전달
        }
    }
}