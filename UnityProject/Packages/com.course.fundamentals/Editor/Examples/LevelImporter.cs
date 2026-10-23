using System.IO;
using Course.Fundamentals.Core;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Course.Fundamentals.EditorTools.Examples
{
    [ScriptedImporter(1, "level")]
    public sealed class LevelImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var data = ScriptableObject.CreateInstance<LevelData>();
            data.SetRows(File.ReadAllLines(ctx.assetPath));

            ctx.AddObjectToAsset("level", data);
            ctx.SetMainObject(data);
        }
    }
}