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

    public static float Calculate_I_from_UR(float U, float R)
    {
      if (!checkR(R)) return -1f;
      return U / R;
    }

    public static float Calculate_I_from_RU(float P, float U)
    {
      return P / U;
    }

    public static float Calculate_I_from_PR(float P, float R)
    {
      if (!checkR(R)) return -1f;
      return Mathf.Sqrt(P / R);
    }

    //Voltage formulas

    public static float Calculate_U_from_IR(float I, float R)
    {
      if (!checkR(R)) return -1f;
      return I * R;
    }

    public static float Calculate_U_from_PI(float P, float I)
    {
      if (!checkI(I)) return -1f;
      return P / I;
    }

    public static float Calculate_U_from_PR(float P, float R)
    {
      if (!checkR(R)) return -1f;
      return Mathf.Sqrt(P * R);
    }

    //Resistance formulas

    public static float Calculate_R_from_UI(float U, float I)
    {
      if (!checkI(I)) return -1f;
      return U / I;
    }

    public static float Calculate_R_from_UP(float U, float P)
    {
      return Mathf.Pow(U, 2) / P;
    }

    public static float Calculate_R_from_PI(float P, float I)
    {
      if (!checkI(I)) return -1f;
      return P / Mathf.Pow(I, 2);
    }

    //Power formulas

    public static float Calculate_P_from_UI(float U, float I)
    {
      return U * I;
    }

    public static float Calculate_P_from_RI(float R, float I)
    {
      if (!checkR(R)) return -1f;
      return R * Mathf.Pow(I, 2);
    }

    public static float Calculate_P_from_UR(float U, float R)
    {
      if (!checkR(R)) return -1f;
      return Mathf.Pow(U, 2) / R;
    }

  }
}
