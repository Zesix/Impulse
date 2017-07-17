using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnityStandardAssets.CrossPlatformInput.Inspector
{
    [InitializeOnLoad]
    public class CrossPlatformInitialize
    {
        // Custom compiler defines:
        //
        // CROSS_PLATFORM_INPUT : denotes that cross platform input package exists, so that other packages can use their CrossPlatformInput functions.
        // EDITOR_MOBILE_INPUT : denotes that mobile input should be used in editor, if a mobile build target is selected. (i.e. using Unity Remote app).
        // MOBILE_INPUT : denotes that mobile input should be used right now!

        static CrossPlatformInitialize()
        {
            var defines = GetDefinesList(BuildTargetGroups[0]);
            if (defines.Contains("CROSS_PLATFORM_INPUT")) return;
            SetEnabled("CROSS_PLATFORM_INPUT", true, false);
            SetEnabled("MOBILE_INPUT", true, true);
        }


        [MenuItem("Mobile Input/Enable")]
        private static void Enable()
        {
            SetEnabled("MOBILE_INPUT", true, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                case BuildTarget.iOS:
                case BuildTarget.PSM:
                case BuildTarget.Tizen:
                case BuildTarget.WSAPlayer:
                    EditorUtility.DisplayDialog("Mobile Input",
                                                "You have enabled Mobile Input. You'll need to use the Unity Remote app on a connected device to control your game in the Editor.",
                                                "OK");
                    break;

                case BuildTarget.StandaloneOSXUniversal:
                    break;
                case BuildTarget.StandaloneOSXIntel:
                    break;
                case BuildTarget.StandaloneWindows:
                    break;
                case BuildTarget.StandaloneLinux:
                    break;
                case BuildTarget.StandaloneWindows64:
                    break;
                case BuildTarget.WebGL:
                    break;
                case BuildTarget.StandaloneLinux64:
                    break;
                case BuildTarget.StandaloneLinuxUniversal:
                    break;
                case BuildTarget.StandaloneOSXIntel64:
                    break;
                case BuildTarget.PSP2:
                    break;
                case BuildTarget.PS4:
                    break;
                case BuildTarget.XboxOne:
                    break;
                case BuildTarget.SamsungTV:
                    break;
                case BuildTarget.N3DS:
                    break;
                case BuildTarget.WiiU:
                    break;
                case BuildTarget.tvOS:
                    break;
                case BuildTarget.Switch:
                    break;
                case BuildTarget.NoTarget:
                    break;
                default:
                    EditorUtility.DisplayDialog("Mobile Input",
                                                "You have enabled Mobile Input, but you have a non-mobile build target selected in your build settings. The mobile control rigs won't be active or visible on-screen until you switch the build target to a mobile platform.",
                                                "OK");
                    break;
            }
        }


        [MenuItem("Mobile Input/Enable", true)]
        private static bool EnableValidate()
        {
            var defines = GetDefinesList(MobileBuildTargetGroups[0]);
            return !defines.Contains("MOBILE_INPUT");
        }


        [MenuItem("Mobile Input/Disable")]
        private static void Disable()
        {
            SetEnabled("MOBILE_INPUT", false, true);
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                case BuildTarget.iOS:
                    EditorUtility.DisplayDialog("Mobile Input",
                                                "You have disabled Mobile Input. Mobile control rigs won't be visible, and the Cross Platform Input functions will always return standalone controls.",
                                                "OK");
                    break;
                case BuildTarget.StandaloneOSXUniversal:
                    break;
                case BuildTarget.StandaloneOSXIntel:
                    break;
                case BuildTarget.StandaloneWindows:
                    break;
                case BuildTarget.StandaloneLinux:
                    break;
                case BuildTarget.StandaloneWindows64:
                    break;
                case BuildTarget.WebGL:
                    break;
                case BuildTarget.WSAPlayer:
                    break;
                case BuildTarget.StandaloneLinux64:
                    break;
                case BuildTarget.StandaloneLinuxUniversal:
                    break;
                case BuildTarget.StandaloneOSXIntel64:
                    break;
                case BuildTarget.Tizen:
                    break;
                case BuildTarget.PSP2:
                    break;
                case BuildTarget.PS4:
                    break;
                case BuildTarget.PSM:
                    break;
                case BuildTarget.XboxOne:
                    break;
                case BuildTarget.SamsungTV:
                    break;
                case BuildTarget.N3DS:
                    break;
                case BuildTarget.WiiU:
                    break;
                case BuildTarget.tvOS:
                    break;
                case BuildTarget.Switch:
                    break;
                case BuildTarget.NoTarget:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        [MenuItem("Mobile Input/Disable", true)]
        private static bool DisableValidate()
        {
            var defines = GetDefinesList(MobileBuildTargetGroups[0]);
            return defines.Contains("MOBILE_INPUT");
        }


        private static readonly BuildTargetGroup[] BuildTargetGroups = new BuildTargetGroup[]
            {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WSA
            };

        private static readonly BuildTargetGroup[] MobileBuildTargetGroups = new BuildTargetGroup[]
            {
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WSA,
				BuildTargetGroup.PSM, 
				BuildTargetGroup.Tizen
            };


        private static void SetEnabled(string defineName, bool enable, bool mobile)
        {
            //Debug.Log("setting "+defineName+" to "+enable);
            foreach (var group in mobile ? MobileBuildTargetGroups : BuildTargetGroups)
            {
                var defines = GetDefinesList(group);
                if (enable)
                {
                    if (defines.Contains(defineName))
                    {
                        return;
                    }
                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                    {
                        return;
                    }
                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }
                var definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }


        private static List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
    }
}
