#include <stdlib.h>
#include <string.h>
#include <iostream>
#include <assert.h>
#include "sdkexpr/exprsdk.h"


//#using <system.dll>
//using namespace System;
//using namespace System::Text::RegularExpressions;


#define SAMPLE_EXPR_EXPORTS
#include "isLeapYear.h"


//using namespace std;


//static IUNICHAR *IsLeapYear_STR = (IUNICHAR *)L"Is_LeapYear";

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
    functions->validateFunction = validateFunctionIsLeapYear;
    functions->getFunctionDescription = getDescriptionIsLeapYear;
    functions->getFunctionPrototype = getPrototypeIsLeapYear;

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
    functions->module_init = moduleInitIsLeapYear;
    functions->module_deinit = moduleDeinitIsLeapYear;

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
    functions->function_init = functionInitIsLeapYear;
    functions->function_deinit = functionDeinitIsLeapYear;

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
    functions->fnInstance_init = functionInstInitIsLeapYear;
    functions->fnInstance_processRow = processRowIsLeapYear;
    functions->fnInstance_deinit = functionInstDeinitIsLeapYear;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionIsLeapYear(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a 0 (false) or 1 (true) based on if argument Year is a leap year.";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeIsLeapYear(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"Is_LeapYear(Year as int)";

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





// method to validate usage of IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionIsLeapYear(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;
    static IUNICHAR *errMsg = NULL;
    //int nArgsToCheck,i;

    // check on number of args
    // it can take either 3 or 4 or 5 arguments
    if ((numArgs > 3))
    {
        static const char *argNumErr = "Incorrect number of arguments. Is_LeapYear function takes 1 argument.";

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


	if(!isNumeric(inputArgList[0]->datatype))
	{
        static const char *notNumericErr = "Argument is not numeric.";
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
    inputArgList[0]->datatype = eCTYPE_INT32;


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



// method to process row for IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowIsLeapYear(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	//INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;


	int strVal1;//, *strVal2;
	//int len1;//, len2;
	


	if (INFA_EXPR_GetIndicator(arg1) == INFA_EXPR_NULL_DATA)
    {
        INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_NULL_DATA);
        return INFA_EXPR_SUCCESS;
    }


	strVal1 = INFA_EXPR_GetInt(arg1);
	//len1 = INFA_EXPR_GetLength(arg1);	  // ** NOTE: if the arg value is an integer the INFA_EXPR_GetLength will cause a failure
	//strVal2 = INFA_EXPR_GetString(arg2);
	//len2 = INFA_EXPR_GetLength(arg2);


	short xReturn = IsLeapYear(strVal1);

    INFA_EXPR_SetShort(retHandle, xReturn);
	//INFA_EXPR_SetLength(retHandle, 1); //not really required, cs.
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

    return INFA_EXPR_SUCCESS;
}



short IsLeapYear(int xYear) {

	assert(xYear>-4800);

	// old logic
	//short y=short(xYear+4800);       // could also use y+=0, but this reduces the range of possible values.
	//return ( (( !(y % 4) && (y % 100) ) || (!(y % 400))) ? 1 : 0);

	return (xYear%4 == 0 && (xYear %100 != 0 || xYear%400 == 0)) ? 1 : 0;

}




// method to do module level initialization for IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitIsLeapYear(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the IsLeapYear_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitIsLeapYear(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitIsLeapYear(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitIsLeapYear(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitIsLeapYear(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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

// method to do function instance level deinitialization for IsLeapYear function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitIsLeapYear(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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
