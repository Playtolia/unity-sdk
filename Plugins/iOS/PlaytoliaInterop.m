//
//  PlaytoliaInterop.m
//  Unity-iPhone
//
//  Created by Eray Ocak on 24/1/25.
//

#import <Foundation/Foundation.h>
#import <core/core.h>

void InitializePlaytolia () {
    @autoreleasepool {
        [CorePlaytoliaUI.shared attachT:@""];
    }
}

// Session interop functions
void Session_Refresh() {
    @autoreleasepool {
        [CoreSessionCompat.shared refresh];
    }
}

char* Session_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreSessionCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }
        
        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Session_UpdateUsername(const char* username) {
    @autoreleasepool {
        if (username == NULL) {
            return;
        }
        NSString* nsUsername = [NSString stringWithUTF8String:username];
        if (nsUsername != nil) {
            [CoreSessionCompat.shared updateUsernameUsername:nsUsername];
        }
    }
}

void Session_UpdateDisplayName(const char* displayName) {
    @autoreleasepool {
        if (displayName == NULL) {
            return;
        }
        NSString* nsDisplayName = [NSString stringWithUTF8String:displayName];
        if (nsDisplayName != nil) {
            [CoreSessionCompat.shared updateDisplayNameDisplayName:nsDisplayName];
        }
    }
}

void Session_UpdatePassword(const char* oldPassword, const char* newPassword) {
    @autoreleasepool {
        if (oldPassword == NULL || newPassword == NULL) {
            return;
        }
        NSString* nsOldPassword = [NSString stringWithUTF8String:oldPassword];
        NSString* nsNewPassword = [NSString stringWithUTF8String:newPassword];
        if (nsOldPassword != nil && nsNewPassword != nil) {
            [CoreSessionCompat.shared updatePasswordOldPassword:nsOldPassword newPassword:nsNewPassword];
        }
    }
}

// Store interop functions
void Store_Refresh() {
    @autoreleasepool {
        [CoreStoreCompat.shared refresh];
    }
}

char* Store_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreStoreCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }
        
        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Store_SearchItems(const char* query) {
    @autoreleasepool {
        if (query == NULL) {
            return NULL;
        }
        
        NSString* nsQuery = [NSString stringWithUTF8String:query];
        if (nsQuery == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreStoreCompat.shared searchItemsQuery:nsQuery];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Store_GetItemsByType(const char* type) {
    @autoreleasepool {
        if (type == NULL) {
            return NULL;
        }
        
        NSString* nsType = [NSString stringWithUTF8String:type];
        if (nsType == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreStoreCompat.shared getItemsByTypeType:nsType];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Store_GetItemById(const char* itemId) {
    @autoreleasepool {
        if (itemId == NULL) {
            return NULL;
        }
        
        NSString* nsItemId = [NSString stringWithUTF8String:itemId];
        if (nsItemId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreStoreCompat.shared getItemByIdId:nsItemId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Store_GetItemBySku(const char* sku) {
    @autoreleasepool {
        if (sku == NULL) {
            return NULL;
        }
        
        NSString* nsSku = [NSString stringWithUTF8String:sku];
        if (nsSku == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreStoreCompat.shared getItemBySkuSku:nsSku];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Store_BeginPurchaseFlow(const char* itemId, const char* callbackKey) {
    @autoreleasepool {
        if (itemId == NULL || callbackKey == NULL) {
            return;
        }

        NSString* nsItemId = [NSString stringWithUTF8String:itemId];
        NSString* nsCallbackKey = [NSString stringWithUTF8String:callbackKey];
        if (nsItemId != nil && nsCallbackKey != nil) {
            [CoreStoreCompat.shared beginPurchaseFlowItemId:nsItemId callbackKey:nsCallbackKey];
        }
    }
}

// Entitlements interop functions
void Entitlements_Refresh() {
    @autoreleasepool {
        [CoreEntitlementsCompat.shared refresh];
    }
}

char* Entitlements_GetAllEntitlements() {
    @autoreleasepool {
        NSString* resultString = [CoreEntitlementsCompat.shared getAllEntitlements];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Entitlements_GetEntitlementByGrantId(const char* grantId) {
    @autoreleasepool {
        if (grantId == NULL) {
            return NULL;
        }
        
        NSString* nsGrantId = [NSString stringWithUTF8String:grantId];
        if (nsGrantId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreEntitlementsCompat.shared getEntitlementByGrantIdGrantId:nsGrantId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Entitlements_GetEntitlementsByGrantId(const char* grantId) {
    @autoreleasepool {
        if (grantId == NULL) {
            return NULL;
        }
        
        NSString* nsGrantId = [NSString stringWithUTF8String:grantId];
        if (nsGrantId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreEntitlementsCompat.shared getEntitlementsByGrantIdGrantId:nsGrantId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Entitlements_GetEntitlementByItemId(const char* itemId) {
    @autoreleasepool {
        if (itemId == NULL) {
            return NULL;
        }
        
        NSString* nsItemId = [NSString stringWithUTF8String:itemId];
        if (nsItemId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreEntitlementsCompat.shared getEntitlementByItemIdItemId:nsItemId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

// Auth interop functions
void Auth_PromptLogin(bool dismissable) {
    @autoreleasepool {
        [CoreScaffoldStateful.shared promptLoginDismissable:dismissable];
    }
}

void Auth_CancelLogin() {
    @autoreleasepool {
        [CoreScaffoldStateful.shared cancelLogin];
    }
}

void Auth_Logout() {
    @autoreleasepool {
        [CoreAuthCompat.shared logout];
    }
}

char* Auth_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreAuthCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }
        
        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

bool Auth_IsLoggedIn() {
    @autoreleasepool {
        return [CoreAuthCompat.shared isLoggedIn];
    }
}

char* Auth_GetAccessToken() {
    @autoreleasepool {
        NSString* token = [CoreAuthCompat.shared getAccessToken];
        if (token == nil) {
            return NULL;
        }
        const char* utf8String = [token UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

// Scaffold interop functions
void Scaffold_HideOverlay() {
    @autoreleasepool {
        [CoreScaffoldStateful.shared hideOverlay];
    }
}

void Scaffold_ShowOverlay() {
    @autoreleasepool {
        [CoreScaffoldStateful.shared showOverlay];
    }
}

void Scaffold_Induce(const char* eventString) {
    @autoreleasepool {
        if (eventString == NULL) {
            return;
        }
        
        NSString* nsEventString = [NSString stringWithUTF8String:eventString];
        if (nsEventString != nil) {
            [CoreScaffoldStateful.shared induceEvent_:nsEventString];
        }
    }
}

void Scaffold_SetOverlayButtonAnchor(const char* anchor) {
    @autoreleasepool {
        if (anchor == NULL) return;
        NSString* value = [NSString stringWithUTF8String:anchor];
        if (value != nil) {
            [CoreScaffoldStateful.shared setOverlayButtonAnchorByNameAnchor:value];
        }
    }
}

void Scaffold_SetOverlayButtonMinimized(bool minimized) {
    @autoreleasepool {
        [CoreScaffoldStateful.shared setOverlayButtonMinimizedMinimized:minimized];
    }
}

void Scaffold_SetOverlayButtonDraggingEnabled(bool enabled) {
    @autoreleasepool {
        [CoreScaffoldStateful.shared setOverlayButtonDraggingEnabledEnabled:enabled];
    }
}

void Scaffold_SetOverlayButtonBackgroundColor(const char* color) {
    @autoreleasepool {
        if (color == NULL) return;
        NSString* value = [NSString stringWithUTF8String:color];
        if (value != nil) {
            [CoreScaffoldStateful.shared setOverlayButtonBackgroundColorColor:value];
        }
    }
}

void Scaffold_SetOverlayButtonOpacity(float opacity) {
    @autoreleasepool {
        [CoreScaffoldStateful.shared setOverlayButtonOpacityOpacity:opacity];
    }
}

void Scaffold_SetOverlayButtonIdleOpacity(float opacity) {
    @autoreleasepool {
        [CoreScaffoldStateful.shared setOverlayButtonIdleOpacityOpacity:opacity];
    }
}

void Scaffold_SetOverlayButtonIdleDelay(long long delayMilliseconds) {
    @autoreleasepool {
        [CoreScaffoldStateful.shared setOverlayButtonIdleDelayMillisDelayMillis:delayMilliseconds];
    }
}

// Subscriptions interop functions
void Subscriptions_Refresh() {
    @autoreleasepool {
        [CoreSubscriptionsCompat.shared refresh];
    }
}

char* Subscriptions_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreSubscriptionsCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }
        
        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Subscriptions_GetActiveSubscriptions() {
    @autoreleasepool {
        NSString* resultString = [CoreSubscriptionsCompat.shared getActiveSubscriptions];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Subscriptions_GetAllSubscriptions() {
    @autoreleasepool {
        NSString* resultString = [CoreSubscriptionsCompat.shared getAllSubscriptions];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Subscriptions_GetSubscriptionById(const char* subscriptionId) {
    @autoreleasepool {
        if (subscriptionId == NULL) {
            return NULL;
        }
        
        NSString* nsSubscriptionId = [NSString stringWithUTF8String:subscriptionId];
        if (nsSubscriptionId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreSubscriptionsCompat.shared getSubscriptionByIdId:nsSubscriptionId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Subscriptions_GetSubscriptionByItemId(const char* itemId) {
    @autoreleasepool {
        if (itemId == NULL) {
            return NULL;
        }
        
        NSString* nsItemId = [NSString stringWithUTF8String:itemId];
        if (nsItemId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreSubscriptionsCompat.shared getSubscriptionByItemIdItemId:nsItemId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Subscriptions_GetSubscriptionByItemSku(const char* itemSku) {
    @autoreleasepool {
        if (itemSku == NULL) {
            return NULL;
        }
        
        NSString* nsItemSku = [NSString stringWithUTF8String:itemSku];
        if (nsItemSku == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreSubscriptionsCompat.shared getSubscriptionByItemSkuItemSku:nsItemSku];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Subscriptions_GetSubscriptionsByType(const char* type) {
    @autoreleasepool {
        if (type == NULL) {
            return NULL;
        }
        
        NSString* nsType = [NSString stringWithUTF8String:type];
        if (nsType == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreSubscriptionsCompat.shared getSubscriptionsByTypeType:nsType];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

bool Subscriptions_HasActiveSubscription() {
    @autoreleasepool {
        return [CoreSubscriptionsCompat.shared hasActiveSubscription];
    }
}

bool Subscriptions_HasActiveSubscriptionOfType(const char* type) {
    @autoreleasepool {
        if (type == NULL) {
            return false;
        }
        
        NSString* nsType = [NSString stringWithUTF8String:type];
        if (nsType == nil) {
            return false;
        }
        
        return [CoreSubscriptionsCompat.shared hasActiveSubscriptionOfTypeType:nsType];
    }
}

// Wallet interop functions
void Wallet_Refresh() {
    @autoreleasepool {
        [CoreWalletCompat.shared refresh];
    }
}

char* Wallet_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreWalletCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }
        
        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Wallet_GetCurrencies() {
    @autoreleasepool {
        NSString* resultString = [CoreWalletCompat.shared getCurrencies];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Wallet_GetCurrencyById(const char* currencyId) {
    @autoreleasepool {
        if (currencyId == NULL) {
            return NULL;
        }
        
        NSString* nsCurrencyId = [NSString stringWithUTF8String:currencyId];
        if (nsCurrencyId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreWalletCompat.shared getCurrencyByIdId:nsCurrencyId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Wallet_GetCurrencyByCode(const char* code) {
    @autoreleasepool {
        if (code == NULL) {
            return NULL;
        }
        
        NSString* nsCode = [NSString stringWithUTF8String:code];
        if (nsCode == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreWalletCompat.shared getCurrencyByCodeCode:nsCode];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Wallet_GetWalletForCurrencyWithId(const char* currencyId) {
    @autoreleasepool {
        if (currencyId == NULL) {
            return NULL;
        }
        
        NSString* nsCurrencyId = [NSString stringWithUTF8String:currencyId];
        if (nsCurrencyId == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreWalletCompat.shared getWalletForCurrencyWithIdId:nsCurrencyId];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Wallet_GetWalletForCurrencyWithCode(const char* code) {
    @autoreleasepool {
        if (code == NULL) {
            return NULL;
        }
        
        NSString* nsCode = [NSString stringWithUTF8String:code];
        if (nsCode == nil) {
            return NULL;
        }
        
        NSString* resultString = [CoreWalletCompat.shared getWalletForCurrencyWithCodeCode:nsCode];
        if (resultString == nil) {
            return NULL;
        }
        
        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

// Party interop functions
void Party_Refresh() {
    @autoreleasepool {
        [CorePartyCompat.shared refresh];
    }
}

char* Party_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CorePartyCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }
        
        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Party_GetCurrentParty() {
    @autoreleasepool {
        NSString* partyString = [CorePartyCompat.shared getCurrentParty];
        if (partyString == nil) {
            return NULL;
        }
        
        const char* utf8String = [partyString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Party_GetMembers() {
    @autoreleasepool {
        NSString* membersString = [CorePartyCompat.shared getMembers];
        if (membersString == nil) {
            return NULL;
        }
        
        const char* utf8String = [membersString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

bool Party_IsLeader() {
    @autoreleasepool {
        return [CorePartyCompat.shared isLeader];
    }
}

bool Party_IsInParty() {
    @autoreleasepool {
        return [CorePartyCompat.shared isInParty];
    }
}

bool Party_IsMatchmaking() {
    @autoreleasepool {
        return [CorePartyCompat.shared isMatchmaking];
    }
}

char* Party_GetMatchmakingTicket() {
    @autoreleasepool {
        NSString* ticketString = [CorePartyCompat.shared getMatchmakingTicket];
        if (ticketString == nil) {
            return NULL;
        }
        
        const char* utf8String = [ticketString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Party_CreateParty(int maxSize, bool generateJoinCode) {
    @autoreleasepool {
        [CorePartyCompat.shared createPartyMaxSize:maxSize generateJoinCode:generateJoinCode];
    }
}

void Party_JoinByCode(const char* joinCode) {
    @autoreleasepool {
        if (joinCode == NULL) {
            return;
        }
        
        NSString* nsJoinCode = [NSString stringWithUTF8String:joinCode];
        if (nsJoinCode != nil) {
            [CorePartyCompat.shared joinByCodeJoinCode:nsJoinCode];
        }
    }
}

void Party_LeaveParty() {
    @autoreleasepool {
        [CorePartyCompat.shared leaveParty];
    }
}

void Party_KickMember(const char* memberId) {
    @autoreleasepool {
        if (memberId == NULL) {
            return;
        }
        
        NSString* nsMemberId = [NSString stringWithUTF8String:memberId];
        if (nsMemberId != nil) {
            [CorePartyCompat.shared kickMemberMemberId:nsMemberId];
        }
    }
}

void Party_TransferLeadership(const char* newLeaderId) {
    @autoreleasepool {
        if (newLeaderId == NULL) {
            return;
        }
        
        NSString* nsNewLeaderId = [NSString stringWithUTF8String:newLeaderId];
        if (nsNewLeaderId != nil) {
            [CorePartyCompat.shared transferLeadershipNewLeaderId:nsNewLeaderId];
        }
    }
}

void Party_SetReady(bool ready) {
    @autoreleasepool {
        [CorePartyCompat.shared setReadyReady:ready];
    }
}

void Party_InvitePlayer(const char* playerId) {
    @autoreleasepool {
        if (playerId == NULL) {
            return;
        }
        
        NSString* nsPlayerId = [NSString stringWithUTF8String:playerId];
        if (nsPlayerId != nil) {
            [CorePartyCompat.shared invitePlayerPlayerId:nsPlayerId];
        }
    }
}

void Party_AcceptInvite(const char* inviteId) {
    @autoreleasepool {
        if (inviteId == NULL) {
            return;
        }
        
        NSString* nsInviteId = [NSString stringWithUTF8String:inviteId];
        if (nsInviteId != nil) {
            [CorePartyCompat.shared acceptInviteInviteId:nsInviteId];
        }
    }
}

void Party_StartMatchmaking(const char* queueName) {
    @autoreleasepool {
        if (queueName == NULL) {
            return;
        }
        
        NSString* nsQueueName = [NSString stringWithUTF8String:queueName];
        if (nsQueueName != nil) {
            [CorePartyCompat.shared startMatchmakingQueueName:nsQueueName];
        }
    }
}

void Party_CancelMatchmaking() {
    @autoreleasepool {
        [CorePartyCompat.shared cancelMatchmaking];
    }
}

// Social interop functions
void Social_Refresh() {
    @autoreleasepool {
        [CoreSocialCompat.shared refresh];
    }
}

char* Social_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreSocialCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }
        
        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Social_GetFriends() {
    @autoreleasepool {
        NSString* friendsString = [CoreSocialCompat.shared getFriends];
        if (friendsString == nil) {
            return NULL;
        }
        
        const char* utf8String = [friendsString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Social_GetIncomingFriendRequests() {
    @autoreleasepool {
        NSString* requestsString = [CoreSocialCompat.shared getIncomingFriendRequests];
        if (requestsString == nil) {
            return NULL;
        }
        
        const char* utf8String = [requestsString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Social_GetOutgoingFriendRequests() {
    @autoreleasepool {
        NSString* requestsString = [CoreSocialCompat.shared getOutgoingFriendRequests];
        if (requestsString == nil) {
            return NULL;
        }
        
        const char* utf8String = [requestsString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }
        
        // Allocate memory and copy the string
        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Social_SendFriendRequest(const char* playerId) {
    @autoreleasepool {
        if (playerId == NULL) {
            return;
        }
        
        NSString* nsPlayerId = [NSString stringWithUTF8String:playerId];
        if (nsPlayerId != nil) {
            [CoreSocialCompat.shared sendFriendRequestPlayerId:nsPlayerId];
        }
    }
}

void Social_AcceptFriendRequest(const char* requestId) {
    @autoreleasepool {
        if (requestId == NULL) {
            return;
        }
        
        NSString* nsRequestId = [NSString stringWithUTF8String:requestId];
        if (nsRequestId != nil) {
            [CoreSocialCompat.shared acceptFriendRequestRequestId:nsRequestId];
        }
    }
}

void Social_RejectFriendRequest(const char* requestId) {
    @autoreleasepool {
        if (requestId == NULL) {
            return;
        }
        
        NSString* nsRequestId = [NSString stringWithUTF8String:requestId];
        if (nsRequestId != nil) {
            [CoreSocialCompat.shared rejectFriendRequestRequestId:nsRequestId];
        }
    }
}

void Social_CancelFriendRequest(const char* requestId) {
    @autoreleasepool {
        if (requestId == NULL) {
            return;
        }
        
        NSString* nsRequestId = [NSString stringWithUTF8String:requestId];
        if (nsRequestId != nil) {
            [CoreSocialCompat.shared cancelFriendRequestRequestId:nsRequestId];
        }
    }
}

void Social_RemoveFriend(const char* friendshipId) {
    @autoreleasepool {
        if (friendshipId == NULL) {
            return;
        }

        NSString* nsFriendshipId = [NSString stringWithUTF8String:friendshipId];
        if (nsFriendshipId != nil) {
            [CoreSocialCompat.shared removeFriendFriendshipId:nsFriendshipId];
        }
    }
}

char* Social_SearchPlayers(const char* query) {
    @autoreleasepool {
        if (query == NULL) {
            return NULL;
        }

        NSString* nsQuery = [NSString stringWithUTF8String:query];
        if (nsQuery == nil) {
            return NULL;
        }

        NSString* resultString = [CoreSocialCompat.shared searchPlayersQuery:nsQuery];
        if (resultString == nil) {
            return NULL;
        }

        const char* utf8String = [resultString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

// Notifications interop functions
void Notifications_Refresh() {
    @autoreleasepool {
        [CoreNotificationsCompat.shared refresh];
    }
}

char* Notifications_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreNotificationsCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }

        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Notifications_RegisterPushToken(const char* token) {
    @autoreleasepool {
        if (token == NULL) {
            return;
        }

        NSString* nsToken = [NSString stringWithUTF8String:token];
        if (nsToken != nil) {
            [CoreNotificationsCompat.shared registerPushTokenToken:nsToken];
        }
    }
}

void Notifications_RequestPushPermission() {
    @autoreleasepool {
        [CoreNotificationsCompat.shared requestPushPermission];
    }
}

void Notifications_OpenNotificationSettings() {
    @autoreleasepool {
        [CoreNotificationsCompat.shared openNotificationSettings];
    }
}

char* Notifications_GetPermission() {
    @autoreleasepool {
        NSString* permString = [CoreNotificationsCompat.shared getPermission];
        if (permString == nil) {
            return NULL;
        }

        const char* utf8String = [permString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Notifications_SendTestPush(const char* title, const char* body, const char* data) {
    @autoreleasepool {
        if (title == NULL || body == NULL) {
            return;
        }

        NSString* nsTitle = [NSString stringWithUTF8String:title];
        NSString* nsBody = [NSString stringWithUTF8String:body];
        NSString* nsData = data != NULL ? [NSString stringWithUTF8String:data] : nil;

        [CoreNotificationsCompat.shared sendTestPushTitle:nsTitle body:nsBody data:nsData];
    }
}

// Promotion interop functions
char* Promotion_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CorePromotionCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }

        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Promotion_ConfigureReview(int minSessions, int minDaysSinceInstall, int maxRequestsPerYear, int cooldownDays) {
    @autoreleasepool {
        [CorePromotionCompat.shared configureReviewMinSessions:minSessions minDaysSinceInstall:minDaysSinceInstall maxRequestsPerYear:maxRequestsPerYear cooldownDays:cooldownDays];
    }
}

void Promotion_RequestReview(bool force) {
    @autoreleasepool {
        [CorePromotionCompat.shared requestReviewForce:force];
    }
}

void Promotion_RequestReviewAfterPositiveEvent(bool force) {
    @autoreleasepool {
        [CorePromotionCompat.shared requestReviewAfterPositiveEventForce:force];
    }
}

bool Promotion_CanRequestReview() {
    @autoreleasepool {
        return [CorePromotionCompat.shared canRequestReview];
    }
}

void Promotion_SetNeverAskAgain(bool value) {
    @autoreleasepool {
        [CorePromotionCompat.shared setNeverAskAgainValue:value];
    }
}

// Analytics interop functions
char* Analytics_SerializeState() {
    @autoreleasepool {
        NSString* stateString = [CoreAnalyticsCompat.shared serializeState];
        if (stateString == nil) {
            return NULL;
        }

        const char* utf8String = [stateString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Analytics_Track(const char* eventName, const char* propertiesJson) {
    @autoreleasepool {
        if (eventName == NULL) return;

        NSString* nsEventName = [NSString stringWithUTF8String:eventName];
        NSString* nsPropertiesJson = propertiesJson != NULL
            ? [NSString stringWithUTF8String:propertiesJson]
            : nil;

        [CoreAnalyticsCompat.shared trackEventName:nsEventName propertiesJson:nsPropertiesJson];
    }
}

char* Analytics_GetDeviceId() {
    @autoreleasepool {
        NSString* deviceId = [CoreAnalyticsCompat.shared getDeviceId];
        if (deviceId == nil) {
            return NULL;
        }

        const char* utf8String = [deviceId UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Analytics_GetSessionId() {
    @autoreleasepool {
        NSString* sessionId = [CoreAnalyticsCompat.shared getSessionId];
        if (sessionId == nil) {
            return NULL;
        }

        const char* utf8String = [sessionId UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

void Analytics_Flush() {
    @autoreleasepool {
        [CoreAnalyticsCompat.shared flush];
    }
}

char* Attribution_GetAttributionDataJson() {
    @autoreleasepool {
        NSString* jsonString = [CoreAttributionCompat.shared getAttributionDataJson];
        if (jsonString == nil) {
            return NULL;
        }

        const char* utf8String = [jsonString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}

char* Attribution_GetInstallReferrerJson() {
    @autoreleasepool {
        NSString* jsonString = [CoreAttributionCompat.shared getInstallReferrerJson];
        if (jsonString == nil) {
            return NULL;
        }

        const char* utf8String = [jsonString UTF8String];
        if (utf8String == NULL) {
            return NULL;
        }

        size_t length = strlen(utf8String) + 1;
        char* result = (char*)malloc(length);
        strcpy(result, utf8String);
        return result;
    }
}
