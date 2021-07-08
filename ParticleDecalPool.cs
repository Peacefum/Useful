using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalData
{
    //데칼이 붙을 파티클 위치 , 크기 , 회전방향,색상
    public Vector3 position;
    public float size;
    public Vector3 rotation;
    public Color color;
}

public class ParticleDecalPool : MonoBehaviour
{
    //최대갯수를 정하고 이펙트 생성 
    public int maxDecals = 100;
    public float decalSizeMin = .5f;
    public float decalSizeMax = 1.5f;

    private ParticleSystem decalParticleSystem;
    private int particleDecalDataIndex;

    private ParticleDecalData[] particleData;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        decalParticleSystem = GetComponent<ParticleSystem>();
        //최대 데칼 배열 생성
        particles = new ParticleSystem.Particle[maxDecals];
        particleData = new ParticleDecalData[maxDecals];

        for (int i=0; i< maxDecals; i++)
        {
            particleData[i] = new ParticleDecalData();
        }

    }
    //파티클 충돌때마다 호출할 함수
    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        SetParticleData(particleCollisionEvent, colorGradient);
        DisplayParticles();
    }

    private void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient colorGradient)
    {
        //최대치를 넘으면
        if(particleDecalDataIndex >= maxDecals)
        {
            particleDecalDataIndex = 0;
        }

        //이펙트
        particleData[particleDecalDataIndex].position = particleCollisionEvent.intersection;
        //데칼 이펙트가 메쉬(쿼드)로 되어있어서, 어느방향을 볼지 결정 // 근데 안보였음 파티클 설정에서 설정 바꿈

        //방향이 안맞아서 안보임
        //파티클 충돌이벤트 노말정보

        Vector3 particleRotationEuler = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;
        particleRotationEuler.z = Random.Range(0, 360);
        particleData[particleDecalDataIndex].rotation = particleRotationEuler;
        particleData[particleDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);
        particleData[particleDecalDataIndex].color = colorGradient.Evaluate(Random.Range(0f, 1f));
        particleDecalDataIndex++;
    }

    private void DisplayParticles()
    {
        for(int i=0; i< particleData.Length; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].rotation3D = particleData[i].rotation;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = particleData[i].color;
        }

        decalParticleSystem.SetParticles(particles, particles.Length);
    }

}
