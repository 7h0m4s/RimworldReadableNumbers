using System;
using System.Collections.Generic;
using RimworldReadableNumbers.Utility;
using UnityEngine;
using UnityStubs.Gui;

namespace RimworldReadableNumbers;

public class RnGuiBackend : IGuiBackend
{
    private readonly IGuiBackend _guiBackendImplementation;

    public RnGuiBackend(IGuiBackend guiBackendImplementation)
    {
        _guiBackendImplementation = guiBackendImplementation;
    }

    public Vector2 MeasureText(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth,
        bool richText = true)
    {
        if (!Utility.Patching.DisableReadableNumberFormatting)
        {
            Processing.ProcessLabel(ref text);
        }
        return _guiBackendImplementation.MeasureText(fontIndex, fontSize, fontStyle, text, wrapWidth, richText);
    }

    public float LineSpacing(int fontIndex, float fontSize)
    {
        return _guiBackendImplementation.LineSpacing(fontIndex, fontSize);
    }

    public void FontMetrics(int fontIndex, out float importSize, out float ascent, out float lineSpacing)
    {
        _guiBackendImplementation.FontMetrics(fontIndex, out importSize, out ascent, out lineSpacing);
    }

    public int RegisterFont(string name, byte[] fontData, float importSize, int renderingMode)
    {
        return _guiBackendImplementation.RegisterFont(name, fontData, importSize, renderingMode);
    }

    public int TextIndexAt(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, Vector2 local,
        bool richText = true)
    {
        return _guiBackendImplementation.TextIndexAt(fontIndex, fontSize, fontStyle, text, wrapWidth, local, richText);
    }

    public Vector2 TextCursorPos(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, int index,
        out float lineHeight, bool richText = true)
    {
        return _guiBackendImplementation.TextCursorPos(fontIndex, fontSize, fontStyle, text, wrapWidth, index, out lineHeight, richText);
    }

    public void TextSelectionRects(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, int from,
        int to, List<Rect> rects, bool richText = true)
    {
        _guiBackendImplementation.TextSelectionRects(fontIndex, fontSize, fontStyle, text, wrapWidth, from, to, rects, richText);
    }

    public void DrawText(Rect rect, string text, int fontIndex, float fontSize, FontStyle fontStyle, TextAnchor anchor,
        bool wordWrap, Color color, Rect clipRect = new Rect(), bool richText = true)
    {
        if (!Utility.Patching.DisableReadableNumberFormatting)
        {
            Processing.ProcessLabel(ref text);
        }
        _guiBackendImplementation.DrawText(rect, text, fontIndex, fontSize, fontStyle, anchor, wordWrap, color, clipRect, richText);
    }

    public void DrawTexture(Rect rect, Texture image, ScaleMode scaleMode, Color color)
    {
        _guiBackendImplementation.DrawTexture(rect, image, scaleMode, color);
    }

    public void DrawTextureWithTexCoords(Rect rect, Texture image, Rect texCoords, Color color)
    {
        _guiBackendImplementation.DrawTextureWithTexCoords(rect, image, texCoords, color);
    }

    public void DrawTextureWithMaterial(Rect rect, Texture image, Rect texCoords, Color color, Material material,
        Waylith.Rendering.Material passMaterial)
    {
        _guiBackendImplementation.DrawTextureWithMaterial(rect, image, texCoords, color, material, passMaterial);
    }

    public void FillRect(Rect rect, Color color)
    {
        _guiBackendImplementation.FillRect(rect, color);
    }

    public void FillRectRounded(Rect rect, Color color, float rounding)
    {
        _guiBackendImplementation.FillRectRounded(rect, color, rounding);
    }

    public void DrawTextureQuad(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Texture image, Color color)
    {
        _guiBackendImplementation.DrawTextureQuad(p0, p1, p2, p3, image, color);
    }

    public void DrawPrimitives(int mode, ReadOnlySpan<GuiVertex> vertices, Texture texture, bool screenSpace)
    {
        _guiBackendImplementation.DrawPrimitives(mode, vertices, texture, screenSpace);
    }

    public void SetMatrix(Matrix4x4 matrix)
    {
        _guiBackendImplementation.SetMatrix(matrix);
    }

    public void PushGroup(Rect rect)
    {
        _guiBackendImplementation.PushGroup(rect);
    }

    public void PushScrollView(Rect clipRect, Vector2 contentOffset)
    {
        _guiBackendImplementation.PushScrollView(clipRect, contentOffset);
    }

    public void PopGroup()
    {
        _guiBackendImplementation.PopGroup();
    }

    public void GetGroup(int index, out Rect rect, out Vector2 contentOrigin)
    {
        _guiBackendImplementation.GetGroup(index, out rect, out contentOrigin);
    }

    public void BeginWindowContent(Rect windowRect)
    {
        _guiBackendImplementation.BeginWindowContent(windowRect);
    }

    public void EndWindowContent()
    {
        _guiBackendImplementation.EndWindowContent();
    }

    public void PushClipRect(Rect rect)
    {
        _guiBackendImplementation.PushClipRect(rect);
    }

    public void PopClipRect()
    {
        _guiBackendImplementation.PopClipRect();
    }

    public void ActiveRenderTextureChanged(RenderTexture active)
    {
        _guiBackendImplementation.ActiveRenderTextureChanged(active);
    }

    public void SetTextLanguage(string languageFolderName)
    {
        _guiBackendImplementation.SetTextLanguage(languageFolderName);
    }

    public Vector2 ScreenToGuiPoint(Vector2 screenPoint)
    {
        return _guiBackendImplementation.ScreenToGuiPoint(screenPoint);
    }

    public Vector2 GuiToScreenPoint(Vector2 guiPoint)
    {
        return _guiBackendImplementation.GuiToScreenPoint(guiPoint);
    }

    public bool MouseOutsideHitClip => _guiBackendImplementation.MouseOutsideHitClip;

    public int GroupCount => _guiBackendImplementation.GroupCount;

    public bool InlineControlsObscured => _guiBackendImplementation.InlineControlsObscured;
}