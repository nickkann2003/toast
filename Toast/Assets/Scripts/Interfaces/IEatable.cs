using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEatable
{
    bool IsEatable { get; }
    void TakeBite();
    void EatWhole();
}
