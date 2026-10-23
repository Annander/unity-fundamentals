using System;
using UnityEngine;

namespace Course.Fundamentals.Core
{
    public sealed class LevelData : ScriptableObject
    {
        [SerializeField] private string[] rows = Array.Empty<string>();

        public string[] Rows => rows;

        public void SetRows(string[] value) => rows = value;
    }
}