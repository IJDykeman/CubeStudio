
struct VertexToPixel
{
	float4 Position   	: POSITION;
    float4 Color		: COLOR0;
	float4 PosToUse : TEXCOORD0;
    float2 TextureCoords: TEXCOORD1;
	float3x3 worldSpaceToTangentSpace : TEXCOORD2;
	float3 ViewDirection : COLOR1;

	
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};





//------- Constants --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float3 xLightLoc;
float xAmbient;
bool xEnableLighting;
bool xShowNormals;
float3 xCamPos;
float3 xCamUp;
float3 xViewDirection;
float xPointSpriteSize;

//------- Texture Samplers --------

Texture xTexture;
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };
//change LINEAR to POINT for blocky textures

Texture xNormalMap;
sampler NormalMapSampler = sampler_state { texture = <xNormalMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

Texture xSpecularMap;
sampler SpecularMapSampler = sampler_state { texture = <xSpecularMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

Texture xHeightMap;
sampler HeightMapSampler = sampler_state { texture = <xHeightMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

Texture xGlossMap;
sampler GlossMapSampler = sampler_state { texture = <xGlossMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = mirror; AddressV = mirror; };

//------- Technique: Colored --------

VertexToPixel ColoredVS(float4 inPos : POSITION, float3 inNormal : NORMAL0, float4 inColor : COLOR, float4 inPaint : COLOR1, float2 texCoord : TEXCOORD0, float3 binormal : NORMAL1, float3 tangent: NORMAL2)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection) ;
	Output.PosToUse = mul(inPos, xWorld);
	Output.Color = inColor;

	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	

	Output.TextureCoords = texCoord;
	
	Output.worldSpaceToTangentSpace[0] = mul(normalize(tangent), xWorld);
	Output.worldSpaceToTangentSpace[1] = mul(normalize(binormal), xWorld);
	Output.worldSpaceToTangentSpace[2] = mul(normalize(inNormal), xWorld);
	Output.ViewDirection = mul(Output.worldSpaceToTangentSpace, xViewDirection - normalize(mul(inPos, xWorld)));
	return Output;    
}

/*
PixelToFrame ColoredPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
    




	float level = tex2D(HeightMapSampler, PSIn.TextureCoords).r;
	float2 displacedTex = (level * .02 - .01) * xViewDirection.xy + PSIn.TextureCoords;



			float3 normalMap = 2.0 *(tex2D(NormalMapSampler, displacedTex)) - 1.0;
			normalMap = normalize(mul(normalMap, PSIn.worldSpaceToTangentSpace));
		//normalMap = -normalMap;
		//normalMap = (0, -1, 0);
	float4 normal = float4(normalMap, 1.0);
		float3 lightDirection = normalize(xLightLoc - PSIn.PosToUse);


		float4 color = tex2D(TextureSampler, displacedTex);
		//Output.Color.rgb *= (PSIn.LightingFactor + xAmbient)*saturate(   ((-1.2)*(pow(PSIn.Color.r*4,1)/6) + 1) );
		float4 diffuse = saturate(dot(lightDirection, normal));

		float3 reflectVec = reflect(lightDirection, normal);
		float4 specular = pow(saturate(dot(normalize(reflectVec), normalize(xViewDirection))), 1) *tex2D(SpecularMapSampler, displacedTex);

		//float3 reflectionVector = -reflect(lightDirection, normal);
		//float specular = dot(normalize(reflectionVector), normalize(xViewDirection));
		//specular = pow(specular, specular*36);


		//diffuse *= 1 / pow(distance(xLightLoc, PSIn.PosToUse),2);

		Output.Color = //color * xAmbient +
			color*diffuse + color *specular;
		//Output.Color.rgb *= ((-1.2)*(pow(PSIn.Color.r * 4, 1) / 6) + 1);
	Output.Color.a = 1;

	return Output;
}*/
PixelToFrame ColoredPS(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	float level = tex2D(HeightMapSampler, PSIn.TextureCoords).r;
	float2 displacedTex = (level * .02 - .01) * xViewDirection.xy + PSIn.TextureCoords;

	// Sample the textures
	float3 normalMap = 2.0 *(tex2D(NormalMapSampler, displacedTex)) - 1.0;
	normalMap = normalize(mul(normalMap, PSIn.worldSpaceToTangentSpace));
	//normalMap = -normalMap;
	//normalMap = (0, -1, 0);
	float4 Normal = float4(normalMap, 1.0);
		float3  Specular = tex2D(SpecularMapSampler, displacedTex).rgb;
		float3  Diffuse = tex2D(TextureSampler, displacedTex).rgb;
		float2  Roughness = tex2D(GlossMapSampler, displacedTex).rg;
		//Roughness.rg = pow(Roughness.rg,8);//MAGIC HACK
	Roughness.r *= 3.0f;

	// Correct the input and compute aliases

	float3  ViewDir = -normalize(xViewDirection);
		float3  LightDir = normalize(xLightLoc - PSIn.PosToUse);
		float3  vHalf = normalize(LightDir + ViewDir);
		float  NormalDotHalf = dot(Normal, vHalf);
	float  ViewDotHalf = dot(vHalf, ViewDir);
	float  NormalDotView = dot(Normal, ViewDir);
	float  NormalDotLight = dot(Normal, LightDir);

	// Compute the geometric term
	float  G1 = (2.0f * NormalDotHalf * NormalDotView) / ViewDotHalf;
	float  G2 = (2.0f * NormalDotHalf * NormalDotLight) / ViewDotHalf;
	float  G = min(1.0f, max(0.0f, min(G1, G2)));

	// Compute the fresnel term
	float  F = Roughness.g + (1.0f - Roughness.g) * pow(1.0f - NormalDotView, 5.0f);

	// Compute the roughness term
	float  R_2 = Roughness.r * Roughness.r;
	float  NDotH_2 = NormalDotHalf * NormalDotHalf;
	float  A = 1.0f / (4.0f * R_2 * NDotH_2 * NDotH_2);
	float  B = exp(-(1.0f - NDotH_2) / (R_2 * NDotH_2));
	float  R = A * B;

	// Compute the final term
	//float3  S = Specular * ((G * F * R) / (NormalDotLight * NormalDotView));
	float3  Final = /*cLightColour.rgb **/ max(0.0f, NormalDotLight) * (Diffuse * 1 / pow(distance(xLightLoc, PSIn.PosToUse), 1));// +S);

		Output.Color = float4(Final, 1.0f);
	return Output;
}

technique Colored
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 ColoredVS();
		PixelShader  = compile ps_2_0 ColoredPS();
	}
}
