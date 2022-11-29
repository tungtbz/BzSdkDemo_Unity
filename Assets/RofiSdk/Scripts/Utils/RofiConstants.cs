using System;
namespace RofiSdk
{
    public class RofiConstants
    {
        public static String ANDROID_CODE_PACKAGE = "com.rofi.lib";
        public static String ANDROID_NATIVE_BRIDGE = ".AndroidBridge";

        //this name for call back from native
        public static String SDK_OBJECT_NAME = "RofiSdkHelper";

        public static String OPEN_LOGIN_SCENE_FUNC_NAME = "OpenLoginScene";
        public static String LOG_EVENT_FUNC_NAME = "LogEventOpenApp";

        public static String ANDROID_BRIDGE_CLASS = ANDROID_CODE_PACKAGE + ANDROID_NATIVE_BRIDGE;
        
    }
}

