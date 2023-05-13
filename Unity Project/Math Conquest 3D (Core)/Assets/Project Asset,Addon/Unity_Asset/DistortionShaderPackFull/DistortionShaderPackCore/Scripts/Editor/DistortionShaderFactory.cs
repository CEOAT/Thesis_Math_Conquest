using UnityEditor;
using UnityEngine;

namespace nightowl.distortionshaderpack
{
    public class DistortionShaderFactory
    {

        private static string shaderCode;
        private static int tabs = 0;
        private static DistortionShaderConfig config;

        [MenuItem("Assets/Create/DistortionShader")]
        [MenuItem("Window/DistortionShaderPack/Create DistortionShader")]
        private static void OpenShaderAssistent()
        {
            var window = EditorWindow.GetWindow(typeof(DistortionShaderAssistentWindow));
            window.title = "DistortionShader Assistent";
            window.Show();
        }

        public static void Build(DistortionShaderConfig aConfig)
        {
            Debug.Log("DistortionShaderFactory creating shader for: " + aConfig.ToString());
            shaderCode = "";
            config = aConfig;
            StartShaderCode();
            AddProperties();
            AddCategory();
            EndShaderCode();
            WriteFile();
        }

        private static void StartShaderCode()
        {
            AddLine("Shader \"DistortionShaderPack/" + config.name + "\"");
            AddLine("{");
            tabs++;
        }

        private static void AddProperties()
        {
            AddLine("Properties");
            AddLine("{");
            tabs++;
            if (config.UseRenderTexture)    AddLine(config.CustomGrabTextureName + "(\"" + config.CustomGrabTextureName + "\", 2D) = \"black\" { }");
            if (config.UseColor)            AddLine("_MainColor(\"Main color\", Color) = (0,0,0,1)");
            if (config.UsesColors)          AddLine("_StrengthColor(\"Color strength\", Float) = 1");

            if (config.UseCenterDistortion) AddLine("_DistortionStrength (\"Distortion strength\", Range(-2,2)) = 0.3");
            if (config.UseCenterDistortion) AddLine("_DistortionCircle (\"Distortion circle\", Range(0,1)) = 1");

            if (config.UseAlpha)            AddLine("_StrengthAlpha(\"Alpha strength\", Range(0,2)) = 0");
            if (config.UseBlending)         AddLine("_StrengthBlend(\"Blend strength\", Float) = 5");

            if (config.UseNormal)           AddLine("_NormalTexture(\"Normal\", 2D) = \"blue\" { }");
            if (config.UseNormal)           AddLine("_NormalTexStrength(\"Normal strength\", Range(0,1)) = 0");
            if (config.UseNormal)           AddLine("_NormalTexFrameless(\"Normal circle\", Range(0,1)) = 1.0");
            if (config.UseNormalMovement)   AddLine("_UVOffset(\"UVOffset XY, ignore ZW\", Vector) = (0,0.01,0,0)");
            tabs--;
            AddLine("}");
        }

        private static void AddCategory()
        {
            AddLine("Category");
            AddLine("{");
            tabs++;
            AddCategoryConfig();
            AddSubShader();
            tabs--;
            AddLine("}");
        }

        private static void AddCategoryConfig()
        {
            if (!config.UseURP) AddLine("Tags { \"Queue\" = \"Transparent\" \"RenderType\" = \"Transparent\" \"IgnoreProjector\" = \"True\" }");
            if (config.UseURP) AddLine("Tags { \"Queue\" = \"Transparent\" \"RenderType\" = \"Opaque\" \"RenderPipeline\" = \"LightweightPipeline\" \"IgnoreProjector\" = \"True\" }");
            if (!config.UseURP) AddLine("Blend SrcAlpha OneMinusSrcAlpha");
            AddLine("ZWrite Off");
        }

        private static void AddSubShader()
        {
            AddLine("SubShader");
            AddLine("{");
            tabs++;
            if (!config.UseURP && !config.UseRenderTexture) AddGrabPass();
            AddMainPass();
            tabs--;
            AddLine("}");
        }

        private static void AddGrabPass()
        {
            AddLine("GrabPass");
            AddLine("{");
            tabs++;
            AddLine("\"_GrabTexture\"");
            AddLine("Name \"BASE\"");
            AddLine("Tags { \"LightMode\" = \"Always\" }");
            tabs--;
            AddLine("}");
        }

        private static void AddMainPass()
        {
            AddLine("Pass");
            AddLine("{");
            tabs++;
            AddLine("Name \"BASE\"");
            if (!config.UseURP) AddLine("Tags { \"LightMode\" = \"Always\" }");
            if (config.UseURP) AddLine("Tags { \"LightMode\" = \"LightweightForward\" }");
            AddCGProgram();
            tabs--;
            AddLine("}");
        }

        private static void AddCGProgram()
        {
            AddLine("CGPROGRAM");
            AddLine("#pragma vertex vert");
            AddLine("#pragma fragment frag");
            AddLine("#include \"UnityCG.cginc\"");
            //AddLine("#pragma multi_compile_fog");
            AddShaderVaribles();
            AddVertexInput();
            AddFragmentInput();
            AddVertexCode();
            AddFragmentCode();
            AddLine("ENDCG");
        }

        private static void AddShaderVaribles()
        {
            AddLine("");
                                            AddLine("sampler2D " + config.CustomGrabTextureName + ";");
            if (config.UseBlending)         AddLine("sampler2D _LastCameraDepthTexture;");
            if (config.UseCenterDistortion) AddLine("float _DistortionStrength;");
            if (config.UseCenterDistortion) AddLine("float _DistortionCircle;");
            if (config.UseColor)            AddLine("float4 _MainColor;");
            if (config.UsesColors)          AddLine("float _StrengthColor;");
            if (config.UseAlpha)            AddLine("float _StrengthAlpha;");
            if (config.UseBlending)         AddLine("float _StrengthBlend;");
            if (config.UseNormal)           AddLine("sampler2D _NormalTexture;");
            if (config.UseNormal)           AddLine("float4 _NormalTexture_ST;");
            if (config.UseNormal)           AddLine("float _NormalTexStrength;");
            if (config.UseNormal)           AddLine("float _NormalTexFrameless;");
            if (config.UseNormalMovement)   AddLine("float4 _UVOffset;");
        }

        private static void AddVertexInput()
        {
            AddLine("");
            AddLine("struct VertexInput");
            AddLine("{");
            tabs++;
            AddLine("float4 vertex : POSITION;");
            AddLine("float2 texcoord0 : TEXCOORD0;");
            if (config.UseVertexColor) AddLine("float4 color : COLOR;");
            tabs--;
            AddLine("};");
        }

        private static void AddFragmentInput()
        {
            AddLine("");
            AddLine("struct Vert2Frag");
            AddLine("{");
            tabs++;
            AddLine("float4 position : SV_POSITION;");
            AddLine("float4 uv_grab : TEXCOORD0;");
            AddLine("float2 uv : TEXCOORD1;");
            int texcoordid = 2;
            if (config.UseNormal) AddLine("float2 uv_normal : TEXCOORD"+ texcoordid + ";");
            if (config.UseNormal) texcoordid++;
            if (config.UseNormalMovement) AddLine("float2 movement: TEXCOORD"+ texcoordid + ";");
            if (config.UseNormalMovement) texcoordid++;
            if (config.UseVertexColor) AddLine("float4 color : TEXCOORD"+ texcoordid + ";");
            //if (config.UseVertexColor) texcoordid++;
            tabs--;
            AddLine("};");
        }

        private static void AddVertexCode()
        {
            AddLine("Vert2Frag vert (VertexInput vertIn)");
            AddLine("{");
            tabs++;
            AddLine("Vert2Frag output;");
            AddLine("output.position = UnityObjectToClipPos(vertIn.vertex);");
            AddLine("output.uv_grab = ComputeGrabScreenPos(output.position);");
            AddLine("output.uv = vertIn.texcoord0;");
            if (config.UseNormal) AddLine("output.uv_normal = vertIn.texcoord0.xy * _NormalTexture_ST.xy + _NormalTexture_ST.zw;");
            if (config.UseNormalMovement) AddLine("output.movement = _UVOffset.xy*_Time.y;");
            if (config.UseVertexColor) AddLine("output.color = vertIn.color;");
            AddLine("return output;");
            tabs--;
            AddLine("}");
        }

        private static void AddFragmentCode()
        {
            AddVectorFromCenterCode();
            AddDistortionStrengthCode();
            if (config.UseNormal) AddNormalCode();
            if (config.UseBlending) AddBlendCode();

            AddLine("half4 frag (Vert2Frag fragIn) : SV_Target");
            AddLine("{");
            tabs++;

            AddLine("float4 uvScreen = UNITY_PROJ_COORD(fragIn.uv_grab);");
            AddLine("float2 direction = getVectorFromCenter(fragIn.uv);");

            AddLine("float strength = getDistortionStrength(fragIn.uv);");
            if (config.UseCenterDistortion) AddLine("strength = (_DistortionCircle*strength + (1-_DistortionCircle)) * _DistortionStrength;");

            if (config.UseCenterDistortion) AddLine("float2 newDirection = direction * strength;");
            if (config.UseCenterDistortion) AddLine("uvScreen += float4(newDirection.x, newDirection.y, 0, 0);");
            if (config.UsesColors) AddLine("float2 influence = normalize(direction) * strength;");

            if (!config.UseNormalMovement) AddLine("float2 offset = float2(0,0);");
            if (config.UseNormalMovement) AddLine("float2 offset = fragIn.movement;");
            if (config.UseNormal) AddLine("float2 normal = getNormal(_NormalTexture, fragIn.uv_normal, fragIn.uv, offset, _NormalTexFrameless, _NormalTexStrength);");
            if (config.UseNormal) AddLine("uvScreen += float4(normal.x, normal.y, 0, 0);");
            if (config.UsesColors && config.UseNormal) AddLine("influence += normal.xy;");

            if (config.UseBlending) AddLine("float blend = getBlend(fragIn.uv_grab, _LastCameraDepthTexture) * _StrengthBlend;");

            string grabTexture = config.CustomGrabTextureName;
            AddLine("float4 final = tex2Dproj("+grabTexture+", uvScreen);");
            if (!config.UseAlpha) AddLine("float alpha = 1;");
            if (config.UseAlpha)  AddLine("float alpha = (1 - _StrengthAlpha) + (strength*_StrengthAlpha);");
            AddLine("final = float4(final.xyz, alpha);");
            if (config.UsesColors) AddLine("strength = saturate(sqrt(pow(abs(influence.x), 2.0) + pow(abs(influence.y), 2.0)) * _StrengthColor);");
            string mainColor = "";
            string vertexColor = "";
            if (config.UseColor) mainColor = "_MainColor*";
            if (config.UseVertexColor) vertexColor = "fragIn.color*";
            if (config.UsesColors) AddLine("final = final + ("+mainColor+vertexColor+"strength);");
            vertexColor = "";
            if (config.UseVertexColor) vertexColor = "*fragIn.color.a";
            if (config.UseBlending) AddLine("final.w = final.w*saturate(blend)"+ vertexColor + ";");
            mainColor = "";
            vertexColor = "";
            if (config.UseColor) mainColor = "*_MainColor.w";
            if (config.UseVertexColor) vertexColor = "*fragIn.color.w";
            AddLine("final.w = saturate(final.w" + mainColor + vertexColor +");");
            AddLine("return final;");
            tabs--;
            AddLine("}");
        }

        private static void AddVectorFromCenterCode()
        {
            AddLine("float2 getVectorFromCenter(float2 uv)");
            AddLine("{");
            string screenX = "_ScreenParams.x";
            if (config.UseURP) screenX = "(_ScreenParams.x*2)";
            AddLine("	float factor = _ScreenParams.y / "+screenX+";");
            AddLine("	float2 direction = float2((uv.x-0.5)*.5, (uv.y-0.5)) * factor;");
            AddLine("	return (direction);");
            AddLine("}");
        }

        private static void AddDistortionStrengthCode()
        {
            AddLine("float getDistortionStrength(float2 uv)");
            AddLine("{");
            AddLine("	float2 diff = float2(distance(0.5, uv.x), distance(0.5, uv.y)) * 2.0;");
            AddLine("	float dist = saturate(length(diff));");
            AddLine("	return 1.0-dist;");
            AddLine("}");
        }

        private static void AddNormalCode()
        {
            AddLine("float2 getNormal(sampler2D _NormalTexture, float2 normalUv, float2 uv, float2 uvOffset, float frameless, float strength)");
            AddLine("{");
            AddLine("	float2 normal = tex2D( _NormalTexture, normalUv+uvOffset ).zy;");
            AddLine("	float length = getDistortionStrength(uv);");

            AddLine("	float normalTexStrength = ((1-frameless) + frameless*length) * strength;");
            string uvCorrection = ".5";
            if (config.UseURP) uvCorrection = "1";
            AddLine("	normal.x = ((normal.x-"+uvCorrection+")*2) * normalTexStrength;");
            AddLine("	normal.y = ((normal.y-.5)*2) * normalTexStrength;");

            AddLine("	return normal;");
            AddLine("}");
        }

        private static void AddBlendCode()
        {
            AddLine("float getBlend(float4 posScreen, sampler2D depthTexture)");
            AddLine("{");
            AddLine("	float depth = tex2Dproj(depthTexture, posScreen);");
            AddLine("	depth = 1.0 / (_ZBufferParams.z * depth + _ZBufferParams.w);");
            AddLine("	return (depth - posScreen.w);");
            AddLine("}");
        }

        private static void EndShaderCode()
        {
            tabs--;
            AddLine("}");
        }

        private static void WriteFile()
        {
            UnityEngine.Debug.Log(shaderCode);
            System.IO.File.WriteAllText(GetFilePath(config.name), shaderCode);
            AssetDatabase.Refresh();
        }

        public static string GetFilePath(string name)
        {
            return Application.dataPath + "/" + name + ".shader";
        }

        private static void AddLine(string text)
        {
            string tab = "";
            for(int a=0;a<tabs;a++)
            {
                tab += "\t";
            }
            shaderCode += tab + text + "\n";
        }


    }
}