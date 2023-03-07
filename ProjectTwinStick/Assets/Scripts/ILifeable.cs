using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifeable
{
        public float GetMaxHp();
        public float GetCurrentHp();
        public void SetMaxHp(float value);
        public void IncreaseMaxHp(float amount);
        public event Action<float> OnSetMaxHp;
        public event Action<float> OnIncreaseMaxHp;
        public void DecreaseMaxHp(float amount);
        public event Action<float> OnDecreaseMaxHp;
        public void SetCurrentHp(float value);
        public event Action<float> OnSetCurrentHp;
        public void IncreaseCurrentHp(float amount);

        public event Action<float> OnIncreaseCurrentHp;
        public void DecreaseCurrentHp(float amount);
        public event Action<float> OnDecreaseCurrentHp;
}

