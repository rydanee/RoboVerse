using UnityEngine;

namespace phys
{
  public static class ElectricityMath
  {
    public static bool checkR(float R)
    {
      if (R <= 0)
      {
        Debug.Log("Короткое замыкание!");
        return false;
      }

      return true;
    }

    public static bool checkI(float I)
    {
      if (I == 0f)
      {
        Debug.Log("Ток равен нулю!");
        return false;
      }

      return true;
    }

    //Current formulas

    public static float Calculate_I(float U, float R)
    {
      if (!checkR(R)) return -1f;
      return U / R;
    }

    public static float Calculate_I(float P, float U)
    {
      return P / U;
    }

    public static float Calculate_I(float P, float R)
    {
      if (!checkR(R)) return -1f;
      return Mathf.Sqrt(P / R);
    }

    //Voltage formulas

    public static float Calculate_U(float I, float R)
    {
      if (!checkR(R)) return -1f;
      return I * R;
    }

    public static float Calculate_U(float P, float I)
    {
      if (!checkI(I)) return -1f;
      return P / I;
    }

    public static float Calculate_U(float P, float R)
    {
      if (!checkR(R)) return -1f;
      return Mathf.Sqrt(P * R);
    }

    //Resistance formulas

    public static float Calculate_R(float U, float I)
    {
      if (!checkI(I)) return -1f;
      return U / I;
    }

    public static float Calculate_R(float U, float P)
    {
      return Mathf.Pow(U, 2) / P;
    }

    public static float Calculate_R(float P, float I)
    {
      if (!checkI(I)) return -1f;
      return P / Mathf.Pow(I, 2);
    }

    //Power formulas

    public static float Calculate_P(float U, float I)
    {
      return U * I;
    }

    public static float Calculate_P(float R, float I)
    {
      if (!checkR(R)) return -1f;
      return R * Mathf.Pow(I, 2);
    }

    public static float Calculate_P(float U, float R)
    {
      if (!checkR(R)) return -1f;
      return Mathf.Pow(U, 2) / R;
    }

  }
}
