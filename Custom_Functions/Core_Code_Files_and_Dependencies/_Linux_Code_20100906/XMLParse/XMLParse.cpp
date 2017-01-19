#include "xpath_static.h"

#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include <iostream>
#include "sdkexpr/exprsdk.h"

#define SAMPLE_EXPR_EXPORTS
#include "XMLParse.h"


using namespace std;


static IUNICHAR *XMLParse_STR = (IUNICHAR *)L"XMLParse";


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
    functions->validateFunction = validateFunctionXMLParse;
    functions->getFunctionDescription = getDescriptionXMLParse;
    functions->getFunctionPrototype = getPrototypeXMLParse;

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
    functions->module_init = moduleInitXMLParse;
    functions->module_deinit = moduleDeinitXMLParse;

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
    functions->function_init = functionInitXMLParse;
    functions->function_deinit = functionDeinitXMLParse;

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
    functions->fnInstance_init = functionInstInitXMLParse;
    functions->fnInstance_processRow = processRowXMLParse;
    functions->fnInstance_deinit = functionInstDeinitXMLParse;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionXMLParse(IUNICHAR* ns, IUNICHAR* sFuncName)
{
	static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a string value based on inputs: XML Doc String (Arg1), Default Return Value (Arg2), XMLPath Query (Arg3)";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeXMLParse(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"XMLParse(XMLDocString as string, DefaultValue as string, XMLPath as string";

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



// method to validate usage of XMLParse function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionXMLParse(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;

    // check of number of arguments
    if (numArgs != 3)  // should be 3 args
    {
        IUNICHAR *errMsg = (IUNICHAR *)L"XMLParse function requires 3 arguments.";
        exprStatus.status = IFAILURE;
        exprStatus.errMsg = errMsg;
        return exprStatus;
    }

	retValue->datatype = inputArgList[0]->datatype;  //if needing a return string this must be used if a string is input otherwise validation in designer will need data type set to integer
    retValue->precision = 4000;
    //retValue->scale = 0;

    // if input value is constant
    // return value is also constant
    // if all the arguments are constant then return value is also constant
	//NOTE: *** if no input port is considered this must be set to false.
    retValue->isValueConstant = IFALSE;	//checkArgsConstness(inputArgList, 0, numArgs-1);

    exprStatus.status = ISUCCESS;
    return exprStatus;
}


// method to process row for XMLParse function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowXMLParse(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
	INFA_EXPR_OPD_RUNTIME_HANDLE* arg3 = fnInstance->inputOPDHandles[2];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;

	char s[4001];
	char *strVal1, *strVal2, *strVal3;//, *strVal4, *strVal5, *strVal6;

	if ((INFA_EXPR_GetIndicator(arg1) == INFA_EXPR_NULL_DATA)
        || (INFA_EXPR_GetIndicator(arg2) == INFA_EXPR_NULL_DATA)
        || (INFA_EXPR_GetIndicator(arg3) == INFA_EXPR_NULL_DATA))
    {
        INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_NULL_DATA);
        return INFA_EXPR_SUCCESS;
    }


	// Set arguments to strings
	strVal1 = INFA_EXPR_GetString(arg1);

	//if (INFA_EXPR_GetIndicator(arg2) != INFA_EXPR_NULL_DATA)
	strVal2 = INFA_EXPR_GetString(arg2);
	
	//if (INFA_EXPR_GetIndicator(arg3) != INFA_EXPR_NULL_DATA)
	strVal3 = INFA_EXPR_GetString(arg3);
	

	// XPath Library
	XPathParse(strVal1, strVal2, strVal3, s);


	//TiXmlDocument *p_doc;
	//TIXML_STRING S_res;
	//p_doc = new TiXmlDocument;
	////char testOut[100];

	//if(p_doc -> Parse(strVal1)){

	//
	//	S_res = TinyXPath::S_xpath_string (p_doc -> RootElement(), strVal3);
	//	
	//	sprintf(strVal2, "%s", S_res.c_str());
	//	
	//	INFA_EXPR_SetString(retHandle, strVal2);
	//	INFA_EXPR_SetLength(retHandle, strlen(strVal2));
	//}
	//else
	//{
	//	INFA_EXPR_SetString(retHandle, strVal2);
	//	INFA_EXPR_SetLength(retHandle, strlen(strVal2));
	//}

	//
	//delete p_doc; 


	INFA_EXPR_SetString(retHandle, s);
	INFA_EXPR_SetLength(retHandle, strlen(s));

	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

	free(s);

    return INFA_EXPR_SUCCESS;
	
}


void XPathParse(char *xmlDoc, char *defaultReturnVal, char *XPathQuery, char *s)
{
	TiXmlDocument * p_doc;
	TIXML_STRING S_res;
	p_doc = new TiXmlDocument;


	//std:string strTest = string(xmlDoc);
	//char *xmlstring = "<?xml version=\"1.0\"?><note type=3><to>Get It</to><from>XYZ</from><heading>Test</heading><body>Testing XML</body></note>";
	try
	{
		p_doc -> Parse(xmlDoc);

		S_res = TinyXPath::S_xpath_string (p_doc -> RootElement(), XPathQuery);

		if(S_res.length() <= 0)
			throw 1;

		strcpy(s, S_res.c_str());
	}
	catch(int &i)
	{
		strcpy(s, defaultReturnVal);
		//sprintf(s, "%s", defaultReturnVal);
	}
	delete p_doc;
}



// method to do module level initialization for XMLParse function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitXMLParse(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the XMLParse_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for XMLParse function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitXMLParse(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for XMLParse function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitXMLParse(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for XMLParse function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitXMLParse(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for XMLParse function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitXMLParse(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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

// method to do function instance level deinitialization for XMLParse function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitXMLParse(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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