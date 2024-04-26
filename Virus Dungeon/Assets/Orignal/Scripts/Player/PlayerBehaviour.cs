using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour instance;

    public PlayerHealth health;
    public PlayerMovement move;
    public PlayerShooting shooting;
    public PlayerShootSuper shootSuper;
    public PlayerBlink blink;
    public PlayerSpecialState specialState;

    private void Awake()
    {
        instance = this;
        health = GetComponent<PlayerHealth>();
        move = GetComponent<PlayerMovement>();
        shooting = GetComponentInChildren<PlayerShooting>();
        shootSuper = GetComponentInChildren<PlayerShootSuper>();
        blink = GetComponent<PlayerBlink>();
        specialState = GetComponent<PlayerSpecialState>();

    }
}
