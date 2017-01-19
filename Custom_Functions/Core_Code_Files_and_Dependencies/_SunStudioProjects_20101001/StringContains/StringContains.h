#ifndef __StringContains_PLUGIN_HPP
#define __StringContains_PLUGIN_HPP

#if defined(WIN32)
    #if defined SAMPLE_EXPR_EXPORTS
        #define  SAMPLE_EXPR_SPEC __declspec(dllexport)
    #else
        #define  SAMPLE_EXPR_SPEC __declspec(dllimport)   
    #endif
#else
    #define SAMPLE_EXPR_SPEC
#endif


// method to get description of StringContains function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getDescriptionStringContains(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to get prototype of StringContains function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getPrototypeStringContains(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to validate usage of StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS validateFunctionStringContains(IUNICHAR* ns, IUNICHAR* sFuncName, 
                              IUINT32 numArgs, INFA_EXPR_OPD_METADATA** inputArgList, 
                              INFA_EXPR_OPD_METADATA* retValue);

// method to process row for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_ROWSTATUS processRowStringContains(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg);

// method to do module level initialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleInitStringContains(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do module level deinitialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleDeinitStringContains(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do function level initialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInitStringContains(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function level deinitialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionDeinitStringContains(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function instance level initialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstInitStringContains(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

// method to do function instance level deinitialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitStringContains(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

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




short StringContains(char *masterText, char *searchText);







#endif
