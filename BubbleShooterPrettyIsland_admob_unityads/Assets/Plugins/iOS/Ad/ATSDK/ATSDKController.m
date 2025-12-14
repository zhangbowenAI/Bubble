#import "UnityAppController.h"
#import "ATSDKController.h"
#import <AnyThinkSDK/ATAPI.h>
#import <AnyThinkSDK/ATAdManager.h>
#import "../AdMacros.h"
#import "../../AppConfig.h"

//开屏广告
#import "ATSDKSplashViewController.h"

//激励视频广告
#import "ATSDKRewardedViewController.h"

//原生广告
#import "ATSDKNativeViewController.h"

//插屏广告
#import "ATSDKInterstitialViewController.h"

ATSDKSplashViewController *atSDKSplashViewController = nil;
ATSDKRewardedViewController *atSDKRewardedViewController = nil;
ATSDKNativeViewController *atSDKNativeViewController = nil;
ATSDKInterstitialViewController *atSDKInterstitialViewController = nil;

@implementation ATSDKController

- (void)atsdkinit{
    NSLog(@"ATSDKController - atsdkinit");
    [ATAPI setLogEnabled:NO];//Turn on debug logs
    [[ATAPI sharedInstance] startWithAppID:[AppConfig atSDKAppId] appKey:[AppConfig atSDKAppKey] error:nil];
    
    atSDKSplashViewController = [[ATSDKSplashViewController alloc] init];
    [atSDKSplashViewController showSplashAd:[AppConfig atSDKSplashId]];
    
    atSDKRewardedViewController = [[ATSDKRewardedViewController alloc] init];
    atSDKNativeViewController = [[ATSDKNativeViewController alloc] init];
    atSDKInterstitialViewController = [[ATSDKInterstitialViewController alloc] init];

    [atSDKRewardedViewController initRewardedVideoAd];
//    [atSDKNativeViewController initNativeAd];
    
}

- (void)showAdwithtype:(NSString *)adType
{
    if  ([adType isEqualToString:@"showRewardedVideoAd"])
    {
        [atSDKNativeViewController closeNativeAd];
        [atSDKRewardedViewController showRewardedVideoAd:[AppConfig atSDKRewardedVideoId]];
    }
    else if ([adType isEqualToString:@"showInterstitialAd"])
    {
        [atSDKInterstitialViewController showInterstitialAd:[AppConfig atSDKInterstitialAdId]];
    }
    else if ([adType isEqualToString:@"showFullScreenVideoAd"])
    {
        [atSDKInterstitialViewController showInterstitialAd:[AppConfig atSDKInterstitialVideoAdId]];
    }
    else if ([adType isEqualToString:@"showNativeAd"])
    {
        [atSDKNativeViewController showNativeAd:[AppConfig atSDKNativeId]];
    }
    else if (([adType isEqualToString:@"closeNativeAd"]))
    {
        [atSDKNativeViewController closeNativeAd];
    }
}

- (void)closeNativeBannerAd
{
}

- (void)closeBannerAd
{
}
@end
