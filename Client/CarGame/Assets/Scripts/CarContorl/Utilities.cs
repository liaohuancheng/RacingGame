using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utilities 
{
    public static float MilePerHour2MeterPerSecond(float v)
    {
        return (float)(v * 0.44704);
    }

    public static bool SameSign(float first,float second)
    {
        if (Mathf.Sign(first) == Mathf.Sign(second))
            return true;
        else
            return false;
    }

    public static float EvaluateNormPower(float normPower)
    {
        if (normPower < 1)
            return 10 - normPower * 9;
        else
            return 1.9f - normPower * 0.9f;
    }
}
