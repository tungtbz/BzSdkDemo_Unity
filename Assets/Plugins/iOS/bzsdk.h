//
//  bzsdk.h
//  bzsdk
//
//  Created by HeroFi-AccountGP on 02/11/2022.
//

#import <Foundation/Foundation.h>
#import "IronSource/IronSource.h"
#import <AppsFlyerLib/AppsFlyerLib.h>

@import FirebaseAnalytics;
@import FirebaseCore;

@interface bzsdk : NSObject <ISRewardedVideoDelegate>
- (void)WarmUp;
- (void)InitFireBase;
- (void)LogEvent: (NSString*) eventName parameters:(nullable NSDictionary<NSString *, id> *)parameters;
- (void)InitAdsService;
- (BOOL)IsVideoRewardAvailable;
- (void)ShowVideoReward;
- (void)ShowVideoReward:(NSString*) placement;
- (NSString *)getJsonFromObj:(id)obj;

+ (bzsdk *) sharedObject;

@end
