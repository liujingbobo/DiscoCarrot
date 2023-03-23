using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BooleanUtility
{
    /// <summary>
    /// if the flag is set to true, it will return true the next time accessed, but immediately consumed
    /// </summary>
    public class OneUseFlag
    {
        private bool _value = false;

        public bool Value
        {
            get {
                if (_value)
                {
                    _value = false;
                    return true;
                }
                return false;
            }
            set
            {
                _value = value;
            }
        }
    }
}
