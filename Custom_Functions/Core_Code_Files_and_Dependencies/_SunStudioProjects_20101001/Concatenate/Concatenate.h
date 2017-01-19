#ifndef __Concatenate_PLUGIN_HPP
#define __Concatenate_PLUGIN_HPP

#if defined(WIN32)
    #if defined SAMPLE_EXPR_EXPORTS
        #define  SAMPLE_EXPR_SPEC __declspec(dllexport)
    #else
        #define  SAMPLE_EXPR_SPEC __declspec(dllimport)   
    #endif
#else
    #define SAMPLE_EXPR_SPEC
#endif

// method to get description of Concatenate function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getDescriptionConcatenate(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to get prototype of Concatenate function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getPrototypeConcatenate(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to validate usage of Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS validateFunctionConcatenate(IUNICHAR* ns, IUNICHAR* sFuncName, 
                              IUINT32 numArgs, INFA_EXPR_OPD_METADATA** inputArgList, 
                              INFA_EXPR_OPD_METADATA* retValue);

// method to process row for Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_ROWSTATUS processRowConcatenate(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg);

// method to do module level initialization for Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleInitConcatenate(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do module level deinitialization for Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleDeinitConcatenate(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do function level initialization for Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInitConcatenate(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function level deinitialization for Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionDeinitConcatenate(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function instance level initialization for Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstInitConcatenate(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

// method to do function instance level deinitialization for Concatenate function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitConcatenate(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

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





#endif
