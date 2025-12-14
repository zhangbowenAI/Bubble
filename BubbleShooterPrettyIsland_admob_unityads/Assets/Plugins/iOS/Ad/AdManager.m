#import "AdManager.h"
#import "ATSDK/ATSDKController.h"
#import <UMAnalytics/MobClick.h>

@interface AdManager ()

@property (nonatomic) NSDictionary *adTypeDictData;
@end

@implementation AdManager

ATSDKController *atSdkController = nil;

- (void)adSdkInit
{
    atSdkController = [[ATSDKController alloc] init];
    [atSdkController atsdkinit];    
}

- (void)sendEvent:(NSString *) adType
{
    if ( adType != nil )
    {
        if ([adType isEqualToString:@"RewardVideoAD"])
        {
            [self showAdwithType:@"showRewardedVideoAd"];
        }
        else if ([adType isEqualToString:@"CPAD"])
        {
            [self showAdwithType:@"showInterstitialAd"];
        }
        else if ([adType isEqualToString:@"SceneVideoAD"])
        {
            [self showAdwithType:@"showFullScreenVideoAd"];
        }
    }
}

- (void)showAdwithType:(NSString *)adType
{
    [atSdkController showAdwithtype:adType];
}

- (void)rewardVideoAdIsFinish
{
    UnitySendMessage("ADInterface", "FinishAdvertisement", "");
}

- (void)rewardVideoAdIsFailed
{
    UnitySendMessage("ADInterface", "FailAdvertisement", "");
}
@end
