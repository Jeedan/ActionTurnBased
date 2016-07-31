using UnityEngine;
using System.Collections;

public interface IAbility
{
    IEnumerator Anticipate();
    IEnumerator Attack();
    IEnumerator FinishAttack();
}
