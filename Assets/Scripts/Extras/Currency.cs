using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Currency
{
    public static string ToCurrency(this float amount)
    {
        int length = amount.ToString().Length;
        if (length > 5)
        {
            return amount.ToString("0,,.##M");
        }
        else if (length > 2)
        {
            return amount.ToString("0,.##K");
        }

        return amount.ToString();
    }
}
