#import "UmengInterface.h"
#import <UMCommon/UMCommon.h>
#import <UMAnalytics/MobClick.h>
#import <UMAnalytics/MobClickGameAnalytics.h>
#import <UMCommonLog/UMCommonLogHeaders.h>
#import "AppConfig.h"

@implementation UmengInterface

static UmengInterface * _instance = nil;

+(instancetype) shareInstance
{
    static dispatch_once_t onceToken ;
    dispatch_once(&onceToken, ^{
        _instance = [[super allocWithZone:NULL] init] ;
    }) ;
     
    return _instance ;
}

- (void) InitUmeng
{
    NSString *appKey = [AppConfig umengId];
    NSString *channelId = [AppConfig umengChannelId];
    [UMConfigure setEncryptEnabled:YES];
    [UMCommonLogManager setUpUMCommonLogManager];
    [UMConfigure setLogEnabled:NO];
    [UMConfigure initWithAppkey:appKey channel:channelId];
    [MobClick setScenarioType:E_UM_GAME];
}

void StartLevelIOS(void *level)
{
    [MobClickGameAnalytics startLevel: [NSString stringWithUTF8String:level]];
}

void FinishLevelIOS(void *level)
{
    [MobClickGameAnalytics finishLevel: [NSString stringWithUTF8String:level]];
}

void FailLevelIOS(void *level)
{
    [MobClickGameAnalytics failLevel: [NSString stringWithUTF8String:level]];
}

void SendEventIOS(void *eventId, void *eventParams)
{
    NSString *eventid = [NSString stringWithUTF8String:eventId];
    NSString *param = [NSString stringWithUTF8String:eventParams];

    NSMutableDictionary *paramDic = [[NSMutableDictionary alloc] initWithCapacity:0];
    NSArray *paramArray = [param componentsSeparatedByString:@"|"];
    for (int i = 0; i < [paramArray count]; i++) {
        if (paramArray[i] != nil) {
            NSArray *items = [paramArray[i] componentsSeparatedByString:@"-"];
            [paramDic setObject:items[1] forKey:items[0]];
        }
    }    
    [MobClick event:eventid attributes:paramDic];
}

@end
