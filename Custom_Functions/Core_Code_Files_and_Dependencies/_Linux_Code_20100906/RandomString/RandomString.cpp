#include <stdlib.h>
#include <stdio.h>
#include <string.h>
//#include <iostream>
#include <cstdlib>
#include <ctime>
#include "sdkexpr/exprsdk.h"


#define SAMPLE_EXPR_EXPORTS
#include "RandomString.hpp"


using namespace std;


//static IUNICHAR *RandomString_STR = (IUNICHAR *)L"RandomString";


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
    functions->validateFunction = validateFunctionRandomString;
    functions->getFunctionDescription = getDescriptionRandomString;
    functions->getFunctionPrototype = getPrototypeRandomString;

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
    functions->module_init = moduleInitRandomString;
    functions->module_deinit = moduleDeinitRandomString;

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
    functions->function_init = functionInitRandomString;
    functions->function_deinit = functionDeinitRandomString;

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
    functions->fnInstance_init = functionInstInitRandomString;
    functions->fnInstance_processRow = processRowRandomString;
    functions->fnInstance_deinit = functionInstDeinitRandomString;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionRandomString(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a random string/password based on length (Arg1), flag include numbers (Arg2), flag include special chars (Arg3).";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeRandomString(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"RandomString(Arg1 as integer, Arg2 [0|1] as integer, Arg3 [0|1] as integer)";

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



// method to validate usage of RandomString function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionRandomString(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;
    static IUNICHAR *errMsg = NULL;
    int nArgsToCheck,i;


    // check of number of arguments
    if (numArgs != 3)  // should be 3 args
    {
        IUNICHAR *errMsg = (IUNICHAR *)L"RandomString function takes 3 arguments.";
        exprStatus.status = IFAILURE;
        exprStatus.errMsg = errMsg;
        return exprStatus;
    }



	// check all arguments are numeric
    // and ask for implicit conversion to
    // INT32 datatype
    // if 5th argument is specified
    // it is of type integer

    nArgsToCheck = 3;	//(numArgs < 5) ? numArgs : 4;
    for (i=0; i<nArgsToCheck; i++)
    {
        // if not numeric then error
        if (!isNumeric(inputArgList[i]->datatype))
        {
            static const char *notNumericErr = "Argument is not numeric. Flags must be set as 0 (false) or 1 (true).";

            if (!errMsg)
                {
                    unsigned int len = (int)strlen(notNumericErr);
                    unsigned int i;
                    errMsg = (IUNICHAR*)malloc(2*len+2);
                    for (i=0; i<len; i++)
                    {
                        errMsg[i] = notNumericErr[i];
                    }   
                    errMsg[i] = 0;
                }

            exprStatus.status = IFAILURE;
            exprStatus.errMsg = errMsg;
            return exprStatus;
        }

        // set the expected type to be double
        // for implicit conversions
        inputArgList[i]->datatype = eCTYPE_INT32;
    }




	    // this is an echo function
    // would return whatever is coming from 
    // input
    //retValue->datatype = inputArgList[0]->datatype;
    /*retValue->precision = inputArgList[0]->precision;*/
    //retValue->scale = inputArgList[0]->scale;

    retValue->datatype = eCTYPE_CHAR;
    retValue->precision = 100;
    //retValue->scale = 0;

    // if input value is constant
    // return value is also constant
    // if all the arguments are constant then return value is also constant
	//NOTE: *** if no input port is considered this must be set to false.
    retValue->isValueConstant = IFALSE;	//checkArgsConstness(inputArgList, 0, numArgs-1);

    exprStatus.status = ISUCCESS;
    return exprStatus;
}



// method to process row for RandomString function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowRandomString(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
	INFA_EXPR_OPD_RUNTIME_HANDLE* arg3 = fnInstance->inputOPDHandles[2];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;

	// set initial valuses and defaults
	int length = 10;
	bool isAddNumber = false, isAddSpecial = false;
	char s[101];
	

	if ((INFA_EXPR_GetIndicator(arg1) == INFA_EXPR_NULL_DATA)
        || (INFA_EXPR_GetIndicator(arg2) == INFA_EXPR_NULL_DATA)
        || (INFA_EXPR_GetIndicator(arg2) == INFA_EXPR_NULL_DATA))
    {
        INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_NULL_DATA);
        return INFA_EXPR_SUCCESS;
    }

	length = INFA_EXPR_GetInt(arg1);

	if(length > 100 || length < 0)
		length = 10;

	if(INFA_EXPR_GetInt(arg2) == 1)
		isAddNumber = true;

	if(INFA_EXPR_GetInt(arg3) == 1)
		isAddSpecial = true;


	randomString(length, s, isAddNumber, isAddSpecial);

	

	INFA_EXPR_SetString(retHandle, s);
	INFA_EXPR_SetLength(retHandle, strlen(s));
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

	/*delete s;
	free(s);*/
	
    return INFA_EXPR_SUCCESS;
}




void randomString(int length, char *s, bool addNumber, bool addSpecial) {

	static const char alphanum1[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
	static const char alphanum2[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	static const char alphanum3[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*.";
	static const char alphanum4[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*.";

/*	//Original
	static char alphanum[] =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
		"abcdefghijklmnopqrstuvwxyz"
		"!@#$%^&*.";
*/

	int i = 0;
    
	
	if(addNumber && addSpecial){
		for (i; i < length; ++i) {
			s[i] = alphanum4[rand() % (sizeof(alphanum4) - 1)];
		}
	}
	else if(addNumber){
		for (i; i < length; ++i) {
			s[i] = alphanum2[rand() % (sizeof(alphanum2) - 1)];
		}
	}
	else if(addSpecial){
		for (i; i < length; ++i) {
			s[i] = alphanum3[rand() % (sizeof(alphanum3) - 1)];
		}
	}
	else {
		for (i; i < length; ++i) {
			s[i] = alphanum1[rand() % (sizeof(alphanum1) - 1)];
		}
	}



    s[length] = 0;
}






// method to do module level initialization for RandomString function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitRandomString(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the RandomString_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for RandomString function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitRandomString(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for RandomString function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitRandomString(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for RandomString function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitRandomString(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for RandomString function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitRandomString(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
{
    //INFA_EXPR_STATUS exprStatus;
    //exprStatus.status = ISUCCESS;

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
    //return exprStatus;

	INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level deinitialization for RandomString function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitRandomString(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
{
    /*INFA_EXPR_STATUS exprStatus;
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
    return exprStatus;*/

	INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}
