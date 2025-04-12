[33mcommit c30938702ac3141f77db0145093dc03438787e78[m[33m ([m[1;36mHEAD[m[33m -> [m[1;32mchung-sun[m[33m, [m[1;31morigin/chung-sun[m[33m)[m
Author: Chung Sun <chungsunlee06@gmail.com>
Date:   Thu Apr 10 16:48:28 2025 +0800

    test

[1mdiff --git a/Assets/Scenes/try.unity b/Assets/Scenes/try.unity[m
[1mnew file mode 100644[m
[1mindex 0000000..d939ddd[m
[1m--- /dev/null[m
[1m+++ b/Assets/Scenes/try.unity[m
[36m@@ -0,0 +1,218 @@[m
[32m+[m[32m%YAML 1.1[m
[32m+[m[32m%TAG !u! tag:unity3d.com,2011:[m
[32m+[m[32m--- !u!29 &1[m
[32m+[m[32mOcclusionCullingSettings:[m
[32m+[m[32m  m_ObjectHideFlags: 0[m
[32m+[m[32m  serializedVersion: 2[m
[32m+[m[32m  m_OcclusionBakeSettings:[m
[32m+[m[32m    smallestOccluder: 5[m
[32m+[m[32m    smallestHole: 0.25[m
[32m+[m[32m    backfaceThreshold: 100[m
[32m+[m[32m  m_SceneGUID: 00000000000000000000000000000000[m
[32m+[m[32m  m_OcclusionCullingData: {fileID: 0}[m
[32m+[m[32m--- !u!104 &2[m
[32m+[m[32mRenderSettings:[m
[32m+[m[32m  m_ObjectHideFlags: 0[m
[32m+[m[32m  serializedVersion: 10[m
[32m+[m[32m  m_Fog: 0[m
[32m+[m[32m  m_FogColor: {r: 0.5, g: 0.5, b: 0.5, a: 1}[m
[32m+[m[32m  m_FogMode: 3[m
[32m+[m[32m  m_FogDensity: 0.01[m
[32m+[m[32m  m_LinearFogStart: 0[m
[32m+[m[32m  m_LinearFogEnd: 300[m
[32m+[m[32m  m_AmbientSkyColor: {r: 0.212, g: 0.227, b: 0.259, a: 1}[m
[32m+[m[32m  m_AmbientEquatorColor: {r: 0.114, g: 0.125, b: 0.133, a: 1}[m
[32m+[m[32m  m_AmbientGroundColor: {r: 0.047, g: 0.043, b: 0.035, a: 1}[m
[32m+[m[32m  m_AmbientIntensity: 1[m
[32m+[m[32m  m_AmbientMode: 3[m
[32m+[m[32m  m_SubtractiveShadowColor: {r: 0.42, g: 0.478, b: 0.627, a: 1}[m
[32m+[m[32m  m_SkyboxMaterial: {fileID: 0}[m
[32m+[m[32m  m_HaloStrength: 0.5[m
[32m+[m[32m  m_FlareStrength: 1[m
[32m+[m[32m  m_FlareFadeSpeed: 3[m
[32m+[m[32m  m_HaloTexture: {fileID: 0}[m
[32m+[m[32m  m_SpotCookie: {fileID: 10001, guid: 0000000000000000e000000000000000, type: 0}[m
[32m+[m[32m  m_DefaultReflectionMode: 0[m
[32m+[m[32m  m_DefaultReflectionResolution: 128[m
[32m+[m[32m  m_ReflectionBounces: 1[m
[32m+[m[32m  m_ReflectionIntensity: 1[m
[32m+[m[32m  m_CustomReflection: {fileID: 0}[m
[32m+[m[32m  m_Sun: {fileID: 0}[m
[32m+[m[32m  m_UseRadianceAmbientProbe: 0[m
[32m+[m[32m--- !u!157 &3[m
[32m+[m[32mLightmapSettings:[m
[32m+[m[32m  m_ObjectHideFlags: 0[m
[32m+[m[32m  serializedVersion: 13[m
[32m+[m[32m  m_BakeOnSceneLoad: 0[m
[32m+[m[32m  m_GISettings:[m
[32m+[m[32m    serializedVersion: 2[m
[32m+[m[32m    m_BounceScale: 1[m
[32m+[m[32m    m_IndirectOutputScale: 1[m
[32m+[m[32m    m_AlbedoBoost: 1[m
[32m+[m[32m    m_EnvironmentLightingMode: 0[m
[32m+[m[32m    m_EnableBakedLightmaps: 0[m
[32m+[m[32m    m_EnableRealtimeLightmaps: 0[m
[32m+[m[32m  m_LightmapEditorSettings:[m
[32m+[m[32m    serializedVersion: 12[m
[32m+[m[32m    m_Resolution: 2[m
[32m+[m[32m    m_BakeResolution: 40[m
[32m+[m[32m    m_AtlasSize: 1024[m
[32m+[m[32m    m_AO: 0[m
[32m+[m[32m    m_AOMaxDistance: 1[m
[32m+[m[32m    m_CompAOExponent: 1[m
[32m+[m[32m    m_CompAOExponentDirect: 0[m
[32m+[m[32m    m_ExtractAmbientOcclusion: 0[m
[32m+[m[32m    m_Padding: 2[m
[32m+[m[32m    m_LightmapParameters: {fileID: 0}[m
[32m+[m[32m    m_LightmapsBakeMode: 1[m
[32m+[m[32m    m_TextureCompression: 1[m
[32m+[m[32m    m_ReflectionCompression: 2[m
[32m+[m[32m    m_MixedBakeMode: 2[m
[32m+[m[32m    m_BakeBackend: 1[m
[32m+[m[32m    m_PVRSampling: 1[m
[32m+[m[32m    m_PVRDirectSampleCount: 32[m
[32m+[m[32m    m_PVRSampleCount: 512[m
[32m+[m[32m    m_PVRBounces: 2[m
[32m+[m[32m    m_PVREnvironmentSampleCount: 256[m
[32m+[m[32m    m_PVREnvironmentReferencePointCount: 2048[m
[32m+[m[32m    m_PVRFilteringMode: 1[m
[32m+[m[32m    m_PVRDenoiserTypeDirect: 1[m
[32m+[m[32m    m_PVRDenoiserTypeIndirect: 1[m
[32m+[m[32m    m_PVRDenoiserTypeAO: 1[m
[32m+[m[32m    m_PVRFilterTypeDirect: 0[m
[32m+[m[32m    m_PVRFilterTypeIndirect: 0[m
[32m+[m[32m    m_PVRFilterTypeAO: 0[m
[32m+[m[32m    m_PVREnvironmentMIS: 1[m
[32m+[m[32m    m_PVRCulling: 1[m
[32m+[m[32m    m_PVRFilteringGaussRadiusDirect: 1[m
[32m+[m[32m    m_PVRFilteringGaussRadiusIndirect: 1[m
[32m+[m[32m    m_PVRFilteringGaussRadiusAO: 1[m
[32m+[m[32m    m_PVRFilteringAtrousPositionSigmaDirect: 0.5[m
[32m+[m[32m    m_PVRFilteringAtrousPositionSigmaIndirect: 2[m
[32m+[m[32m    m_PVRFilteringAtrousPositionSigmaAO: 1[m
[32m+[m[32m    m_ExportTrainingData: 0[m
[32m+[m[32m    m_TrainingDataDestination: TrainingData[m
[32m+[m[32m    m_LightProbeSampleCountMultiplier: 4[m
[32m+[m[32m  m_LightingDataAsset: {fileID: 20201, guid: 0000000000000000f000000000000000, type: 0}[m
[32m+[m[32m  m_LightingSettings: {fileID: 0}[m
[32m+[m[32m--- !u!196 &4[m
[32m+[m[32mNavMeshSettings:[m
[32m+[m[32m  serializedVersion: 2[m
[32m+[m[32m  m_ObjectHideFlags: 0[m
[32m+[m[32m  m_BuildSettings:[m
[32m+[m[32m    serializedVersion: 3[m
[32m+[m[32m    agentTypeID: 0[m
[32m+[m[32m    agentRadius: 0.5[m
[32m+[m[32m    agentHeight: 2[m
[32m+[m[32m    agentSlope: 45[m
[32m+[m[32m    agentClimb: 0.4[m
[32m+[m[32m    ledgeDropHeight: 0[m
[32m+[m[32m    maxJumpAcrossDistance: 0[m
[32m+[m[32m    minRegionArea: 2[m
[32m+[m[32m    manualCellSize: 0[m
[32m+[m[32m    cellSize: 0.16666667[m
[32m+[m[32m    manualTileSize: 0[m
[32m+[m[32m    tileSize: 256[m
[32m+[m[32m    buildHeightMesh: 0[m
[32m+[m[32m    maxJobWorkers: 0[m
[32m+[m[32m    preserveTilesOutsideBounds: 0[m
[32m+[m[32m    debug:[m
[32m+[m[32m      m_Flags: 0[m
[32m+[m[32m  m_NavMeshData: {fileID: 0}[m
[32m+[m[32m--- !u!1 &912454028[m
[32m+[m[32mGameObject:[m
[32m+[m[32m  m_ObjectHideFlags: 0[m
[32m+[m[32m  m_CorrespondingSourceObject: {fileID: 0}[m
[32m+[m[32m  m_PrefabInstance: {fileID: 0}[m
[32m+[m[32m  m_PrefabAsset: {fileID: 0}[m
[32m+[m[32m  serializedVersion: 6[m
[32m+[m[32m  m_Component:[m
[32m+[m[32m  - component: {fileID: 912454031}[m
[32m+[m[32m  - component: {fileID: 912454030}[m
[32m+[m[32m  - component: {fileID: 912454029}[m
[32m+[m[32m  m_Layer: 0[m
[32m+[m[32m  m_Name: Main Camera[m
[32m+[m[32m  m_TagString: MainCamera[m
[32m+[m[32m  m_Icon: {fileID: 0}[m
[32m+[m[32m  m_NavMeshLayer: 0[m
[32m+[m[32m  m_StaticEditorFlags: 0[m
[32m+[m[32m  m_IsActive: 1[m
[32m+[m[32m--- !u!81 &912454029[m
[32m+[m[32mAudioListener:[m
[32m+[m[32m  m_ObjectHideFlags: 0[m
[32m+[m[32m  m_CorrespondingSourceObject: {fileID: 0}[m
[32m+[m[32m  m_PrefabInstance: {fileID: 0}[m
[32m+[m[32m  m_PrefabAsset: {fileID: 0}[m
[32m+[m[32m  m_GameObject: {fileID: 912454028}[m
[32m+[m[32m  m_Enabled: 1[m
[32m+[m[32m--- !u!20 &912454030[m
[32m+[m[32mCamera:[m
[32m+[m[32m  m_ObjectHideFlags: 0[m
[32m+[m[32m  m_CorrespondingSourceObject: {fileID: 0}[m
[32m+[m[32m  m_PrefabInstance: {fileID: 0}[m
[32m+[m[32m  m_PrefabAsset: {fileID: 0}[m
[32m+[m[32m  m_GameObject: {fileID: 912454028}[m
[32m+[m[32m  m_Enabled: 1[m
[32m+[m[32m  serializedVersion: 2[m
[32m+[m[32m  m_ClearFlags: 1[m
[32m+[m[32m  m_BackGroundColor: {r: 0.19215687, g: 0.3019608, b: 0.4745098, a: 0}[m
[32m+[m[32m  m_projectionMatrixMode: 1[m
[32m+[m[32m  m_GateFitMode: 2[m
[32m+[m[32m  m_FOVAxisMode: 0[m
[32m+[m[32m  m_Iso: 200[m
[32m+[m[32m  m_ShutterSpeed: 0.005[m
[32m+[m[32m  m_Aperture: 16[m
[32m+[m[32m  m_FocusDistance: 10[m
[32m+[m[32m  m_FocalLength: 50[m
[32m+[m[32m  m_BladeCount: 5[m
[32m+[m[32m  m_Curvature: {x: 2, y: 11}[m
[32m+[m[32m  m_BarrelClipping: 0.25[m
[32m+[m[32m  m_Anamorphism: 0[m
[32m+[m[32m  m_SensorSize: {x: 36, y: 24}[m
[32m+[m[32m  m_LensShift: {x: 0, y: 0}[m
[32m+[m[32m  m_NormalizedViewPortRect:[m
[32m+[m[32m    serializedVersion: 2[m
[32m+[m[32m    x: 0[m
[32