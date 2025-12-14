#import "ATSDKSplashViewController.h"
#import <AnyThinkSplash/AnyThinkSplash.h>

@interface ATSDKSplashViewController ()<ATSplashDelegate>

@end

@implementation ATSDKSplashViewController

- (void)showSplashAd:(NSString *)slotID
{
    UIWindow *mainWindow = nil;
    if ( @available(iOS 13.0, *) ) {
       mainWindow = [UIApplication sharedApplication].windows.firstObject;
       [mainWindow makeKeyWindow];
    } else {
        mainWindow = [UIApplication sharedApplication].keyWindow;
    }
    [[ATAdManager sharedManager] loadADWithPlacementID:slotID extra:@{kATSplashExtraTolerateTimeoutKey:@5.5} customData:nil delegate:self window:mainWindow containerView:nil];
}

#pragma mark - AT Splash Delegate method(s)
-(void) didFinishLoadingADWithPlacementID:(NSString *)placementID {
    
}

-(void) didFailToLoadADWithPlacementID:(NSString*)placementID error:(NSError*)error {

}

-(void)splashDidShowForPlacementID:(NSString*)placementID extra:(NSDictionary*)extra {

}

-(void)splashDidClickForPlacementID:(NSString*)placementID extra:(NSDictionary*)extra {

}

-(void)splashDidCloseForPlacementID:(NSString*)placementID extra:(NSDictionary*)extra {

}

#pragma mark - splash delegate with networkID and adsourceID
-(void)didShowNativeSplashAdForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra{

}

-(void)didClickNaitveSplashAdForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra{

}

-(void)didCloseNativeSplashAdForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra{

}
@end
