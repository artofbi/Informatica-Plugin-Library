#include <stdlib.h>
#include <string.h>
#include <iostream>
#include "sdkexpr/exprsdk.h"


//#using <system.dll>
//using namespace System;
//using namespace System::Text::RegularExpressions;


#define SAMPLE_EXPR_EXPORTS
#include "StringContains.h"

using namespace std;


//static IUNICHAR *StringContains_STR = (IUNICHAR *)L"StringContains";

// method to get plugin version
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS INFA_EXPR_GetPluginVersion(INFA_VERSION* sdkVersion, INFA_VERSION* pluginVersion)
{
    pluginVersion->m_major = 1;
    pluginVersion->m_minor = 0;
    pluginVersion->m_patch = 0;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to delete the string allocated by this plugin. used for deleting the error
// messages
extern "C" SAMPLE_EXPR_SPEC void INFA_EXPR_DestroyString(IUNICHAR *strToDelete)
{
	free(strToDelete);
}


// method to get validation interfaces
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS INFA_EXPR_ValidateGetUserInterface(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                                INFA_EXPR_VALIDATE_METHODS* functions)
{
    INFA_EXPR_STATUS retStatus;
    retStatus.errMsg = NULL;

    // check function name is not null
    if (!sFuncName)
    {
        retStatus.status = IFAILURE;
        return retStatus;
    }

    // set the appropriate function pointers
    functions->validateFunction = validateFunctionStringContains;
    functions->getFunctionDescription = getDescriptionStringContains;
    functions->getFunctionPrototype = getPrototypeStringContains;

    retStatus.status = ISUCCESS;
    return retStatus;
}

// method to get module interfaces
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS INFA_EXPR_ModuleGetUserInterface(INFA_EXPR_LIB_METHODS* functions)
{    
    // User would write the code here to set the function pointers. This is    
    // pretty much the same template that every plugin uses except for the    
    // change in the names of the actual functions.    
    functions->module_init = moduleInitStringContains;
    functions->module_deinit = moduleDeinitStringContains;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get function interfaces
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS INFA_EXPR_FunctionGetUserInterface(IUNICHAR* nameSpaceName, 
                                                IUNICHAR* functionName, 
                                                INFA_EXPR_FUNCTION_METHODS* functions)
{    
    functions->function_init = functionInitStringContains;
    functions->function_deinit = functionDeinitStringContains;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get function instance interfaces
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS INFA_EXPR_FunctionInstanceGetUserInterface(IUNICHAR* nameSpaceName, 
                                                        IUNICHAR* functionName, 
                                                        INFA_EXPR_FUNCTION_INSTANCE_METHODS* functions)
{    
    functions->fnInstance_init = functionInstInitStringContains;
    functions->fnInstance_processRow = processRowStringContains;
    functions->fnInstance_deinit = functionInstDeinitStringContains;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionStringContains(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a 0 (false) or 1 (true) based on if Arg2 is found in Arg1.";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeStringContains(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"StringContains(Arg1 as string, Arg2 as string)";

    return uniProt;
}



// helper function to check whether an argument is of numeric type
IBOOLEAN isNumeric(ECDataType dt)
{
    if ((dt == eCTYPE_SHORT)
        || (dt == eCTYPE_LONG)
        || (dt == eCTYPE_INT32)
        || (dt == eCTYPE_LONG64)
        || (dt == eCTYPE_FLOAT)
        || (dt == eCTYPE_DOUBLE)
        || (dt == eCTYPE_DECIMAL18_FIXED)
        || (dt == eCTYPE_DECIMAL28_FIXED))
    {
        return ITRUE;
    }
    
    return IFALSE;
}

//helper function to check whether all arguments are constant
IBOOLEAN checkArgsConstness(INFA_EXPR_OPD_METADATA** inputArgList, 
                            IUINT32 startIndex,
                            IUINT32 endIndex)
{
    IUINT32 i;
    for (i=startIndex; i<=endIndex; i++)
        if (!inputArgList[i]->isValueConstant)
            return IFALSE;
    return ITRUE;
}







//void MarshalString ( String ^ s, string& os ) {
//   using namespace Runtime::InteropServices;
//   const char* chars = 
//      (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
//   os = chars;
//   Marshal::FreeHGlobal(IntPtr((void*)chars));
//}
//
//void MarshalString ( String ^ s, wstring& os ) {
//   using namespace Runtime::InteropServices;
//   const wchar_t* chars = 
//      (const wchar_t*)(Marshal::StringToHGlobalUni(s)).ToPointer();
//   os = chars;
//   Marshal::FreeHGlobal(IntPtr((void*)chars));
//}
//
//
//
//
//




// method to validate usage of StringContains function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionStringContains(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;

    // check of number of arguments
    if (numArgs < 2 || numArgs > 2)
    {
        IUNICHAR *errMsg = (IUNICHAR *)L"StringContains function takes 2 arguments.";
        exprStatus.status = IFAILURE;
        exprStatus.errMsg = errMsg;
        return exprStatus;
    }



    retValue->datatype = eCTYPE_SHORT;
    retValue->precision = 5;
    retValue->scale = 0;

    // if input value is constant
    // return value is also constant
    // if all the arguments are constant then return value is also constant
    retValue->isValueConstant = checkArgsConstness(inputArgList, 0, numArgs-1);

    exprStatus.status = ISUCCESS;
    return exprStatus;
}



// method to process row for StringContains function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowStringContains(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;


	char *strVal1, *strVal2;
	int len1, len2;


	if ((INFA_EXPR_GetIndicator(arg1) == INFA_EXPR_NULL_DATA)
        || (INFA_EXPR_GetIndicator(arg2) == INFA_EXPR_NULL_DATA)
		)
    {
        INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_NULL_DATA);
        return INFA_EXPR_SUCCESS;
    }


	strVal1 = INFA_EXPR_GetString(arg1);
	len1 = INFA_EXPR_GetLength(arg1);
	strVal2 = INFA_EXPR_GetString(arg2);
	len2 = INFA_EXPR_GetLength(arg2);


	short xReturn = StringContains(strVal1, strVal2);

    INFA_EXPR_SetShort(retHandle, xReturn);
	//INFA_EXPR_SetLength(retHandle, 1); //not really required, cs.
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

    return INFA_EXPR_SUCCESS;
}




short StringContains(char *masterText, char *searchText) {

	string a = string(masterText);
	string b = string(searchText);
	size_t found;

	// different member versions of find in the same order as above:
	found=a.find(b);
	if (found!=string::npos)
		return 1;
	else
		return 0;
}




// method to do module level initialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitStringContains(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the StringContains_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitStringContains(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitStringContains(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitStringContains(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitStringContains(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;

    INFA_EXPR_OPD_RUNTIME_HANDLE *retHandle = funInstHandle->retHandle;

    // allocate memory depending on datatype
    if (retHandle->pOPDMetadata->datatype == eCTYPE_CHAR)
        retHandle->pUserDefinedPtr = new char[retHandle->pOPDMetadata->precision+1];
    else if (retHandle->pOPDMetadata->datatype == eCTYPE_UNICHAR)
        retHandle->pUserDefinedPtr = new IUNICHAR[retHandle->pOPDMetadata->precision+1];
    else if (retHandle->pOPDMetadata->datatype == eCTYPE_RAW)
        retHandle->pUserDefinedPtr = new unsigned char[retHandle->pOPDMetadata->precision];
    else if (retHandle->pOPDMetadata->datatype == eCTYPE_TIME)
        retHandle->pUserDefinedPtr = new INFA_EXPR_DATE();
    return exprStatus;
}

// method to do function instance level deinitialization for StringContains function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitStringContains(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    INFA_EXPR_OPD_RUNTIME_HANDLE *retHandle = funInstHandle->retHandle;
    
    if (retHandle->pOPDMetadata->datatype == eCTYPE_CHAR)
        delete [] (char *)retHandle->pUserDefinedPtr;
    else if (retHandle->pOPDMetadata->datatype == eCTYPE_UNICHAR)
        delete [] (IUNICHAR *)retHandle->pUserDefinedPtr;
    else if (retHandle->pOPDMetadata->datatype == eCTYPE_RAW)
        delete [] (unsigned char *)retHandle->pUserDefinedPtr;
    else if (retHandle->pOPDMetadata->datatype == eCTYPE_TIME)
        delete (INFA_EXPR_DATE *)retHandle->pUserDefinedPtr;
    return exprStatus;
}
