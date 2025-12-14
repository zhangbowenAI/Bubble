#ifndef AdMacros_h
#define AdMacros_h

#define mainColor BUD_RGB(0xff, 0x63, 0x5c)
#define titleBGColor BUD_RGB(73, 15, 15)
#define BUD_RGB(a,b,c) [UIColor colorWithRed:(a/255.0) green:(b/255.0) blue:(c/255.0) alpha:1]

#ifndef SCREEN_WIDTH
#define SCREEN_WIDTH ((([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortrait) || ([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortraitUpsideDown)) ? [[UIScreen mainScreen] bounds].size.width : [[UIScreen mainScreen] bounds].size.height)
#endif

#ifndef SCREEN_HEIGHT
#define SCREEN_HEIGHT ((([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortrait) || ([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationPortraitUpsideDown)) ? [[UIScreen mainScreen] bounds].size.height : [[UIScreen mainScreen] bounds].size.width)
#endif

#define iPhoneX [[UIScreen mainScreen] bounds].size.width == 375.0f && [[UIScreen mainScreen] bounds].size.height == 812.0f
#define iPhoneXR [[UIScreen mainScreen] bounds].size.width == 414.0f && [[UIScreen mainScreen] bounds].size.height == 896.0f

#define KIsiPhoneX (iPhoneX || iPhoneXR)

#define kStateBarHeight (isFullScreen ? 44.0 : 20.0)
#define kNavigationBarHeight (kStateBarHeight + 44.0)
#define kTabBarHeight (isFullScreen ? (49.0+34.0) : 49.0)

#endif
