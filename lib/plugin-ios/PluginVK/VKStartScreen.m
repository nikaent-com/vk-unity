#import "VKStartScreen.h"
static NSArray *SCOPE = nil;

@interface VKStartScreen () <UIAlertViewDelegate, VKSdkUIDelegate>

@end

@implementation VKStartScreen

- (void)viewDidLoad {
    [super viewDidLoad];
    [self initReg];
}
- (void)start:(NSString*) appVkId scope:(NSArray *) scope {
    SCOPE = @[VK_PER_FRIENDS, VK_PER_WALL, VK_PER_AUDIO, VK_PER_PHOTOS, VK_PER_NOHTTPS, VK_PER_EMAIL, VK_PER_MESSAGES];
    id delegate = [[UIApplication sharedApplication] delegate];
    [[VKSdk initializeWithAppId:appVkId] registerDelegate:self];
    [[VKSdk instance] setUiDelegate:self];
    [VKSdk wakeUpSession:SCOPE completeBlock:^(VKAuthorizationState state, NSError *error) {
        if (state == VKAuthorizationAuthorized) {
            NSLog(@"VKAuthorizationAuthorized");
            [self startWorking];
        } else if (error) {
            [[[UIAlertView alloc] initWithTitle:nil message:[error description] delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil] show];
        }
    }];
}

- (void)startWorking {
    NSLog(@"startWorking");
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
}

-(void) auth:(NSArray *) scope{
    [VKSdk authorize:scope];
}

- (void)testCaptcha {
    VKRequest *request = [[VKApiCaptcha new] force];
    [request executeWithResultBlock:^(VKResponse *response) {
        NSLog(@"Result: %@", response);
    }                    errorBlock:^(NSError *error) {
        NSLog(@"Error: %@", error);
    }];
}


- (void)vkSdkNeedCaptchaEnter:(VKError *)captchaError {
    VKCaptchaViewController *vc = [VKCaptchaViewController captchaControllerWithError:captchaError];
    [vc presentIn:self];
}

- (void)vkSdkTokenHasExpired:(VKAccessToken *)expiredToken {
    [VKSdk authorize:nil];
}

- (void)vkSdkAccessAuthorizationFinishedWithResult:(VKAuthorizationResult *)result {
    NSLog(@"vkSdkAccessAuthorizationFinishedWithResult\n");
    if (result.token) {
        [self startWorking];
    } else if (result.error) {
        NSLog([NSString stringWithFormat:@"Access denied\n%@", result.error]);
    }
}

- (void)vkSdkUserAuthorizationFailed {
    NSLog(@"Access denied\n");
    [self.navigationController popToRootViewControllerAnimated:YES];
}

- (void)vkSdkShouldPresentViewController:(UIViewController *)controller {
    NSLog(@"vkSdkShouldPresentViewController\n");
    
    [self presentViewController:controller animated:YES completion:nil];
}

-(void) initReg
{
    NSLog(@"AsPush :: registering app for remote notifications.");
    
    //Code below found on stack overflow. Fantastic find.
    
    id delegate = [[UIApplication sharedApplication] delegate];
    
    Class objectClass = object_getClass(delegate);
    
    NSString *newClassName = [NSString stringWithFormat:@"Custom_%@", NSStringFromClass(objectClass)];
    Class modDelegate = NSClassFromString(newClassName);
    if (modDelegate == nil)
    {
        // this class doesn't exist; create it
        // allocate a new class
        modDelegate = objc_allocateClassPair(objectClass, [newClassName UTF8String], 0);
        
        SEL selectorToOverride1 = @selector(application:openURL:sourceApplication:annotation:);
        SEL selectorToOverride2 = @selector(application:didFinishLaunchingWithOptions:);
        
        Method m1 = class_getInstanceMethod([VKStartScreen class], selectorToOverride1);
        Method m2 = class_getInstanceMethod([VKStartScreen class], selectorToOverride2);
        
        IMP theImplementation1 = [self methodForSelector:selectorToOverride1];
        IMP theImplementation2 = [self methodForSelector:selectorToOverride2];
        
        class_addMethod(modDelegate, selectorToOverride1, theImplementation1, method_getTypeEncoding(m1));
        class_addMethod(modDelegate, selectorToOverride2, theImplementation2, method_getTypeEncoding(m2));
        // register the new class with the runtime
        objc_registerClassPair(modDelegate);
    }
    // change the class of the object
    object_setClass(delegate, modDelegate);
    
    //Register this app for remote notifications
    [[UIApplication sharedApplication] registerForRemoteNotificationTypes: UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeSound | UIRemoteNotificationTypeBadge];
}


- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    return YES;
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation {
    [VKSdk processOpenURL:url fromApplication:sourceApplication];
    
    return YES;
}

@end
