#import "ATSDKRewardedViewController.h"
#import <AnyThinkRewardedVideo/AnyThinkRewardedVideo.h>
#import "AppConfig.h"
#import "../AdManager.h"

@interface ATSDKRewardedViewController () <ATRewardedVideoDelegate>
{
    @private int adReloadTime;
}
@property (nonatomic) BOOL isFinishVideoAd;
@end

@implementation ATSDKRewardedViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
}

- (void)initRewardedVideoAd
{
    [self loadRewardedVideoAd];
    
    adReloadTime = 0;
}

- (void) loadRewardedVideoAd
{
    [[ATAdManager sharedManager] loadADWithPlacementID:[AppConfig atSDKRewardedVideoId]
                                                 extra:@{kATAdLoadingExtraUserIDKey:[[[UIDevice currentDevice] identifierForVendor] UUIDString] != nil ? [[[UIDevice currentDevice] identifierForVendor] UUIDString] : @""}
                                              delegate:self];
}

- (void)showRewardedVideoAd:(NSString *)slotID
{
    _isFinishVideoAd = false;
    if ([[ATAdManager sharedManager] rewardedVideoReadyForPlacementID:slotID]) {
        [[ATAdManager sharedManager] showRewardedVideoWithPlacementID:slotID inViewController:[UIApplication sharedApplication].keyWindow.rootViewController delegate:self];
        
        adReloadTime = 0;
    } else {
        if (adReloadTime < 6){
            dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
                adReloadTime = adReloadTime + 1;
                [self loadRewardedVideoAd];
                
                [NSThread sleepForTimeInterval:3.0f];
                dispatch_async(dispatch_get_main_queue(), ^{
                    [self showRewardedVideoAd:slotID];
                });
            });
        } else {
            UnitySendMessage("AdvertisementControl", "FailAdvertisement", "");
            
            adReloadTime = 0;
        }
    }
}

#pragma mark - loading delegate
-(void) didFinishLoadingADWithPlacementID:(NSString *)placementID {
 
}

-(void) didFailToLoadADWithPlacementID:(NSString* )placementID error:(NSError *)error {

}

#pragma mark - showing delegate
-(void) rewardedVideoDidRewardSuccessForPlacemenID:(NSString *)placementID extra:(NSDictionary *)extra{
    _isFinishVideoAd = true;
}

-(void) rewardedVideoDidStartPlayingForPlacementID:(NSString *)placementID extra:(NSDictionary *)extra {

}

-(void) rewardedVideoDidEndPlayingForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {

}

-(void) rewardedVideoDidFailToPlayForPlacementID:(NSString*)placementID error:(NSError*)error extra:(NSDictionary *)extra {

}

-(void) rewardedVideoDidCloseForPlacementID:(NSString*)placementID rewarded:(BOOL)rewarded extra:(NSDictionary *)extra {
    
    if (_isFinishVideoAd)
    {
        UnitySendMessage("ADInterface", "FinishAdvertisement", "");
    }
    else
    {
        UnitySendMessage("ADInterface", "FailAdvertisement", "");
    }
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
        [NSThread sleepForTimeInterval:2.0f];
        dispatch_async(dispatch_get_main_queue(), ^{
                [self loadRewardedVideoAd];
        });
    });

}

-(void) rewardedVideoDidClickForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {

}
@end
