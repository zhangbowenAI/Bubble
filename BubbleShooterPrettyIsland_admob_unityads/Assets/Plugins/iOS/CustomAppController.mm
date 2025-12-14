#import "UnityAppController.h"
#import <AnyThinkSDK/ATAPI.h>
#import "Umeng/UmengInterface.h"
#import "Ad/AdInterface.h"

@interface CustomAppController : UnityAppController
@end

IMPL_APP_CONTROLLER_SUBCLASS (CustomAppController)

@implementation CustomAppController

- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    
    [[UmengInterface shareInstance] InitUmeng];

    [[AdInterface shareInstance] InitSDK];
    
    [ATAPI setLogEnabled:false];
    
    return YES;
}

- (void)applicationDidBecomeActive:(UIApplication*)application
{
    [super applicationDidBecomeActive:application];
}

- (void)applicationDidEnterBackground:(UIApplication*)application
{
    [super applicationDidEnterBackground:application];
}
@end
