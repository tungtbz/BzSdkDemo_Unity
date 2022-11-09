//
//  NSObject+bzsdk.m
//  bzsdk
//
//  Created by HeroFi-AccountGP on 02/11/2022.
//

#import "bzsdk.h"

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

@implementation bzsdk
char *const BZSDK_EVENTS = "BzSDKEvent";

+ (bzsdk *) sharedObject {
    static bzsdk *sharedClass = nil;
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

- (void) WarmUp{
    [FIRApp configure];
    [self InitAdsService];
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

- (void)didClickRewardedVideo:(ISPlacementInfo *)placementInfo {
    
}

- (void)didReceiveRewardForPlacement:(ISPlacementInfo *)placementInfo {
    NSDictionary *dict = @{@"placement_reward_amount": placementInfo.rewardAmount,
                           @"placement_reward_name": placementInfo.rewardName,
                           @"placement_name": placementInfo.placementName};
    
    UnitySendMessage(BZSDK_EVENTS, "onRewardedVideoAdRewarded", MakeStringCopy([self getJsonFromObj:dict]));
}

- (void)rewardedVideoDidClose {
    
}

- (void)rewardedVideoDidEnd {
    
}

- (void)rewardedVideoDidFailToShowWithError:(NSError *)error {
    
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

@end

#ifdef __cplusplus
extern "C" {
#endif
    void _WarmUp(){
        [bzsdk.sharedObject WarmUp];
    }

    bool _IsRewardAvailable(){
        return [bzsdk.sharedObject IsVideoRewardAvailable];
    }

    void _ShowAds(){
        [bzsdk.sharedObject ShowVideoReward];
    }

    void _ShowAdsWithPlacement(char* placementName){
        [bzsdk.sharedObject ShowVideoReward:GetStringParam(placementName)];
    }
    
    //demo log event
//    void _LogEventOpenApp(){
//        [bzsdk.sharedObject LogEvent:kFIREventAppOpen parameters:@{}];
//    }
//
//    void _LogEventStartLevel(int level){
//        [bzsdk.sharedObject LogEvent:kFIREventLevelStart parameters:@{
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
        
        [bzsdk.sharedObject LogEvent:GetStringParam(eventName) parameters:dict];
    }
    
#ifdef __cplusplus
}
#endif
