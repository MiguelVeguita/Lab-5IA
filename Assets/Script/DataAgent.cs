using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
 
[System.Serializable]
public class Data
{
    [Range(0f, 1f)]  
    public float value;
    public float valueMax=1;
    public float time;
    public float timeRate;
    public float timeFrameRate =0;
    public Data() { 
    
    
    }
}
public class DataAgent : MonoBehaviour
{
    public Data Energy = new Data();
    public Data Sleep = new Data();
    public Data WC = new Data();
    Coroutine CoroutineEnergy=null;

    public bool CantLoadEnergy { get => CoroutineEnergy == null; }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator LoadEnergyTime(float time)
    {

        while(time>0)
        {
            time--;
            Energy.value = Mathf.Lerp(Energy.value, Energy.valueMax, Time.deltaTime * 20f);
            yield return new WaitForSecondsRealtime(1);
        }
        Energy.value = Energy.valueMax;
        StopCoroutine(CoroutineEnergy);
        CoroutineEnergy = null;

    }
    public void LoadEnergy()
    {
        if (CoroutineEnergy == null)
            CoroutineEnergy = StartCoroutine(LoadEnergyTime(Energy.time));

    }

    public void DiscountEnergy()
    {
        if(Energy.timeFrameRate > Energy.timeRate)
        {
            Energy.timeFrameRate = 0;
            Energy.value-=0.03f;
        }
        Energy.timeFrameRate += Time.deltaTime;
    }



    public void StopLoadingEnergy()
    {
        if (CoroutineEnergy != null)
        {
            StopCoroutine(CoroutineEnergy);
            CoroutineEnergy = null;
        }
    }

    public void IncreaseSleep()
    {
        if (Sleep.timeFrameRate > Sleep.timeRate)
        {
            Sleep.timeFrameRate = 0;
            Sleep.value += 0.03f;
            Sleep.value = Mathf.Clamp(Sleep.value, 0, Sleep.valueMax);
        }
        Sleep.timeFrameRate += Time.deltaTime;
    }

    public void DecreaseSleep()
    {
        if (Sleep.timeFrameRate > Sleep.timeRate)
        {
            Sleep.timeFrameRate = 0;
            Sleep.value -= 0.03f;
            Sleep.value = Mathf.Clamp(Sleep.value, 0, Sleep.valueMax);
        }
        Sleep.timeFrameRate += Time.deltaTime;
    }
    public void IncreaseWC()
    {
        if (WC.timeFrameRate > WC.timeRate)
        {
            WC.timeFrameRate = 0;
            WC.value += 0.04f; // Ajusta esta velocidad según necesites
            WC.value = Mathf.Clamp(WC.value, 0, WC.valueMax);
        }
        WC.timeFrameRate += Time.deltaTime;
    }
    public void ReduceWC()
    {
        if (WC.timeFrameRate > WC.timeRate)
        {
            WC.timeFrameRate = 0;
            WC.value -= 0.1f; // Ajusta la velocidad de reducción
            WC.value = Mathf.Clamp(WC.value, 0, WC.valueMax);
        }
        WC.timeFrameRate += Time.deltaTime;
    }
    public void EmptyWC()
    {
        WC.value = 0;
    }


}
