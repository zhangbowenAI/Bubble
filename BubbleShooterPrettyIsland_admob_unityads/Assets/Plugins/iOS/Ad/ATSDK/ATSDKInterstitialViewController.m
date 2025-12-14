#import "ATSDKInterstitialViewController.h"
#import <AnyThinkInterstitial/AnyThinkInterstitial.h>
#import "AppConfig.h"
#import "../AdManager.h"

@interface ATSDKInterstitialViewController () <ATInterstitialDelegate>
@end

@implementation ATSDKInterstitialViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
}

- (void)showInterstitialAd:(NSString *)slotID
{
    [[ATAdManager sharedManager] loadADWithPlacementID:slotID extra:nil delegate:self];
}

#pragma mark - loading delegate
-(void) didFinishLoadingADWithPlacementID:(NSString *)placementID {
    if ([[ATAdManager sharedManager] interstitialReadyForPlacementID:placementID]) {
        [[ATAdManager sharedManager] showInterstitialWithPlacementID:placementID inViewController:[UIApplication sharedApplication].keyWindow.rootViewController delegate:self];
    }
}

-(void) didFailToLoadADWithPlacementID:(NSString* )placementID error:(NSError *)error {
    
}

#pragma mark - showing delegate
-(void) interstitialDidShowForPlacementID:(NSString *)placementID extra:(NSDictionary *)extra {

}

-(void) interstitialFailedToShowForPlacementID:(NSString*)placementID error:(NSError*)error extra:(NSDictionary *)extra {

}

-(void) interstitialDidFailToPlayVideoForPlacementID:(NSString*)placementID error:(NSError*)error extra:(NSDictionary*)extra {

}

-(void) interstitialDidStartPlayingVideoForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {

}

-(void) interstitialDidEndPlayingVideoForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {

}

-(void) interstitialDidCloseForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {

}

-(void) interstitialDidClickForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {

}
@end
