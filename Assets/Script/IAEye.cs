using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEye : MonoBehaviour
{
    public DataView dataView = new DataView();
    public Health ViewToy = new Health();
    float[] arrayRate = new float[100];
    float FrameRate = 0;
    int index = 0;
    public float MinRate;
    public float MaxRate;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < arrayRate.Length; i++)
        {
            arrayRate[i] = Random.Range(MinRate, MaxRate);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FrameRate > arrayRate[index])
        {
            FrameRate = 0;
            Scan();
            index++;
            index = index % arrayRate.Length;
        }

        FrameRate += Time.deltaTime;
    }

    void Scan()
    {
        // Se reinicia la referencia al objeto detectado
        ViewToy = null;

        Collider[] toys = Physics.OverlapSphere(transform.position, dataView.distance, dataView.Scanlayers);
        float min_dist = float.MaxValue;

        for (int i = 0; i < toys.Length; i++)
        {
            GameObject toy = toys[i].gameObject;
            UnityEngine.Debug.Log("name: " + toy.name);

            Health health = toy.GetComponent<Health>();
            if (health != null && health.AimOffSet != null && dataView.IsInSight(health.AimOffSet))
            {
                float dist = (transform.position - health.transform.position).magnitude;
                if (min_dist > dist)
                {
                    ViewToy = health;
                    min_dist = dist;
                }
            }
        }

        if (ViewToy == null)
            dataView.InSight = false;
    }

    private void OnValidate()
    {
        dataView.CreateMesh();
    }

    private void OnDrawGizmos()
    {
        dataView.OnDrawGizmos();
    }
}
