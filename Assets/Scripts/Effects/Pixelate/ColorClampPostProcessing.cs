using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColorClampPostProcessing : PostProcessEffectSettings
{
    public enum Target { ViewSpace, WorldSpace }

    [System.Serializable]
    public sealed class TargetParameter : ParameterOverride<Target> {}

    public TargetParameter target = new TargetParameter();
    public FloatParameter columns = new FloatParameter() { value = 64f };
    public FloatParameter rows = new FloatParameter() { value = 64f };

    public FloatParameter color_depth = new FloatParameter() { value = 6f };

}

public class ColorClampPostProcessingRenderer<T> : PostProcessEffectRenderer<T> where T : ColorClampPostProcessing
{
    private int _ColumnsID, _RowsID, _ColorDepthID;

    private Shader shader;

    public override void Init()
    {
        base.Init();
        shader = Shader.Find("Hidden/ColorClamp");
        _ColumnsID = Shader.PropertyToID("_Columns");
        _RowsID = Shader.PropertyToID("_Rows");
        _ColorDepthID = Shader.PropertyToID("_ColorDepth");
    }

    public override void Render(PostProcessRenderContext context)
    {
        var cmd = context.command;
        cmd.BeginSample("ColorClamp");
        var sheet = context.propertySheets.Get(shader);

        sheet.properties.SetFloat(_ColumnsID, settings.columns);
        sheet.properties.SetFloat(_RowsID, settings.rows);
        sheet.properties.SetFloat(_ColorDepthID, settings.color_depth);

        var pass = (int)settings.target.value;
        cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, pass);

        cmd.EndSample("ColorClamp");
    }

    // public override DepthTextureMode GetCameraFlags()
    // {
    //     // if (settings == null)
    //     return DepthTextureMode.None;

    //     // return DepthTextureMode.Depth;
    //     // return base.GetCameraFlags();
    // }
}

[System.Serializable]
[PostProcess(typeof(ColorClampPostProcessingRenderer), PostProcessEvent.AfterStack, "Stylized/ColorClampProcess")]
public sealed class ColorClamp : ColorClampPostProcessing { }

public sealed class ColorClampPostProcessingRenderer : ColorClampPostProcessingRenderer<ColorClampPostProcessing> { }
