using System;
using UnityEngine.Events;

public static class Helper
{
  public static float Epsilon = 0.001f;

  public static float ClampAngle(float angle)
  {
    if (angle > 180)
    {
      angle -= 360;
    }
    else if (angle < -180)
    {
      angle += 360;
    }
    return angle;
  }
}

namespace UnityEngine.Events
{
  [System.Serializable] public class UnityEventBool : UnityEvent<bool> { }
  [System.Serializable] public class UnityEventChar : UnityEvent<char> { }
  [System.Serializable] public class UnityEventInt : UnityEvent<int> { }
  [System.Serializable] public class UnityEventFloat : UnityEvent<float> { }
  [System.Serializable] public class UnityEventDouble : UnityEvent<double> { }
  [System.Serializable] public class UnityEventString : UnityEvent<string> { }
}
