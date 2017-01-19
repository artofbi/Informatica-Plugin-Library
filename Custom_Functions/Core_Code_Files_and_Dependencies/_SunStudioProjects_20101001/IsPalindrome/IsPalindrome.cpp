#include <algorithm>
#include <stdlib.h>
#include <string.h>
#include <stdio.h>
#include <iostream>
#include <assert.h>
#include "sdkexpr/exprsdk.h"


//#using <system.dll>
//using namespace System;
//using namespace System::Text::RegularExpressions;


#define SAMPLE_EXPR_EXPORTS
#include "IsPalindrome.h"


using namespace std;


//static IUNICHAR *IsPalindrome_STR = (IUNICHAR *)L"Is_Palindrome";

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
    functions->validateFunction = validateFunctionIsPalindrome;
    functions->getFunctionDescription = getDescriptionIsPalindrome;
    functions->getFunctionPrototype = getPrototypeIsPalindrome;

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
    functions->module_init = moduleInitIsPalindrome;
    functions->module_deinit = moduleDeinitIsPalindrome;

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
    functions->function_init = functionInitIsPalindrome;
    functions->function_deinit = functionDeinitIsPalindrome;

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
    functions->fnInstance_init = functionInstInitIsPalindrome;
    functions->fnInstance_processRow = processRowIsPalindrome;
    functions->fnInstance_deinit = functionInstDeinitIsPalindrome;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionIsPalindrome(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a 0 (false) or 1 (true) after assessing if argument is a palindrome.";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeIsPalindrome(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"Is_Palindrome(Arg1 as String)";

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




// method to validate usage of IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionIsPalindrome(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;
    static IUNICHAR *errMsg = NULL;
    //int nArgsToCheck,i;

    // check on number of args
    // it can take either 3 or 4 or 5 arguments
    if (numArgs != 1)
    {
        static const char *argNumErr = "Incorrect number of arguments. Is_Palindrome function takes 1 argument.";

        if (!errMsg)
        {
            unsigned int len = (int)strlen(argNumErr);
            unsigned int i;
            errMsg = (IUNICHAR*)malloc(2*len+2);
            for (i=0; i<len; i++)
            {
                errMsg[i] = argNumErr[i];
            }   
            errMsg[i] = 0;
        }
        exprStatus.status = IFAILURE;
        exprStatus.errMsg = errMsg;
        return exprStatus;
    }


    retValue->datatype = eCTYPE_SHORT;
    retValue->precision = 4;
    retValue->scale = 0;

    // if input value is constant
    // return value is also constant
    // if all the arguments are constant then return value is also constant
    retValue->isValueConstant = checkArgsConstness(inputArgList, 0, numArgs-1);

    exprStatus.status = ISUCCESS;
    return exprStatus;
}



// method to process row for IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowIsPalindrome(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	//INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;


	char *strVal1;

	


	if (INFA_EXPR_GetIndicator(arg1) == INFA_EXPR_NULL_DATA)
    {
        INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_NULL_DATA);
        return INFA_EXPR_SUCCESS;
    }


	strVal1 = INFA_EXPR_GetString(arg1);


	short xReturn = IsPalindrome(strVal1);

    INFA_EXPR_SetShort(retHandle, xReturn);
	//INFA_EXPR_SetLength(retHandle, 1); //not really required, cs.
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

    return INFA_EXPR_SUCCESS;
}



short IsPalindrome(char* checkString) {

	int  i,d;
    int  length = strlen(checkString);
    char cf, cb;

    for(i=0, d=length-1 ; i < length && d >= 0 ; i++ , d--)
    {
        while(cf= toupper(checkString[i]), (cf < 'A' || cf >'Z') && i < length-1)i++;
        while(cb= toupper(checkString[d]), (cb < 'A' || cb >'Z') && d > 0)d--;
        if(cf != cb && cf >= 'A' && cf <= 'Z' && cb >= 'A' && cb <='Z')
                return 0;
    }
    return 1;

}





// method to do module level initialization for IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitIsPalindrome(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the IsPalindrome_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitIsPalindrome(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitIsPalindrome(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitIsPalindrome(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitIsPalindrome(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;

    //INFA_EXPR_OPD_RUNTIME_HANDLE *retHandle = funInstHandle->retHandle;

    //// allocate memory depending on datatype
    //if (retHandle->pOPDMetadata->datatype == eCTYPE_CHAR)
    //    retHandle->pUserDefinedPtr = new char[retHandle->pOPDMetadata->precision+1];
    //else if (retHandle->pOPDMetadata->datatype == eCTYPE_UNICHAR)
    //    retHandle->pUserDefinedPtr = new IUNICHAR[retHandle->pOPDMetadata->precision+1];
    //else if (retHandle->pOPDMetadata->datatype == eCTYPE_RAW)
    //    retHandle->pUserDefinedPtr = new unsigned char[retHandle->pOPDMetadata->precision];
    //else if (retHandle->pOPDMetadata->datatype == eCTYPE_TIME)
    //    retHandle->pUserDefinedPtr = new INFA_EXPR_DATE();
    return exprStatus;
}

// method to do function instance level deinitialization for IsPalindrome function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitIsPalindrome(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    //INFA_EXPR_OPD_RUNTIME_HANDLE *retHandle = funInstHandle->retHandle;
    //
    //if (retHandle->pOPDMetadata->datatype == eCTYPE_CHAR)
    //    delete [] (char *)retHandle->pUserDefinedPtr;
    //else if (retHandle->pOPDMetadata->datatype == eCTYPE_UNICHAR)
    //    delete [] (IUNICHAR *)retHandle->pUserDefinedPtr;
    //else if (retHandle->pOPDMetadata->datatype == eCTYPE_RAW)
    //    delete [] (unsigned char *)retHandle->pUserDefinedPtr;
    //else if (retHandle->pOPDMetadata->datatype == eCTYPE_TIME)
    //    delete (INFA_EXPR_DATE *)retHandle->pUserDefinedPtr;
    return exprStatus;
}
