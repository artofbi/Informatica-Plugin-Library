#ifndef __CheckSumCRC_PLUGIN_HPP
#define __CheckSumCRC_PLUGIN_HPP

#if defined(WIN32)
    #if defined SAMPLE_EXPR_EXPORTS
        #define  SAMPLE_EXPR_SPEC __declspec(dllexport)
    #else
        #define  SAMPLE_EXPR_SPEC __declspec(dllimport)   
    #endif
#else
    #define SAMPLE_EXPR_SPEC
#endif

// method to get description of CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getDescriptionCheckSumCRC(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to get prototype of CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC IUNICHAR * getPrototypeCheckSumCRC(IUNICHAR* ns, IUNICHAR* sFuncName);

// method to validate usage of CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS validateFunctionCheckSumCRC(IUNICHAR* ns, IUNICHAR* sFuncName, 
                              IUINT32 numArgs, INFA_EXPR_OPD_METADATA** inputArgList, 
                              INFA_EXPR_OPD_METADATA* retValue);

// method to process row for CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_ROWSTATUS processRowCheckSumCRC(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg);

// method to do module level initialization for CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleInitCheckSumCRC(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do module level deinitialization for CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS moduleDeinitCheckSumCRC(INFA_EXPR_MODULE_HANDLE *modHandle);

// method to do function level initialization for CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInitCheckSumCRC(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function level deinitialization for CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionDeinitCheckSumCRC(INFA_EXPR_FUNCTION_HANDLE *funHandle);

// method to do function instance level initialization for CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstInitCheckSumCRC(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

// method to do function instance level deinitialization for CheckSumCRC function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitCheckSumCRC(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle);

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



/**********************************************************************/
// http://www.createwindow.com/programming/crc32/index.htm

unsigned long crc32_table[256];  // Lookup table array
void Init_CRC32_Table();  // Builds lookup table array
unsigned long Reflect(unsigned long ref, char ch);  // Reflects CRC bits in the lookup table
int Get_CRC(char* text) ;  // Creates a CRC from a text string 

/**********************************************************************/

//inline std::string stringify(double x);
//inline std::string stringify(int x);

// http://www.koders.com/cpp/fid68B1BE8C1BF65973ABBBF64E68151120671A03BC.aspx?s=cdef%3Amd5
//unsigned long crc32(char *buf, size_t size);



#endif
