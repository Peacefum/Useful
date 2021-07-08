using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleInk : MonoBehaviour
{
    //파티클개체
    public ParticleSystem particleLauncher;
    //벽에충돌 이펙트
    public ParticleSystem splatterParticles;

    public ParticleDecalPool particleDecalPool;

    //충돌이벤트 저장 리스트
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public Gradient particleColorGradient;

    private void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }


    private void OnParticleCollision(GameObject other)
    {
        //파티클 이벤트 감지 , 리스트에 충돌시 재할당
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);

        //각각 파티클을 해당위치에 이미트 시킨ㅁ
        for(int i = 0; i< collisionEvents.Count; i++)
        {
            GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.1f);
             GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip); // 06 29 페인트 퍼지는 소리
            EmitAtLocation(collisionEvents[i]);

            particleDecalPool.ParticleHit(collisionEvents[i], particleColorGradient);
        }
    }

    private void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        //이벤트 교차점 정보
        splatterParticles.transform.position = particleCollisionEvent.intersection;
        //이벤트의 노말 가져오기
        splatterParticles.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);

        //색사ㅏㅇ은 직접 변경 불가능 , 간접접근으로 변경
        ParticleSystem.MainModule psMain = splatterParticles.main;
        psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));

        //하나의 이벤트에 하나의 파티클 
        splatterParticles.Emit(7);

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.touchCount >0)
        {
            ParticleSystem.MainModule psMain = particleLauncher.main;
            psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));
            particleLauncher.Emit(1);
        }
    }



}
