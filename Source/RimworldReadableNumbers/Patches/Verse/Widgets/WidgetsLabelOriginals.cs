using System;
using LudeonTK;
using RimworldReadableNumbers.Patches.Unity.Gui;
using UnityEngine;
using Verse;

namespace RimworldReadableNumbers.Patches.Verse.Widgets
{
    public class WidgetsLabelOriginals
    {
        public static void LabelWidgetOriginal(Rect rect, GUIContent content)
        {
            GuiLabelReversePatch.LabelOriginal(rect, content, Text.CurFontStyle);
        }

        public static void LabelWidgetOriginal(Rect rect, string label)
        {
            Rect position = rect;
            float num = Prefs.UIScale / 2f;
            if (Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > float.Epsilon)
            {
                position.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect.xMin);
                position.yMin = UIScaling.AdjustCoordToUIScalingFloor(rect.yMin);
                position.xMax = UIScaling.AdjustCoordToUIScalingCeil(rect.xMax + 1E-05f);
                position.yMax = UIScaling.AdjustCoordToUIScalingCeil(rect.yMax + 1E-05f);
            }
            GuiLabelReversePatch.LabelOriginal(position, new GUIContent(label), Text.CurFontStyle); // edited to use GUIContent
        }

        public static void LabelWidgetOriginal(Rect rect, TaggedString label)
        {
            LabelWidgetOriginal(rect, label.Resolve());
        }

        public static void LabelWidgetOriginal(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
        {
            if (!text.NullOrEmpty())
            {
                float num = Text.CalcHeight(text, width);
                Rect rect = new Rect(x, curY, width, num);
                if (!tip.text.NullOrEmpty() || tip.textGetter != null)
                {
                    float x2 = Text.CalcSize(text).x;
                    Rect rect2 = new Rect(rect.x, rect.y, x2, num);
                    DevGUI.DrawHighlightIfMouseover(rect2);
                    TooltipHandler.TipRegion(rect2, tip);
                }
                LabelWidgetOriginal(rect, text);
                curY += num;
            }
        }

        public static void LabelWidgetOriginal(Rect rect, ref float y, string text, TipSignal tip = default(TipSignal))
        {
            if (!text.NullOrEmpty())
            {
                LabelWidgetOriginal(rect.x, ref y, rect.width, text, tip);
            }
        }
    }
}