#import <Foundation/Foundation.h>

@interface AppConfig : NSObject
+ (NSString *)atSDKAppId;
+ (NSString *)atSDKAppKey;
+ (NSString *)atSDKBannerId;
+ (NSString *)atSDKNativeId;
+ (NSString *)atSDKRewardedVideoId;
+ (NSString *)atSDKSplashId;
+ (NSString *)atSDKInterstitialAdId;
+ (NSString *)atSDKInterstitialVideoAdId;

+ (NSString *)umengId;
+ (NSString *)umengChannelId;
@end
