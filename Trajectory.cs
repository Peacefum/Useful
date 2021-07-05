using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    // 팅기는거 까지 보이지는 않는 탄도학

    public int _lineSegmentCount;
    public int _linePointCount;
    public List<Vector3> _linepoints = new List<Vector3>();

    public LineRenderer _lineRenderer;

    public Vector3 premousePosition;
    public float power;
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }


    public void UpdateTranjectory(Vector3 forceVector, Rigidbody rigidbody, Vector3 startingPoint)
    {
        Vector3 velocity = (forceVector / rigidbody.mass) * Time.fixedDeltaTime; // 계산방법인데 뭘까
        float FlightDuration = (2 * velocity.y) / Physics.gravity.y; // 나는시간 위로 가는 벨로시티에 /중력 

        float stepTime = FlightDuration / _lineSegmentCount; // 나느 시간에 / 라인카운트수 

        _linepoints.Clear();

        for( int i=0; i< _linePointCount; i++)
        {
            float stepTimePassed = stepTime * i; //

            //날아가면서 위치 
            Vector3 MovementVector = new Vector3(
                x: velocity.x * stepTimePassed,
                y: velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                z: velocity.z * stepTimePassed);

            RaycastHit hit;
            if(Physics.Raycast(origin: startingPoint, direction: -MovementVector, out hit, MovementVector.magnitude))
            {
                break;
            }
            _linepoints.Add(item: -MovementVector + startingPoint);
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

        Vector3 forceV = (new Vector3(0, Force.y < 0f ? 0f : Force.y, z: Force.y)) * power; // 날아가는힘

        if(forceV.y > 0) //슛 제한
        {
            GetComponent<Rigidbody>().AddForce(forceV);
            GetComponent<Rigidbody>().useGravity = true;
        }
    }



    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            premousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(0))
        {
            HideLine();

            Vector3 forceInit = (premousePosition - Input.mousePosition); // 당기는힘

            Vector3 forceV = (new Vector3(0, forceInit.y < 0f ? 0f : forceInit.y, z: forceInit.y)) * power;

            UpdateTranjectory(forceV, GetComponent<Rigidbody>(), transform.position);

            
        }

        if(Input.GetMouseButtonUp(0))
        {
            HideLine();
            Vector3 mouseReleasePos = Input.mousePosition;
            Shoot( premousePosition - mouseReleasePos);

        }
    }
}





//private void OnMouseDown()
//{
//    premousePosition = Input.mousePosition;
//}

//private void OnMouseDrag()
//{


//    HideLine();

//    Vector3 forceInit = (premousePosition - Input.mousePosition ); // 당기는힘

//    Vector3 forceV = (new Vector3(0, forceInit.y < 0f ? 0f : forceInit.y, z: forceInit.y)) * 2f;

//    UpdateTranjectory(forceV,GetComponent<Rigidbody>(),transform.position);
//}
//private void OnMouseUp()
//{
//    HideLine();
//}
