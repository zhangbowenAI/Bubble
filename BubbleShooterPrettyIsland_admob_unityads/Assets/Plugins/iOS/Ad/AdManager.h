#import <Foundation/Foundation.h>

@interface AdManager : NSObject
- (void)adSdkInit;
- (void)sendEvent:(NSString *)pointType;
@end
