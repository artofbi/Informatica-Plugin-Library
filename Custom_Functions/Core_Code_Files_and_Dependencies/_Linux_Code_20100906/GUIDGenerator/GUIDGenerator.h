#ifndef __GUIDGenerator_PLUGIN_HPP
#define __GUIDGenerator_PLUGIN_HPP

#if defined(WIN32)
    #if defined SAMPLE_EXPR_EXPORTS
        #define  SAMPLE_EXPR_SPEC __declspec(dllexport)
    #else
        #define  SAMPLE_EXPR_SPEC __declspec(dllimport)   
    #endif
#else
    #define SAMPLE_EXPR_SPEC
#endif

// method to get description of GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getDescriptionGUIDGenerator(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to get prototype of GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getPrototypeGUIDGenerator(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to validate usage of GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS validateFunctionGUIDGenerator(IUNICHAR* ns, IUNICHAR* sFuncName, 
                              IUINT32 numArgs, INFA_EXPR_OPD_METADATA** inputArgList, 
                              INFA_EXPR_OPD_METADATA* retValue);

// method to process row for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_ROWSTATUS processRowGUIDGenerator(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg);

// method to do module level initialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleInitGUIDGenerator(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do module level deinitialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleDeinitGUIDGenerator(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do function level initialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInitGUIDGenerator(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function level deinitialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionDeinitGUIDGenerator(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function instance level initialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstInitGUIDGenerator(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

// method to do function instance level deinitialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitGUIDGenerator(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

/**
    These are all plugin callbacks, which have been implemented to get various module,
    function level interfaces
*/
// method to get plugin version
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS INFA_EXPR_GetPluginVersion(INFA_VERSION* sdkVersion, INFA_VERSION* pluginVersion);

// method to delete the string allocated by this plugin. used for deleting the error
// messages
extern "C" SAMPLE_EXPR_SPEC void INFA_EXPR_DestroyString(IUNICHAR *);

// method to get validation interfaces
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS INFA_EXPR_ValidateGetUserInterface( IUNICHAR* ns, IUNICHAR* sFuncName, INFA_EXPR_VALIDATE_METHODS* functions);

// method to get module interfaces
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS INFA_EXPR_ModuleGetUserInterface(INFA_EXPR_LIB_METHODS* functions);

// method to get function interfaces
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS INFA_EXPR_FunctionGetUserInterface(IUNICHAR* nameSpaceName, IUNICHAR* functionName, INFA_EXPR_FUNCTION_METHODS* functions);

// method to get function instance interfaces
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS INFA_EXPR_FunctionInstanceGetUserInterface(IUNICHAR* nameSpaceName, IUNICHAR* functionName, INFA_EXPR_FUNCTION_INSTANCE_METHODS* functions);


void GUIDGenerator(char *vGUID);

#endif