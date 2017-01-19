#include <stdlib.h>
#include <string.h>
#include <iostream>
#include "sdkexpr/exprsdk.h"
#include <boost/regex.hpp>



//#using <system.dll>
//using namespace System;
//using namespace System::Text::RegularExpressions;


#define SAMPLE_EXPR_EXPORTS
#include "Is_ValidEmailAddress.h"

using namespace std;
using namespace boost; 

//static IUNICHAR *IsValidEmailAddress_STR = (IUNICHAR *)L"Is_ValidEmailAddress";

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
    functions->validateFunction = validateFunctionIsValidEmailAddress;
    functions->getFunctionDescription = getDescriptionIsValidEmailAddress;
    functions->getFunctionPrototype = getPrototypeIsValidEmailAddress;

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
    functions->module_init = moduleInitIsValidEmailAddress;
    functions->module_deinit = moduleDeinitIsValidEmailAddress;

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
    functions->function_init = functionInitIsValidEmailAddress;
    functions->function_deinit = functionDeinitIsValidEmailAddress;

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
    functions->fnInstance_init = functionInstInitIsValidEmailAddress;
    functions->fnInstance_processRow = processRowIsValidEmailAddress;
    functions->fnInstance_deinit = functionInstDeinitIsValidEmailAddress;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionIsValidEmailAddress(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a 0 (false) or 1 (true) based on if Arg1 is a valid email address.";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeIsValidEmailAddress(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"Is_ValidEmailAddress(Arg1 as string)";

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



// method to validate usage of IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionIsValidEmailAddress(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;

    // check of number of arguments
    if (numArgs != 1)
    {
        IUNICHAR *errMsg = (IUNICHAR *)L"Is_ValidEmailAddress function takes 1 argument.";
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



// method to process row for IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowIsValidEmailAddress(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	//INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;


	char *strVal1;	//, *strVal2;
	int len1;	//;, len2;


	if ((INFA_EXPR_GetIndicator(arg1) == INFA_EXPR_NULL_DATA))
    {
        INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_NULL_DATA);
        return INFA_EXPR_SUCCESS;
    }


	strVal1 = INFA_EXPR_GetString(arg1);
	len1 = INFA_EXPR_GetLength(arg1);
	/*strVal2 = INFA_EXPR_GetString(arg2);
	len2 = INFA_EXPR_GetLength(arg2);*/


	short xReturn = IsValidEmailAddress(strVal1);

    INFA_EXPR_SetShort(retHandle, xReturn);
	//INFA_EXPR_SetLength(retHandle, 1); //not really required, cs.
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

    return INFA_EXPR_SUCCESS;
}

short IsValidEmailAddress(char* address) {

	//regex expression("(?<user>[^@]+)@(?<host>.+)");

	// full email address (RFC 2822 mailbox), http://regexlib.com/REDetails.aspx?regexp_id=711
	//regex expression("^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|\"((?=[\x01-\x7f])[^\"\\]|\\[\x01-\x7f])*\"\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|\"((?=[\x01-\x7f])[^\"\\]|\\[\x01-\x7f])*\")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$");

	// Email - RFC 2821, 2822 Compliant, http://regexlib.com/REDetails.aspx?regexp_id=2558 (best)
	//see cref="http://tools.ietf.org/html/2821"/>) and RFC 2822 (<see cref="http://tools.ietf.org/html/2822"/
	regex expression("^((([!#$%&'*+\\-/=?^_`{|}~\\w])|([!#$%&'*+\\-/=?^_`{|}~\\w][!#$%&'*+\\-/=?^_`{|}~\\.\\w]{0,}[!#$%&'*+\\-/=?^_`{|}~\\w]))[@]\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)$");

	//int len = a.length();

	//m[0].matched
	//int process_ftp(const char* response, std::string* msg) 

	cmatch what; 
	if(regex_match(address, what, expression)) 
	{ 
	  // what[0] contains the whole string 
	  // what[1] contains the response code 
	  // what[2] contains the separator character 
	  // what[3] contains the text message. 
	  if(what[0].matched)
		return 1; 
	  else
		return 0;
	} 
	else
	   return 0;


}




// method to do module level initialization for IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitIsValidEmailAddress(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the IsValidEmailAddress_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitIsValidEmailAddress(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitIsValidEmailAddress(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitIsValidEmailAddress(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitIsValidEmailAddress(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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

// method to do function instance level deinitialization for IsValidEmailAddress function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitIsValidEmailAddress(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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
