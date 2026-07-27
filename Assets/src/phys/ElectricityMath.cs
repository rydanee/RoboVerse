using UnityEngine;

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

  public static float Calculate_I_By_Ohms_Law(float U, float R)
  {
    if (!checkR(R)) return -1f;
    return U / R;
  }

  public static float Calculate_U_By_Ohms_Law(float I, float R)
  {
    if (!checkR(R)) return -1f;
    return I * R;
  }

  public static float Calculate_R_By_Ohms_Law(float U, float I)
  {
    if (!checkR(R)) return -1f;
    return U / I;
  }
}
