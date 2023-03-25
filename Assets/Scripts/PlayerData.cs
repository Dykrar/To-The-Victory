using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")]
public class PlayerData : MonoBehaviour
{
    public int HP;
    public int MaxHP;
    public int ATK;
    public Vector3Int position;
}
