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
@property (nonatomic) BOOL isWarmUped;

- (void)warmUp;
- (void)LogEvent: (NSString* _Nonnull) eventName parameters:(nullable NSDictionary<NSString *, id> *)parameters;
- (void)InitAdsService;
- (BOOL)IsVideoRewardAvailable;
- (void)ShowVideoReward;
- (void)ShowVideoReward:(NSString* _Nullable) placement;
- (void)OpenLoginScene;
- (void)setDebug:(BOOL) isDebug;
- (void)getUserInfo:(NSString* _Nonnull) accessToken;

- (void)checkInRefCode:(NSString* _Nonnull)accessToken
               refCode:(NSString* _Nonnull)refCode;

-(void)joinCampaign:(NSString* _Nonnull) accessToken;
                    
-(NSString *_Nonnull)getJsonFromObj:(id _Nonnull)obj;

-(NSString *_Nonnull)getCurrentAccessToken;
-(void) setCachedRefCode :(NSString*) refCode;

+(rofisdk *_Nonnull) sharedObject;

@end
