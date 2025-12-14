#import "ATSDKNativeViewController.h"
#import "MTAutolayoutCategories.h"
#import <AnyThinkNative/AnyThinkNative.h>
#import "AppConfig.h"
#import "../AdManager.h"
#import "../AdMacros.h"

@interface DMADView:ATNativeADView
@property(nonatomic, readonly) UILabel *advertiserLabel;
@property(nonatomic, readonly) UILabel *textLabel;
@property(nonatomic, readonly) UILabel *titleLabel;
@property(nonatomic, readonly) UILabel *ctaLabel;
@property(nonatomic, readonly) UILabel *ratingLabel;
@property(nonatomic, readonly) UIImageView *iconImageView;
@property(nonatomic, readonly) UIImageView *mainImageView;
@property(nonatomic, readonly) UIImageView *sponsorImageView;
@end

@implementation DMADView
-(void) initSubviews {
    [super initSubviews];
    _advertiserLabel = [UILabel autolayoutLabelFont:[UIFont boldSystemFontOfSize:15.0f] textColor:[UIColor blackColor] textAlignment:NSTextAlignmentLeft];
    [self addSubview:_advertiserLabel];
    
    _titleLabel = [UILabel autolayoutLabelFont:[UIFont boldSystemFontOfSize:18.0f] textColor:[UIColor blackColor] textAlignment:NSTextAlignmentLeft];
    [self addSubview:_titleLabel];
    
    _textLabel = [UILabel autolayoutLabelFont:[UIFont systemFontOfSize:15.0f] textColor:[UIColor blackColor]];
    [self addSubview:_textLabel];
    
    _ctaLabel = [UILabel autolayoutLabelFont:[UIFont systemFontOfSize:15.0f] textColor:[UIColor blackColor]];
    [self addSubview:_ctaLabel];
    
    _ratingLabel = [UILabel autolayoutLabelFont:[UIFont systemFontOfSize:15.0f] textColor:[UIColor blackColor]];
    [self addSubview:_ratingLabel];
    
    _iconImageView = [UIImageView autolayoutView];
    _iconImageView.layer.cornerRadius = 4.0f;
    _iconImageView.layer.masksToBounds = YES;
    _iconImageView.contentMode = UIViewContentModeScaleAspectFit;
    [self addSubview:_iconImageView];
    
    _mainImageView = [UIImageView autolayoutView];
    _mainImageView.contentMode = UIViewContentModeScaleAspectFit;
    [self addSubview:_mainImageView];
    
    _sponsorImageView = [UIImageView autolayoutView];
    _sponsorImageView.contentMode = UIViewContentModeScaleAspectFit;
    [self addSubview:_sponsorImageView];
}

-(NSArray<UIView*>*)clickableViews {
    NSMutableArray<UIView*> *clickableViews = [NSMutableArray<UIView*> arrayWithObjects:_iconImageView, _ctaLabel, nil];
    if (self.mediaView != nil) { [clickableViews addObject:self.mediaView]; }
    return clickableViews;
}

-(void) layoutMediaView {
    self.mediaView.frame = CGRectMake(0, 120.0f, CGRectGetWidth(self.bounds), CGRectGetHeight(self.bounds) - 120.0f);
}

-(void) makeConstraintsForSubviews {
    [super makeConstraintsForSubviews];
    NSDictionary *viewsDict = nil;
    if (self.mediaView != nil) {
        viewsDict = @{@"titleLabel":self.titleLabel, @"textLabel":self.textLabel, @"ctaLabel":self.ctaLabel, @"ratingLabel":self.ratingLabel, @"iconImageView":self.iconImageView, @"mainImageView":self.mainImageView, @"mediaView":self.mediaView, @"advertiserLabel":self.advertiserLabel, @"sponsorImageView":self.sponsorImageView};
    } else {
        viewsDict = @{@"titleLabel":self.titleLabel, @"textLabel":self.textLabel, @"ctaLabel":self.ctaLabel, @"ratingLabel":self.ratingLabel, @"iconImageView":self.iconImageView, @"mainImageView":self.mainImageView, @"advertiserLabel":self.advertiserLabel, @"sponsorImageView":self.sponsorImageView};
    }
    [self addConstraintsWithVisualFormat:@"|[mainImageView]|" options:0 metrics:nil views:viewsDict];
    [self addConstraintsWithVisualFormat:@"V:[iconImageView]-20-[mainImageView]|" options:0 metrics:nil views:viewsDict];
    
    [self addConstraintWithItem:self.iconImageView attribute:NSLayoutAttributeWidth relatedBy:NSLayoutRelationEqual toItem:self.iconImageView attribute:NSLayoutAttributeHeight multiplier:1.0f constant:.0f];
    
    [self.titleLabel setContentCompressionResistancePriority:UILayoutPriorityDefaultLow forAxis:UILayoutConstraintAxisHorizontal];
    [self addConstraintsWithVisualFormat:@"|-15-[iconImageView(90)]-8-[titleLabel]-8-[sponsorImageView]-15-|" options:NSLayoutFormatAlignAllTop metrics:nil views:viewsDict];
    [self addConstraintsWithVisualFormat:@"V:|-15-[titleLabel]-8-[textLabel]-8-[ctaLabel]-8-[ratingLabel]-8-[advertiserLabel]" options:NSLayoutFormatAlignAllLeading | NSLayoutFormatAlignAllTrailing metrics:nil views:viewsDict];
}

-(void) makeConstraintsDrawVideoAssets {
    NSMutableDictionary<NSString*, UIView*> *viewsDict = [NSMutableDictionary<NSString*, UIView*> dictionary];
    if (self.dislikeButton != nil) { viewsDict[@"dislikeButton"] = self.dislikeButton; }
    if (self.adLabel != nil) { viewsDict[@"adLabel"] = self.adLabel; }
    if (self.logoImageView != nil) { viewsDict[@"logoImageView"] = self.logoImageView; }
    if (self.logoADImageView != nil) { viewsDict[@"logoAdImageView"] = self.logoADImageView; }
    if (self.videoAdView != nil) { viewsDict[@"videoView"] = self.videoAdView; }
    
    if ([viewsDict count] == 5) {
        self.dislikeButton.translatesAutoresizingMaskIntoConstraints = self.adLabel.translatesAutoresizingMaskIntoConstraints = self.logoImageView.translatesAutoresizingMaskIntoConstraints = self.logoADImageView.translatesAutoresizingMaskIntoConstraints = self.videoAdView.translatesAutoresizingMaskIntoConstraints = NO;
        [self addConstraintsWithVisualFormat:@"V:[logoAdImageView]-15-|" options:0 metrics:nil views:viewsDict];
        [self addConstraintsWithVisualFormat:@"|-15-[dislikeButton]-5-[adLabel]-5-[logoImageView]-5-[logoAdImageView]" options:NSLayoutFormatAlignAllCenterY metrics:nil views:viewsDict];
        [self addConstraintsWithVisualFormat:@"|[videoView]|" options:0 metrics:nil views:viewsDict];
        [self addConstraintsWithVisualFormat:@"V:[videoView(height)]|" options:0 metrics:@{@"height":@(CGRectGetHeight(self.bounds) - 120.0f)} views:viewsDict];
    }
}
@end

@interface ATSDKNativeViewController () <ATNativeADDelegate>
@property(nonatomic, readonly) NSString *placementIDs;
@property(nonatomic, readonly) DMADView *adView;
@property(nonatomic, readonly) UIImageView * adBgView;
@property(nonatomic, readonly) UIImageView * bgImageView;
@property(nonatomic, readonly) UIImageView * closeImageView;
@property(nonatomic, readonly) UIButton * adBtn;

@end
@implementation ATSDKNativeViewController

- (void)viewDidLoad
{
    [super viewDidLoad];
}

- (void)initNativeAd{
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        [[ATAdManager sharedManager] loadADWithPlacementID:[AppConfig atSDKNativeId] extra:@{kExtraInfoNativeAdSizeKey:[NSValue valueWithCGSize:CGSizeMake(CGRectGetWidth(self.view.bounds) - 40.0f, CGRectGetHeight(self.view.bounds) * 0.34 - 25)]} delegate:self];
    }
    //iPad
    else
    {
        [[ATAdManager sharedManager] loadADWithPlacementID:[AppConfig atSDKNativeId] extra:@{kExtraInfoNativeAdSizeKey:[NSValue valueWithCGSize:CGSizeMake(CGRectGetWidth(self.view.bounds) - 40.0f - 200, CGRectGetHeight(self.view.bounds) * 0.34 - 30)]} delegate:self];
    }
    _placementIDs = [AppConfig atSDKNativeId];
}

- (void)showNativeAd:(NSString *)slotID
{
    if ([[ATAdManager sharedManager] nativeAdReadyForPlacementID:slotID]) {
        [self showAD];
    } else {
        [self initNativeAd];
    }
    _placementIDs = slotID;
}

static NSInteger adViewTag = 3333;
-(void) showAD {
    [self removeAdButtonTapped];
    ATNativeADConfiguration *config = [[ATNativeADConfiguration alloc] init];
    
    if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPhone) {
        //广告View
        config.ADFrame = CGRectMake(
                                    15.0f,
                                    KIsiPhoneX ? (CGRectGetHeight(self.view.bounds) * 0.66 + 20) : CGRectGetHeight(self.view.bounds) * 0.66,
                                    CGRectGetWidth(self.view.bounds) - 30.0f,
                                    KIsiPhoneX ? (CGRectGetHeight(self.view.bounds) * 0.34 - 55) : CGRectGetHeight(self.view.bounds) * 0.34
                                    );
        config.mediaViewFrame = CGRectMake(0, 0, CGRectGetWidth(self.view.bounds), 230.0f);
        config.delegate = self;
        config.renderingViewClass = [DMADView class];
        config.rootViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
        config.context = @{kATNativeAdConfigurationContextAdOptionsViewFrameKey:[NSValue valueWithCGRect:CGRectMake(CGRectGetWidth(self.view.bounds) - 43.0f, .0f, 43.0f, 18.0f)], kATNativeAdConfigurationContextAdLogoViewFrameKey:[NSValue valueWithCGRect:CGRectMake(.0f, .0f, 54.0f, 18.0f)], kATNativeAdConfigurationContextNetworkLogoViewFrameKey:[NSValue valueWithCGRect:CGRectMake(CGRectGetWidth(config.ADFrame) - 18.0f, CGRectGetHeight(config.ADFrame) - 18.0f, 18.0f, 18.0f)]};
        _adView = (DMADView*)[[ATAdManager sharedManager] retriveAdViewWithPlacementID:_placementIDs configuration:config];
        
        _adView.tag = adViewTag;
        
        //设置白色圆角底
        _adBgView = [[UIImageView alloc] initWithFrame:CGRectMake(
                                                                  15 + 10,
                                                                  KIsiPhoneX ? (CGRectGetHeight(self.view.bounds) * 0.66) + 10: CGRectGetHeight(self.view.bounds) * 0.66 + 10,
                                                                  CGRectGetWidth(_adView.bounds) - 20,
                                                                  CGRectGetHeight(_adView.bounds) - 20)];
        _adBgView.layer.cornerRadius = 20.0f;
        _adBgView.layer.masksToBounds = YES;
        _adBgView.backgroundColor = [UIColor whiteColor];
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_adBgView];
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_adView];
        
        //外部边框
        _bgImageView = [[UIImageView alloc] initWithFrame:CGRectMake(
                                                                     15,
                                                                     KIsiPhoneX ? (CGRectGetHeight(self.view.bounds) * 0.66): CGRectGetHeight(self.view.bounds) * 0.66,
                                                                     CGRectGetWidth(_adView.bounds),
                                                                     CGRectGetHeight(_adView.bounds))];
        UIImage *bgImage = [UIImage imageNamed:@"ad_bg"];
        [_bgImageView setImage:bgImage];
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_bgImageView];
        
        //关闭按钮
        _adBtn = [UIButton buttonWithType:UIButtonTypeCustom];
        [_adBtn setFrame:CGRectMake((CGRectGetWidth(self.view.bounds) - 51 - 15), CGRectGetHeight(self.view.bounds) * 0.66, 55, 51)];
        [_adBtn setImage:[UIImage imageNamed:@"btn_close"] forState:(UIControlStateNormal)];
        [_adBtn addTarget:self action:@selector(closeNativeAd) forControlEvents:UIControlEventTouchUpInside];
        
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_adBtn];
    }
    //iPad适配
    else
    {
        //广告View
        config.ADFrame = CGRectMake(
                                    110,
                                    CGRectGetHeight(self.view.bounds) * 0.66,
                                    CGRectGetWidth(self.view.bounds) - 30.0f - 200,
                                    CGRectGetHeight(self.view.bounds) * 0.34 + 10
                                    );
        config.mediaViewFrame = CGRectMake(0, 0, CGRectGetWidth(self.view.bounds), 230.0f);
        config.delegate = self;
        config.renderingViewClass = [DMADView class];
        config.rootViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
        config.context = @{kATNativeAdConfigurationContextAdOptionsViewFrameKey:[NSValue valueWithCGRect:CGRectMake(CGRectGetWidth(self.view.bounds) - 43.0f, .0f, 43.0f, 18.0f)], kATNativeAdConfigurationContextAdLogoViewFrameKey:[NSValue valueWithCGRect:CGRectMake(.0f, .0f, 54.0f, 18.0f)], kATNativeAdConfigurationContextNetworkLogoViewFrameKey:[NSValue valueWithCGRect:CGRectMake(CGRectGetWidth(config.ADFrame) - 18.0f, CGRectGetHeight(config.ADFrame) - 18.0f, 18.0f, 18.0f)]};
        _adView = (DMADView*)[[ATAdManager sharedManager] retriveAdViewWithPlacementID:_placementIDs configuration:config];
        
        _adView.tag = adViewTag;
        //设置白色圆角底
        _adBgView = [[UIImageView alloc] initWithFrame:CGRectMake(
                                                                  110 + 10,
                                                                  CGRectGetHeight(self.view.bounds) * 0.66 + 10,
                                                                  CGRectGetWidth(_adView.bounds) - 20,
                                                                  CGRectGetHeight(_adView.bounds) + 5 - 20)];
        _adBgView.layer.cornerRadius = 20.0f;
        _adBgView.layer.masksToBounds = YES;
        _adBgView.backgroundColor = [UIColor whiteColor];
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_adBgView];
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_adView];
        
        //外部边框
        _bgImageView = [[UIImageView alloc] initWithFrame:CGRectMake(
                                                                     110,
                                                                     CGRectGetHeight(self.view.bounds) * 0.66,
                                                                     CGRectGetWidth(_adView.bounds),
                                                                     CGRectGetHeight(_adView.bounds) + 5)];
        UIImage *bgImage = [UIImage imageNamed:@"ad_bg"];
        [_bgImageView setImage:bgImage];
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_bgImageView];
        
        //关闭按钮
        _adBtn = [UIButton buttonWithType:UIButtonTypeCustom];
        [_adBtn setFrame:CGRectMake((CGRectGetWidth(self.view.bounds) - 51 - 120), CGRectGetHeight(self.view.bounds) * 0.66, 55, 51)];
        [_adBtn setImage:[UIImage imageNamed:@"btn_close"] forState:(UIControlStateNormal)];
        [_adBtn addTarget:self action:@selector(closeNativeAd) forControlEvents:UIControlEventTouchUpInside];
        
        [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:_adBtn];
    }
}

-(void) closeNativeAd {
    [self removeAdButtonTapped];
}

//关闭Native广告
-(void) removeAdButtonTapped {
    _adView.delegate = nil;
    [_adBgView removeFromSuperview];
    [_adView removeFromSuperview];
    [_bgImageView removeFromSuperview];
    //    [_closeImageView removeFromSuperview];
    [_adBtn removeFromSuperview];
    [self initNativeAd];
    
}

-(void) adBtnOnClick {
    [_adView clickableViews];
}

#pragma mark - loading delegate
-(void) didFinishLoadingADWithPlacementID:(NSString *)placementID {

}

-(void) didFailToLoadADWithPlacementID:(NSString* )placementID error:(NSError *)error {

}

//Called when user click the ad
-(void) didClickNativeAdInAdView:(ATNativeADView*)adView placementID:(NSString*)placementID extra:(NSDictionary *)extra{

}

-(void) didShowNativeAdInAdView:(ATNativeADView*)adView placementID:(NSString*)placementID extra:(NSDictionary *)extra{
    
}
@end
