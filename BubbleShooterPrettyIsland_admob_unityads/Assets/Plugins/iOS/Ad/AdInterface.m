#import "AdInterface.h"
#import "AdManager.h"

@implementation AdInterface

static AdInterface * _instance = nil;
AdManager *adManager = nil;

+(instancetype) shareInstance
{
    static dispatch_once_t onceToken ;
    dispatch_once(&onceToken, ^{
        _instance = [[super allocWithZone:NULL] init] ;
    }) ;
     
    return _instance ;
}

- (void)InitSDK
{
    adManager = [[AdManager alloc] init];
    [adManager adSdkInit];
}

void SendEvent(void *adtype, void * adspot)
{
    NSString *adType = [NSString stringWithUTF8String:adtype];
    [adManager sendEvent:adType];
}

void ShowAlertView(void *title, void *message)
{
    UIAlertView *alert = [[UIAlertView alloc] initWithTitle:[NSString stringWithUTF8String:title] message:[NSString stringWithUTF8String:message] delegate:nil cancelButtonTitle:@"确定" otherButtonTitles:nil, nil];
    [alert show];
}
@end
