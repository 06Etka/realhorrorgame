using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    [SerializeField] float maxHealth;
    [SerializeField] float maxSanity;
    [SerializeField] float maxStamina;
    float health;
    float sanity;
    public float stamina { get; private set; }

    private void Awake()
    {
        PlayerStats[] stats = FindObjectsOfType<PlayerStats>();
        if(stats.Length > 1)
        {
            print("There are more than 1 PlayerStats instance!");
            return;
        }
        else
        {
            print("PlayerStats instance has been set!");
            Instance = this;
        }
    }

    private void Start()
    {
        health = SetValue(maxHealth);
        sanity = SetValue(maxSanity);
        stamina = SetValue(maxStamina);
    }

    public void ConsumeStamina(float _amt, bool _isOvertime)
    {
        stamina = DecreaseValue(stamina, _amt, _isOvertime);
    }

    public void RegainStamina(float _amt, bool _isOvertime)
    {
        stamina = IncreaseValue(stamina, _amt, _isOvertime);
    }

    float SetValue(float _amt)
    {
        return _amt;
    }
    
    public float IncreaseValue(float _value, float _increaseAmt, bool _isOvertime)
    {
        if (_isOvertime)
        {
            _value += _increaseAmt * Time.deltaTime;
        }
        else
        {
            _value += _increaseAmt;
        }

        return _value;
    }

    public float DecreaseValue(float _value, float _decreaseAmt, bool _isOvertime)
    {
        if(_isOvertime)
        {
            _value -= _decreaseAmt * Time.deltaTime;
        } else
        {
            _value -= _decreaseAmt;
        }
        return _value;

    }
}