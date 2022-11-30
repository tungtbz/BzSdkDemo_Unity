#import <Foundation/Foundation.h>
#import "IronSource/IronSource.h"
#import <AppsFlyerLib/AppsFlyerLib.h>
#import <GoogleSignIn/GoogleSignIn.h>
#import <FBSDKCoreKit.h>
#import <FBSDKLoginKit.h>

#import <login_module_1.xcframework/ios-arm64/login_module_1.framework/Headers/RofiLoginService.h>

#import <FirebaseCore/FirebaseCore.h>
#import <FirebaseAnalytics/FirebaseAnalytics.h>

@interface rofisdk : NSObject <ISRewardedVideoDelegate>
@property (strong,nonatomic) GIDConfiguration* _Nonnull signInConfig;

- (void)WarmUp;
- (void)LogEvent: (NSString*) eventName parameters:(nullable NSDictionary<NSString *, id> *)parameters;
- (void)InitAdsService;
- (BOOL)IsVideoRewardAvailable;
- (void)ShowVideoReward;
- (void)ShowVideoReward:(NSString*) placement;
- (void)OpenLoginScene;
- (void)setDebug:(BOOL) isDebug;
- (void)getUserInfo:(NSString*) accessToken;
- (NSString *)getJsonFromObj:(id)obj;

+ (rofisdk *) sharedObject;

@end
