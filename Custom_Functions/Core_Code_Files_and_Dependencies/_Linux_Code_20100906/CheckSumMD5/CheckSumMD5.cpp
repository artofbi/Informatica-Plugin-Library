#include <hashlibpp.h>
#include <iostream>

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include <cstdlib>
#include <ctime>
#include "sdkexpr/exprsdk.h"


#define SAMPLE_EXPR_EXPORTS
#include "CheckSumMD5.h"


using namespace std;


static IUNICHAR *CheckSumMD5_STR = (IUNICHAR *)L"CheckSumMD5";


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
    functions->validateFunction = validateFunctionCheckSumMD5;
    functions->getFunctionDescription = getDescriptionCheckSumMD5;
    functions->getFunctionPrototype = getPrototypeCheckSumMD5;

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
    functions->module_init = moduleInitCheckSumMD5;
    functions->module_deinit = moduleDeinitCheckSumMD5;

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
    functions->function_init = functionInitCheckSumMD5;
    functions->function_deinit = functionDeinitCheckSumMD5;

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
    functions->fnInstance_init = functionInstInitCheckSumMD5;
    functions->fnInstance_processRow = processRowCheckSumMD5;
    functions->fnInstance_deinit = functionInstDeinitCheckSumMD5;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionCheckSumMD5(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a string based on the MD5 algorithm after assessing all input arguments. Max 50 Arguments. Min 1 Argument.";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeCheckSumMD5(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"CheckSumMD5(Arg1 [, Arg2, Arg3, Arg4, Arg50])";

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



// method to validate usage of CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionCheckSumMD5(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;
    static IUNICHAR *errMsg = NULL;


    // check of number of arguments
    if (numArgs < 1 || numArgs > 50)
    {
        IUNICHAR *errMsg = (IUNICHAR *)L"CheckSumMD5 function takes 1 to 50 arguments";
        exprStatus.status = IFAILURE;
        exprStatus.errMsg = errMsg;
        return exprStatus;
    }


    retValue->datatype = eCTYPE_CHAR;
    retValue->precision = 100;
    retValue->scale = 0;


    // if input value is constant
    // return value is also constant
    // if all the arguments are constant then return value is also constant
	//NOTE: if no input port is considered this must be set to false.
    retValue->isValueConstant = checkArgsConstness(inputArgList, 0, numArgs-1);

    exprStatus.status = ISUCCESS;
    return exprStatus;
}



// method to process row for CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowCheckSumMD5(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;

	// set initial valuses and defaults
	char strCRCHold[8000];
	char strCRC[101];
	char strTmp[50];
	int ival;
	char *strYear, *strMonth, *strDay;
	double fval;
	INFA_EXPR_DATE *infaDate = NULL;
	
	int nArgsToCheck = fnInstance->numInputArgs;

	// Verify that the number of arguments is not greater than 50
	if(nArgsToCheck > 50)
		nArgsToCheck = 50;

	// copy the first argument into the char array to set the memory
	strcpy(strCRCHold, "");

	int i = 0;
	for (i=0; i<nArgsToCheck; i++)
    {
		//strTmp = "";
		if(INFA_EXPR_GetIndicator(fnInstance->inputOPDHandles[i]) == INFA_EXPR_NULL_DATA) //set nulls to ""
			strcat(strCRCHold, "");
		else
		{
			//////strcat(strCRCHold, INFA_EXPR_GetString(fnInstance->inputOPDHandles[i]));
			
			if(!isNumeric(fnInstance->inputOPDHandles[i]->pOPDMetadata->datatype))
			{
				switch (fnInstance->inputOPDHandles[i]->pOPDMetadata->datatype)
				{
					// Not available in this release
					////case eCTYPE_RAW:
					////	//rawval = INFA_EXPR_GetRaw(arg1);
					////	strcat(strCRCHold, INFA_EXPR_GetString( fnInstance->inputOPDHandles[i] ));
					////	break;

					////case eCTYPE_UNICHAR:
					////	ustrval = INFA_EXPR_GetUniString(arg1);
					////	strcat(strCRCHold, INFA_EXPR_GetString( fnInstance->inputOPDHandles[i] ));

					////	len = INFA_EXPR_GetLength(arg1);
					////	memcpy(retHandle->pUserDefinedPtr, ustrval, 2*(len+1));
					////	INFA_EXPR_SetUniString(retHandle, retHandle->pUserDefinedPtr);
					////	INFA_EXPR_SetLength(retHandle, len);
					////	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_GetIndicator(arg1));
					////	break;

					case eCTYPE_ITDKDATETIME:
						strcat(strCRCHold, "[ITDKDATETIME]");
						break;

					case eCTYPE_TIME: //this is the real Infa Date/Time data type.  I have no idea what ITDK is
						infaDate = INFA_EXPR_GetDate(fnInstance->inputOPDHandles[i]);
						ival = (int)infaDate->year;
						sprintf(strTmp, "%i", ival);
						strcat(strCRCHold, strTmp);

						ival = (int)infaDate->month;
						sprintf(strTmp, "%i", ival);
						strcat(strCRCHold, strTmp);
						
						ival = (int)infaDate->day;
						sprintf(strTmp, "%i", ival);
						strcat(strCRCHold, strTmp);
						break;
					default:
						strcat(strCRCHold, INFA_EXPR_GetString(fnInstance->inputOPDHandles[i]));
						break;
				}
			}
			else
			{
				// now depending on datatype
				// get the input argument
				// and set the same value in return value
				// also set the same indicator
				switch (fnInstance->inputOPDHandles[i]->pOPDMetadata->datatype)
				{
					case eCTYPE_SHORT:
						//sval = INFA_EXPR_GetShort(arg1);
						//strcat(strCRCHold, INFA_EXPR_GetString( fnInstance->inputOPDHandles[i] ));
						ival = (int)INFA_EXPR_GetShort(fnInstance->inputOPDHandles[i]);
						sprintf(strTmp, "%i", ival);
						strcat(strCRCHold, strTmp);
						break;

					case eCTYPE_LONG:
					case eCTYPE_LONG64:
						//lval = INFA_EXPR_GetLong(arg1);
						//strcat(strCRCHold, INFA_EXPR_GetString( fnInstance->inputOPDHandles[i] ));
						ival = (int)INFA_EXPR_GetLong(fnInstance->inputOPDHandles[i]);
						sprintf(strTmp, "%i", ival);
						strcat(strCRCHold, strTmp);
						break;

					case eCTYPE_INT32:
						ival = INFA_EXPR_GetInt(fnInstance->inputOPDHandles[i]);
						sprintf(strTmp, "%i", ival);
						strcat(strCRCHold, strTmp);
						break;

					case eCTYPE_DECIMAL18_FIXED:    /* Decimal number with fixed precision */
					case eCTYPE_DECIMAL28_FIXED:	/* Decimal number with fixed precision */
					case eCTYPE_ITDKDATETIME:		/* Readable date time for ITDK */
					case eCTYPE_DECIMAL18:
					case eCTYPE_DECIMAL28:
					case eCTYPE_FLOAT:
					case eCTYPE_DOUBLE:
						fval = INFA_EXPR_GetDouble(fnInstance->inputOPDHandles[i]);
						sprintf(strTmp,"%f",fval);
						strcat(strCRCHold, strTmp);
						break;

					default:
						strcat(strCRCHold, "");
						break;
				}

			} // end check for isNumeric

		} //end if for checking null argument

	} // end for loop
	


	/*
	 * creating a wrapper object
	 */
	hashwrapper *myWrapper = new md5wrapper();

	string a = string(strCRCHold);
	std::string hash1 = myWrapper->getHashFromString(a);

	sprintf(strCRC,"%s", hash1.c_str());

	delete(myWrapper);

	INFA_EXPR_SetString(retHandle, strCRC);
	INFA_EXPR_SetLength(retHandle, strlen(strCRC));
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

    return INFA_EXPR_SUCCESS;
}



// method to do module level initialization for CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitCheckSumMD5(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the CheckSumMD5_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitCheckSumMD5(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitCheckSumMD5(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitCheckSumMD5(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitCheckSumMD5(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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



	//INFA_EXPR_STATUS exprStatus;
 //   exprStatus.status = ISUCCESS;
 //   return exprStatus;
}

// method to do function instance level deinitialization for CheckSumMD5 function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitCheckSumMD5(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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

	//INFA_EXPR_STATUS exprStatus;
 //   exprStatus.status = ISUCCESS;
 //   return exprStatus;
}
