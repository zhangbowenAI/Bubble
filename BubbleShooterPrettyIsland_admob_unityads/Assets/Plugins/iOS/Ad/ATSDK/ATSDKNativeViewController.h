#import <UIKit/UIKit.h>

@interface ATSDKNativeViewController : UIViewController
- (void)initNativeAd;
- (void)showNativeAd:(NSString *)slotID;
- (void)closeNativeAd;
@end
