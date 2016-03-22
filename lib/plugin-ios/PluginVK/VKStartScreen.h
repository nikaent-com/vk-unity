#import <UIKit/UIKit.h>
#import <VKSdk/VKSdk.h>

@interface VKStartScreen : UIViewController <VKSdkDelegate>{
@private
VKRequest *callingRequest;
}

- (void) auth:(NSArray *) scope;
- (void) start:(NSString*) appVkId scope:(NSArray *) scope;
- (void) testCaptcha;

@end

