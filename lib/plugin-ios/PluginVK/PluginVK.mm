//
//  ane.m
//  ane
//
//  Created by Aleksey Kabanov on 15.02.16.
//  Copyright © 2016 Aleksey Kabanov. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <VKSdk/VKSdk.h>
#import "VKStartScreen.h"
#import <Foundation/Foundation.h>


extern "C" {
    extern void UnitySendMessage(const char *, const char *, const char *);
}

VKStartScreen *vkscreen;

NSString *appVkId;

uint32_t _reqCounter = 0;

void traceToAne(NSString *str){
    UnitySendMessage((char *)"VkPlugin", (char *)"trace", [str UTF8String]);
}
void apiBack(NSString *str){
    NSLog(str);
    UnitySendMessage((char *)"VkPlugin", (char *)"call", [str UTF8String]);
}


extern "C" void _init(const char * idVkApp)
{
    traceToAne(@"init start");
    NSDictionary *bundleInfo = [[NSBundle mainBundle] infoDictionary];
    
    appVkId = [NSString stringWithUTF8String: idVkApp];
    
    NSString *appName = [bundleInfo objectForKey:@"CFBundleIdentifier"];
    
    vkscreen = [[VKStartScreen alloc] init];
    UIScreen *sc = [UIScreen mainScreen];
    CGRect rect0 = sc.bounds;
    
    CGRect rect = CGRectMake(0, 0, rect0.size.width, rect0.size.height);
    vkscreen.view.frame = rect;
    
    id delegate = [[UIApplication sharedApplication] delegate];
    UIWindow * win = [delegate window];
    [win addSubview:vkscreen.view];
    
    [vkscreen.view setUserInteractionEnabled:false];
    
    [vkscreen start:appVkId scope:nil];
    
    traceToAne([NSString stringWithFormat:@"AppName: %@",appName]);
    traceToAne([NSString stringWithFormat:@"AppVkId: %@",appVkId]);
}

extern "C" void _login(const char * scopes)
{
    traceToAne(@"begin login");
    
    NSString *strScope = [NSString stringWithUTF8String: scopes];
    traceToAne([NSString stringWithFormat:@"strScope: %@",strScope]);
    NSData *data = [strScope dataUsingEncoding:NSUTF8StringEncoding];
    traceToAne([NSString stringWithFormat:@"data: %@",data]);
    
    NSArray *arrayScope = [NSJSONSerialization JSONObjectWithData:data
                                                          options:0
                                                            error:nil];
    traceToAne([NSString stringWithFormat:@"arrayScope: %lu",(unsigned long)arrayScope.count]);
    traceToAne([NSString stringWithFormat:@"arrayScope: %@",arrayScope]);
    [vkscreen auth:arrayScope];
    traceToAne(@"end login");
}

extern "C" bool _isLoggedIn()
{
    traceToAne(@"isLoggedIn");
    
    return [VKSdk isLoggedIn];
}

extern "C" void _logout()
{
    traceToAne(@"logout");
    
    [VKSdk forceLogout];
}

extern "C" void _testCaptcha()
{
    traceToAne(@"testCaptcha");
    
    [vkscreen testCaptcha];
}

NSString* getString(const char *str){
    return [NSString stringWithUTF8String: (char*) str];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C" char * _apiCall(const char *methodChar, const char *paramsChar)
{
    NSString *method = getString(methodChar);
    NSString *params = getString(paramsChar);
    traceToAne([NSString stringWithFormat:@"apiCall %@",params]);

    NSData *data = [params dataUsingEncoding:NSUTF8StringEncoding];
    
    NSDictionary *paramsDic = [NSJSONSerialization JSONObjectWithData:data
                                                          options:0
                                                            error:nil];
    
    VKRequest *request = [VKApi requestWithMethod:method andParameters:paramsDic];

    uint32_t idReq = ++_reqCounter;
    NSString *idString = [NSString stringWithFormat:@"%d",idReq];
    const char *returnIdRequest = [idString UTF8String];
    
    [request executeWithResultBlock:^(VKResponse *response) {
        NSString *code = [NSString stringWithFormat:@"response%d",idReq];
        apiBack([NSString stringWithFormat:@"{\"%@\":%@}",code, [response responseString]]);

        traceToAne([NSString stringWithFormat:@"Result: %@", response]);
    }                    errorBlock:^(NSError *error) {
        NSString *code = [NSString stringWithFormat:@"responseError%d",idReq];
        NSString *errorJson = [NSString stringWithFormat:@"{\"vkErrorCode\":%ld, \"message\":\"%@\"}",(long)error.vkError.errorCode,error.vkError.errorMessage];
        apiBack([NSString stringWithFormat:@"{\"%@\":%@}",code, errorJson]);
        traceToAne([NSString stringWithFormat:@"Error: %@", [error localizedDescription]]);
    }];
    
    return MakeStringCopy(returnIdRequest);
}

