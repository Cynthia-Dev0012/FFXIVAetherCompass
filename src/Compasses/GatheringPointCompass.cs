﻿using AetherCompass.Common;
using AetherCompass.Configs;
using AetherCompass.UI.GUI;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ImGuiNET;
using Lumina.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Sheets = Lumina.Excel.GeneratedSheets;

namespace AetherCompass.Compasses
{
    public class GatheringPointCompass : Compass
    {
        public override string CompassName => "Gathering Point Compass";
        public override string Description => "Detecting nearby gathering points";

        private GatheringPointCompassConfig GatheringConfig => (GatheringPointCompassConfig)compassConfig;

        private static readonly Vector4 infoTextColour = new(.55f, .98f, 1, 1);
        private static readonly float infoTextShadowLightness = .1f;


        public GatheringPointCompass(PluginConfig config, GatheringPointCompassConfig compassConfig, CompassDetailsWindow detailsWindow, CompassOverlay overlay)
            : base(config, compassConfig, detailsWindow, overlay) { }

        public override bool IsEnabledTerritory(uint terr)
            => CompassUtil.GetTerritoryType(terr)?.TerritoryIntendedUse == 1
            // TODO: diadem? 
            || CompassUtil.GetTerritoryType(terr)?.TerritoryIntendedUse == 47;

        private protected override unsafe bool IsObjective(GameObject* o)
            => o != null && o->ObjectKind == (byte)ObjectKind.GatheringPoint;

        public override unsafe DrawAction? CreateDrawDetailsAction(GameObject* o)
            => new(() =>
            {
                if (o == null) return;
                ImGui.Text($"Lv{GetGatheringLevel(o->DataID)} {CompassUtil.GetName(o)}");
                ImGui.BulletText($"{CompassUtil.GetMapCoordInCurrentMapFormattedString(o->Position)} (approx.)");
                ImGui.BulletText($"{CompassUtil.GetDirectionFromPlayer(o)},  " +
                    $"{CompassUtil.Get3DDistanceFromPlayerDescriptive(o, false)}");
                ImGui.BulletText(CompassUtil.GetAltitudeDiffFromPlayerDescriptive(o));
                DrawFlagButton($"##{(long)o}", CompassUtil.GetMapCoordInCurrentMap(o->Position));
                ImGui.Separator();
            });

        public override unsafe DrawAction? CreateMarkScreenAction(GameObject* o)
            => new(() =>
            {
                if (o == null) return;
                var icon = IconManager.GetGatheringMarkerIcon(GetGatheringPointIconId(o->DataID));
                string descr = $"Lv{GetGatheringLevel(o->DataID)} {CompassUtil.GetName(o)}, {CompassUtil.Get3DDistanceFromPlayerDescriptive(o, true)}";
                DrawScreenMarkerDefault(o, icon, IconManager.MarkerIconSize,
                    .9f, descr, infoTextColour, infoTextShadowLightness, out _);
            });

        private protected override void DisposeCompassUsedIcons() => IconManager.DisposeGatheringPointCompassIcons();

        private protected override unsafe string GetClosestObjectiveDescription(GameObject* o)
            => CompassUtil.GetName(o);


        private static ExcelSheet<Sheets.GatheringPoint>? GatheringPointSheet
            => Plugin.DataManager.GetExcelSheet<Sheets.GatheringPoint>();
        
        private static ExcelSheet<Sheets.GatheringPointBase>? GatheringPointBaseSheet
            => Plugin.DataManager.GetExcelSheet<Sheets.GatheringPointBase>();

        private static ExcelSheet<Sheets.GatheringType>? GatheringTypeSheet
            => Plugin.DataManager.GetExcelSheet<Sheets.GatheringType>();

        // True for those that use special icon;
        private static bool IsSpecialGatheringPointType(GatheringPointType type)
            => type == GatheringPointType.Unspoiled || type == GatheringPointType.Folklore
            || type == GatheringPointType.SFShadow || type == GatheringPointType.DiademClouded;

        // gatheringPointType is GatheringPoint.Type, not GatheringType sheet rows
        private static bool IsSpecialGatheringPointType(byte gatheringPointType)
            => IsSpecialGatheringPointType((GatheringPointType)gatheringPointType);

        private static bool IsSpecialGatheringPoint(uint dataId)
        {
            var gatherPointData = GatheringPointSheet?.GetRow(dataId);
            if (gatherPointData == null) return false;
            return IsSpecialGatheringPointType(gatherPointData.Type);
        }


        private static uint GetGatheringPointIconId(byte gatheringType, GatheringPointType gatheringPointType)
        {
            var typeRow = GatheringTypeSheet?.GetRow(gatheringType);
            if (typeRow == null) return 0;
            return (uint)(IsSpecialGatheringPointType(gatheringPointType) ? typeRow.IconOff : typeRow.IconMain);
        }

        private static uint GetGatheringPointIconId(uint dataId)
        {
            var gatherType = GatheringPointSheet?.GetRow(dataId)?.GatheringPointBase.Value?.GatheringType.Value;
            if (gatherType == null) return 0;
            return (uint)(IsSpecialGatheringPoint(dataId) ? gatherType.IconOff : gatherType.IconMain);
        }

        private static byte GetGatheringLevel(uint dataId)
        {
            var gatherPointBase = GatheringPointSheet?.GetRow(dataId)?.GatheringPointBase.Value;
            return gatherPointBase == null ? byte.MinValue : gatherPointBase.GatheringLevel;
        }


        #region FUTURE/ ExportedGatheringPoint
#if FUTURE
        private IEnumerable<uint>? exportedGatheringPointRowIds;
        
        private static ExcelSheet<Sheets.ExportedGatheringPoint>? ExportedGatheringPointSheet
            => Plugin.DataManager.GetExcelSheet<Sheets.ExportedGatheringPoint>();

        private static ExcelSheet<Sheets.GatheringPointName>? GatheringPointNameSheet
            => Plugin.DataManager.GetExcelSheet<Sheets.GatheringPointName>();


        private void UpdateTerritoryExportedGatheringPointsOnTerrInit(ushort terr)
        {
            exportedGatheringPointRowIds = GatheringPointSheet?.Where(p => p.TerritoryType.Row == terr)
                .Select(p => p.GatheringPointBase.Row);
        }

        private void DrawUiForTerritoryExportedGatheringPoints()
        {
            if (exportedGatheringPointRowIds == null) return;
            if (GatheringPointBaseSheet == null || ExportedGatheringPointSheet == null) return;
            foreach (var rowId in exportedGatheringPointRowIds)
            {
                var gatherPointBase = GatheringPointBaseSheet.GetRow(rowId);
                var exported = ExportedGatheringPointSheet.GetRow(rowId);
                if (gatherPointBase == null || exported == null || exported.GatheringPointType == 0) continue;
                var pos = new Vector3(exported.X, exported.Y, 0);
                var name = $"(Nodes Group: Lv{gatherPointBase!.GatheringLevel} " 
                    + InferGatheringPointName(exported.GatheringType, (GatheringPointType)exported.GatheringPointType)
                    + ")";
                var dist = CompassUtil.Get3DDistanceFromPlayer(pos);
                if (MarkScreen)
                    overlay.AddDrawAction(new(() =>
                    {
                        var icon = IconManager.GetGatheringMarkerIcon(GetGatheringPointIconId(exported.GatheringType, (GatheringPointType)exported.GatheringPointType));
                        string descr = $"{name}, {CompassUtil.DistanceToDescriptiveString(dist, true)}";
                        DrawScreenMarkerDefault(pos, 0, icon, IconManager.MarkerIconSize, .9f, descr, infoTextColour, infoTextShadowLightness, out _);
                    }, dist < 200));
                if (ShowDetail)
                    detailsWindow.AddDrawAction(this, new(() =>
                    {
                        ImGui.Text(name);
                        ImGui.BulletText($"{CompassUtil.GetMapCoordInCurrentMapFormattedString(pos, false)} (approx.)");
                        ImGui.BulletText($"{CompassUtil.GetDirectionFromPlayer(pos)},  " +
                            $"{CompassUtil.DistanceToDescriptiveString(dist, false)}");
                        ImGui.BulletText(CompassUtil.GetAltitudeDiffFromPlayerDescriptive(pos));
                        DrawFlagButton($"##NodesGroup_{rowId}", CompassUtil.GetMapCoordInCurrentMap(pos));
                        ImGui.Separator();
                    }, dist < 200));
            }
        }

        private static string InferGatheringPointName(byte gatheringType, GatheringPointType gatheringPointType)
            => GatheringPointNameSheet?.GetRow(
                gatheringType switch
                {
                    // mining
                    0 => gatheringPointType switch
                    {
                        GatheringPointType.Unspoiled => 5,
                        GatheringPointType.Ephemeral => 9,  // TODO: aetherial reduction one still this name?
                        GatheringPointType.Folklore => 13,
                        GatheringPointType.DiademClouded => 23,
                        _ => 1
                    },
                    // quarrying
                    1 => gatheringPointType switch
                    {
                        GatheringPointType.Unspoiled => 6,
                        GatheringPointType.Ephemeral => 10,
                        GatheringPointType.Folklore => 14,
                        GatheringPointType.DiademClouded => 24,
                        _ => 2
                    },
                    // logging
                    2 => gatheringPointType switch
                    {
                        GatheringPointType.Unspoiled => 7,
                        GatheringPointType.Ephemeral => 11,
                        GatheringPointType.Folklore => 15,
                        GatheringPointType.DiademClouded => 25,
                        _ => 3
                    },
                    // harvesting
                    3 => gatheringPointType switch
                    {
                        GatheringPointType.Unspoiled => 8,
                        GatheringPointType.Ephemeral => 12,
                        GatheringPointType.Folklore => 16,
                        GatheringPointType.DiademClouded => 26,
                        _ => 4
                    },
                    // spearfishing
                    4 => gatheringPointType switch
                    {
                        GatheringPointType.SFShadow => 22,
                        _ => 21
                    },
                    _ => 0,
                })?.Singular ?? string.Empty;
#endif
        #endregion
    }


    // mapping GatheringPoint.Type column, not GatheringType sheet rows
    public enum GatheringPointType: byte
    {
        None,
        Basic,
        Unspoiled,
        Leve,
        Ephemeral,  // for aetherial reduction,
        Folklore,
        SFShadow,   // spearfishing special
        DiademBasic,
        DiademClouded,  // diadem special
    }
}