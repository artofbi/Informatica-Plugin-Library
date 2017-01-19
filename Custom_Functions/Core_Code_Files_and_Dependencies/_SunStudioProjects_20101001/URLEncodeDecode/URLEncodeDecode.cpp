#include <stdlib.h>
#include <string.h>
#include <iostream>
#include "sdkexpr/exprsdk.h"
//#include <boost/regex.hpp> 


#define SAMPLE_EXPR_EXPORTS
#include "URLEncodeDecode.h"

using namespace std;
//using namespace boost; 

//static IUNICHAR *URLEncodeDecode_STR = (IUNICHAR *)L"URLEncodeDecode";




char hexa[]={"0123456789ABCDEFabcdef"};
char nonencode[]={"abcdefghijklmnopqrstuvwqyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/\\"};

int GetHex(char val){
    int i;
    for(i=0;hexa[i];i++){
        if(hexa[i]==val){
            if(i>15){ return i-6; }
            return i;
        }
    }
    return -1;
}

void URLDecode(char* dest,int len,char* source){
    int destc=0,i;
    for(i=0;destc+1<len&&source[i];i++){
        if(source[i]=='%'&&(GetHex(source[i+1])>=0&&GetHex(source[i+2])>=0)){
            dest[destc]=GetHex(source[i+1])<<4|GetHex(source[i+2]);
            i+=2;
        }
        else if(source[i]=='+'){
            dest[destc]=' ';
        }
        else{
            dest[destc]=source[i];
        }
        destc++;
    }
    dest[destc]='\0';
}

void URLEncode(char* dest,int len,char* source, bool usePlusForSpace){

    int destc=0,i,a,clear=0;
    
	for(i=0;destc+1<len&&source[i];i++){
        clear=0;
        for(a=0;nonencode[a];a++){
            if(nonencode[a]==source[i]){
                clear=1;
                break;
            }
        }
        if(!clear){
            
			// Determine + or %20 for spaces here
			// As of version 1 and complying with web standards a + is used instead of %20
			if(source[i] == ' ')
			{

				if(destc+1>=len){
					break;
				}


				dest[destc]='+';
				destc+=1;
			}
			else{

				if(destc+3>=len){
					break;
				}


				dest[destc]='%';
				dest[destc+1]=hexa[source[i]/16];
				dest[destc+2]=hexa[source[i]%16];
				destc+=3;
			}
        }
        else{
            dest[destc]=source[i];
            destc++;
        }
    }
    dest[destc]='\0';
} 















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
    functions->validateFunction = validateFunctionURLEncodeDecode;
    functions->getFunctionDescription = getDescriptionURLEncodeDecode;
    functions->getFunctionPrototype = getPrototypeURLEncodeDecode;

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
    functions->module_init = moduleInitURLEncodeDecode;
    functions->module_deinit = moduleDeinitURLEncodeDecode;

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
    functions->function_init = functionInitURLEncodeDecode;
    functions->function_deinit = functionDeinitURLEncodeDecode;

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
    functions->fnInstance_init = functionInstInitURLEncodeDecode;
    functions->fnInstance_processRow = processRowURLEncodeDecode;
    functions->fnInstance_deinit = functionInstDeinitURLEncodeDecode;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionURLEncodeDecode(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns Encoded or Decoded URL String based on Input (Arg1) and Mode (Arg2).";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeURLEncodeDecode(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"URLEncodeDecode(Arg1 as string, Arg2 [Encode|Decode] as string)";

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



// method to validate usage of URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionURLEncodeDecode(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;

    // check of number of arguments
    if (numArgs > 2 || numArgs < 1)  // should be 1 or 2 args
    {
        IUNICHAR *errMsg = (IUNICHAR *)L"URLEncodeDecode function takes 2 arguments.";
        exprStatus.status = IFAILURE;
        exprStatus.errMsg = errMsg;
        return exprStatus;
    }




	    // this is an echo function
    // would return whatever is coming from 
    // input
    retValue->datatype = inputArgList[0]->datatype;
    /*retValue->precision = inputArgList[0]->precision;*/
    //retValue->scale = inputArgList[0]->scale;

    //retValue->datatype = eCTYPE_CHAR;
    retValue->precision = 500;
    retValue->scale = 0;

    // if input value is constant
    // return value is also constant
    // if all the arguments are constant then return value is also constant
    retValue->isValueConstant = checkArgsConstness(inputArgList, 0, numArgs-1);

    exprStatus.status = ISUCCESS;
    return exprStatus;
}



// method to process row for URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowURLEncodeDecode(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;


	char *strVal1, *strVal2;
	//char *funcOutput;
	int strVal1Len = 0;
	int maxLength = 500;

	//char *testing = "Forget about it all...";

	// Determine Arg1
	if ((INFA_EXPR_GetIndicator(arg1) == INFA_EXPR_NULL_DATA))
    {
        INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_NULL_DATA);
        return INFA_EXPR_SUCCESS;
    }
	else
		strVal1 = INFA_EXPR_GetString(arg1);


	// Determine Arg2
	if ((INFA_EXPR_GetIndicator(arg2) == INFA_EXPR_NULL_DATA))
    {
        strVal2 = "ENCODE";
    }
	else
		strVal2 = INFA_EXPR_GetString(arg2);

	
	// set var to hold output from encode/decode functions
	// the default space is set super high just in case someone tries to test the limits by accident
	char fooURLEncode[501];

	strVal1Len = strlen(strVal1);

	// attempt to mitigate a overload buffer from fooURLEncode
	if(strVal1Len >= 499)
		strVal1Len = maxLength;

	if(strcmp(strVal2,"ENCODE") == 0 
		|| strcmp(strVal2,"E") == 0
		|| strcmp(strVal2,"Encode") == 0
		|| strcmp(strVal2,"encode") == 0
		)
		URLEncode(fooURLEncode, maxLength, strVal1, false);
	else
		URLDecode(fooURLEncode, strVal1Len, strVal1);




    INFA_EXPR_SetString(retHandle, fooURLEncode);
	INFA_EXPR_SetLength(retHandle, strlen(fooURLEncode));
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

    return INFA_EXPR_SUCCESS;
}



// method to do module level initialization for URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitURLEncodeDecode(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the URLEncodeDecode_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitURLEncodeDecode(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitURLEncodeDecode(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitURLEncodeDecode(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitURLEncodeDecode(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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

// method to do function instance level deinitialization for URLEncodeDecode function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitURLEncodeDecode(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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
