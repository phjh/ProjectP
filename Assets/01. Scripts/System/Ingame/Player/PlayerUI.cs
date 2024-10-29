using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Slider easeHpSlider;

    [SerializeField]
    private float hpLerpTime = 0.5f;

    [SerializeField]
    private Gradient damageEasingGradiant;
    [SerializeField]
    private Gradient healEasingGradiant;

    private Coroutine baseHpSettingCoroutine;

    private float lastValue = 100;

    public void SetHpBar(float maxhp, float nowhp)
    {
        if (baseHpSettingCoroutine != null)
        {
            StopCoroutine(baseHpSettingCoroutine);
        }
        baseHpSettingCoroutine = StartCoroutine(SetHpBarCoroutine(nowhp / maxhp));
    }

    //체력바 세팅해주는 코루틴
    private IEnumerator SetHpBarCoroutine(float percent)
    {
        //최근 변경된 체력을 가져와준다
        float baseValue = lastValue;
        float time = 0;

        if (baseValue >= percent)
        {
            Image image = easeHpSlider.fillRect.GetComponent<Image>();
            image.color = damageEasingGradiant.Evaluate(0);
            hpSlider.value = percent;

            while(time <= hpLerpTime)
            {
                lastValue = Mathf.Lerp(baseValue, percent, time / hpLerpTime);
                image.color = damageEasingGradiant.Evaluate(time / hpLerpTime);
                easeHpSlider.value = lastValue;
                time += Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            easeHpSlider.value = percent;
        }
        else
        {
            Image image = easeHpSlider.fillRect.GetComponent<Image>();
            image.color = healEasingGradiant.Evaluate(0);
            easeHpSlider.value = percent;
            
            while (time <= hpLerpTime)
            {
                lastValue = Mathf.Lerp(baseValue, percent, time / hpLerpTime);
                image.color = healEasingGradiant.Evaluate(time / hpLerpTime);
                hpSlider.value = lastValue;
                time += Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            hpSlider.value = percent;
        }

    }

}
