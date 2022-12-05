#import "rofisdk.h"

// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

#ifdef __cplusplus
extern "C" {
#endif
    void UnityPause(int pause);
    extern void UnitySendMessage( const char *className, const char *methodName, const char *param );
    
#ifdef __cplusplus
}
#endif

@implementation rofisdk
char *const SDK_OBJECT_NAME = "RofiSdkHelper";

+ (rofisdk *) sharedObject {
    static rofisdk *sharedClass = nil;
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^{
        sharedClass = [[self alloc] init];
    });
    
    return sharedClass;
}

- (instancetype)init{
    if(self = [super init]){
        
    }
    return self;
}

-(void) setDebug:(BOOL)isDebug{
    NSLog(@"Set Debug %d", isDebug);
    [[RofiLoginService sharedObject] setIsDebug:isDebug];
}

-(void) setloginCallback{
    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"Info" ofType:@"plist"];
    NSDictionary *plistData = [NSDictionary dictionaryWithContentsOfFile:filePath];
    NSString* iosClientId = [plistData objectForKey:@"IosClientId"];
    
    _signInConfig = [[GIDConfiguration alloc] initWithClientID:iosClientId];
    
    [[RofiLoginService sharedObject] setBlockCallbackWhenLogedIn:^(NSString * _Nonnull data) {
        NSLog(@"~~~~~login data: %@",data);
        //send data to unity
        UnitySendMessage(SDK_OBJECT_NAME, "OnLoginInComplete", MakeStringCopy(data));
    }];
    
    UIViewController* rootViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
    
    // Do any additional setup after loading the view.
    [[RofiLoginService sharedObject] setBlockCallbackWhenClickLoginFb:^{
        FBSDKLoginManager *loginManager = [[FBSDKLoginManager alloc] init];
        [loginManager logInWithPermissions:@[@"public_profile"] fromViewController:rootViewController handler:^(FBSDKLoginManagerLoginResult *result, NSError *error) {
            if(error){
                NSLog(@"error: %@",[error localizedDescription] );
            } else if(result.isCancelled){
                NSLog(@"~~~~~Cancel");
            }else{
                if([FBSDKAccessToken currentAccessToken]){
                    NSString* accessToken = [[FBSDKAccessToken currentAccessToken] tokenString];
                    NSLog(@"fb access token: %@", accessToken);
                    
                    [[RofiLoginService sharedObject] onGetFbToken:accessToken];
                }else{
                    NSLog(@"~~~~~Erroror");
                }
            }
        }];
    }];
    
    [[RofiLoginService sharedObject] setBlockCallbackWhenClickLoginGg:^{
        [GIDSignIn.sharedInstance signInWithConfiguration:[self signInConfig]
                                 presentingViewController:rootViewController
                                                 callback:^(GIDGoogleUser * _Nullable user,
                                                            NSError * _Nullable error) {
            if (error) {
                return;
            }
            
            [user.authentication doWithFreshTokens:^(GIDAuthentication * _Nullable authentication,
                                                     NSError * _Nullable error) {
                if (error) { return; }
                if (authentication == nil) { return; }
                
                NSString *idToken = authentication.idToken;
                NSLog(@"token: %@", idToken );
                [[RofiLoginService sharedObject] onGetGgToken:idToken];
                
            }];
        }];
    }];
}

- (void) OpenLoginScene{
    NSBundle* resourcesBundle = [NSBundle bundleWithIdentifier:@"bravezone.login-module-1"];
    UIStoryboard *sb = [UIStoryboard storyboardWithName:@"AuthenStoryboard" bundle:resourcesBundle];
    
    UIViewController *vc = [sb instantiateViewControllerWithIdentifier:@"loginView"];
    vc.modalTransitionStyle = UIModalTransitionStyleCoverVertical;
    [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:vc animated:YES completion:NULL];
}

- (void) WarmUp{
    [FIRApp configure];
    [self InitAdsService];
    [self setloginCallback];
}

-(void) LogEvent:(NSString *)eventName parameters:(NSDictionary<NSString *,id> *)parameters{
    [FIRAnalytics logEventWithName:eventName parameters:parameters];
    [[AppsFlyerLib shared] logEvent:eventName withValues:parameters];
}

-(void) InitAdsService
{
    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"Info" ofType:@"plist"];
    NSDictionary *plistData = [NSDictionary dictionaryWithContentsOfFile:filePath];
    
    NSString* ironsourceKey = [plistData objectForKey:@"IronsourceKey"];
    
    NSLog(@"Init IronSource Load From Info.plist %@",ironsourceKey);
    [IronSource setRewardedVideoDelegate:self];
    
    // To init Rewarded Video
    [IronSource initWithAppKey:ironsourceKey adUnits:@[IS_REWARDED_VIDEO]];
    
    [ISIntegrationHelper validateIntegration];
}

- (BOOL) IsVideoRewardAvailable{
    return [IronSource hasRewardedVideo];
}

-(void)ShowVideoReward{
    [IronSource showRewardedVideoWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController];
}
-(void)ShowVideoReward:(NSString *)placementName{
    [IronSource showRewardedVideoWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController placement:placementName];
}

-(void)getUserInfo:(NSString *)accessToken{
    [[[RofiLoginService sharedObject] getNetworkService] getUserInfo:accessToken callback:^(int code, NSString * _Nonnull data) {
        if(code == OK_CODE){
            UnitySendMessage(SDK_OBJECT_NAME, "OnGetUserInfo", MakeStringCopy(data));
        }else{
            UnitySendMessage(SDK_OBJECT_NAME, "OnGetUserInfoFailed", MakeStringCopy(@""));
        }
    }];
}

- (void)didClickRewardedVideo:(ISPlacementInfo *)placementInfo {
    
}

- (void)didReceiveRewardForPlacement:(ISPlacementInfo *)placementInfo {
    NSDictionary *dict = @{@"placement_reward_amount": placementInfo.rewardAmount,
                           @"placement_reward_name": placementInfo.rewardName,
                           @"placement_name": placementInfo.placementName};
    
    UnitySendMessage(SDK_OBJECT_NAME, "OnVideoAdRewarded", MakeStringCopy([self getJsonFromObj:dict]));
}

- (void)rewardedVideoDidClose {
    
}

- (void)rewardedVideoDidEnd {
    
}

- (void)rewardedVideoDidFailToShowWithError:(NSError *)error {
    UnitySendMessage(SDK_OBJECT_NAME, "OnVideoAdFailed", MakeStringCopy(@""));
}

- (void)rewardedVideoDidOpen {
    
}

- (void)rewardedVideoDidStart {
    
}

- (void)rewardedVideoHasChangedAvailability:(BOOL)available {
    
}

- (NSString *)getJsonFromObj:(id)obj {
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:obj options:0 error:&error];
    if (!jsonData) {
        NSLog(@"Got an error: %@", error);
        return @"";
    } else {
        NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        return jsonString;
    }
}

- (void)checkInRefCode:(NSString *)accessToken gameId:(NSString *)gameId camId:(NSString *)camId refCode:(NSString *)refCode{
    [[[RofiLoginService sharedObject] getNetworkService] checkInRefCode:accessToken gameId:gameId camId:camId refCode:refCode callback:^(int code, NSString * _Nullable data) {
        if(code == OK_CODE){
            NSLog(@"Check in ref code OK!");
            UnitySendMessage(SDK_OBJECT_NAME, "OnRefCheckInSuccess", MakeStringCopy(@""));
            
        }else{
            NSLog(@"Check in ref code Failed!");
            UnitySendMessage(SDK_OBJECT_NAME, "OnRefCheckInFail", MakeStringCopy(data));
        }
    }];
}

-(NSString*) getCurrentAccessToken{

    return [[[RofiLoginService sharedObject] getNetworkService] getCachedAccessToken];
}

-(void) setCachedRefCode :(NSString*) refCode{
    NSLog(@"setCachedRefCode %@",refCode);
    [[RofiLoginService sharedObject] setCachedRefCode:refCode];
}

-(NSString*) getCachedRefCode{
    return [[RofiLoginService sharedObject] getCachedRefCode];
}

@end

#ifdef __cplusplus
extern "C" {
#endif
    
    void _SetRefCodeCached(char* refCode){
        [rofisdk.sharedObject setCachedRefCode:GetStringParam(refCode)];
    }
    
    char* _GetCurrentAccessToken(){
        return MakeStringCopy([[rofisdk sharedObject] getCurrentAccessToken]);
    }
    
    char* _GetRefCodeCached(){
        return MakeStringCopy([[rofisdk sharedObject] getCachedRefCode]);
    }
    
    void _SetDebugMode(bool isDebug){
        [rofisdk.sharedObject setDebug:isDebug ? TRUE : FALSE];
    }
    
    void _GetUserInfo(char* accessToken){
        [rofisdk.sharedObject getUserInfo:GetStringParam(accessToken)];
    }
    
    void _RefCheckIn(char* accessToken, char* gameId, char* camId, char* refCode){
        [rofisdk.sharedObject checkInRefCode:GetStringParam(accessToken) gameId:GetStringParam(gameId) camId:GetStringParam(camId) refCode:GetStringParam(refCode)];
    }
    
    void _OpenLoginScene(){
        [rofisdk.sharedObject OpenLoginScene];
    }
    
    void _WarmUp(){
        [rofisdk.sharedObject WarmUp];
    }
    
    bool _IsRewardAvailable(){
        return [rofisdk.sharedObject IsVideoRewardAvailable];
    }
    
    void _ShowAds(){
        [rofisdk.sharedObject ShowVideoReward];
    }
    
    void _ShowAdsWithPlacement(char* placementName){
        [rofisdk.sharedObject ShowVideoReward:GetStringParam(placementName)];
    }
    
    //demo log event
    //    void _LogEventOpenApp(){
    //        [rofisdk.sharedObject LogEvent:kFIREventAppOpen parameters:@{}];
    //    }
    //
    //    void _LogEventStartLevel(int level){
    //        [rofisdk.sharedObject LogEvent:kFIREventLevelStart parameters:@{
    //            kFIRParameterLevel:[NSNumber numberWithInt:level]
    //        }];
    //    }
    
    void _LogEvent(char* eventName,char* eventData){
        NSError*error;
        
        NSData *data = [GetStringParam(eventData) dataUsingEncoding:NSUTF8StringEncoding];
        if (!data) {
            return;
        }
        
        NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
        if (!dict) {
            return;
        }
        
        [rofisdk.sharedObject LogEvent:GetStringParam(eventName) parameters:dict];
    }
    
#ifdef __cplusplus
}
#endif
