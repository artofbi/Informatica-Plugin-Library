#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <iostream>
#include <cstdlib>
#include <ctime>
#include "sdkexpr/exprsdk.h"
#include <boost/lexical_cast.hpp>
#include <boost/uuid/uuid_io.hpp>
#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp> 
#include <boost/uuid/uuid_serialize.hpp>





#define SAMPLE_EXPR_EXPORTS
#include "GUIDGenerator.h"


using namespace std;
using namespace boost;

//static IUNICHAR *GUIDGenerator_STR = (IUNICHAR *)L"GUIDGenerator";


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
    functions->validateFunction = validateFunctionGUIDGenerator;
    functions->getFunctionDescription = getDescriptionGUIDGenerator;
    functions->getFunctionPrototype = getPrototypeGUIDGenerator;

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
    functions->module_init = moduleInitGUIDGenerator;
    functions->module_deinit = moduleDeinitGUIDGenerator;

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
    functions->function_init = functionInitGUIDGenerator;
    functions->function_deinit = functionDeinitGUIDGenerator;

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
    functions->fnInstance_init = functionInstInitGUIDGenerator;
    functions->fnInstance_processRow = processRowGUIDGenerator;
    functions->fnInstance_deinit = functionInstDeinitGUIDGenerator;

    INFA_EXPR_STATUS retStatus;
    retStatus.status = ISUCCESS;
    retStatus.errMsg = NULL;
    return retStatus;
}

// method to get description of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getDescriptionGUIDGenerator(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniDesc = (IUNICHAR *)L"Returns a GUID.";
    return uniDesc;
}

// method to get prototype of function
extern "C" SAMPLE_EXPR_SPEC 
    IUNICHAR * getPrototypeGUIDGenerator(IUNICHAR* ns, IUNICHAR* sFuncName)
{
    static IUNICHAR *uniProt = (IUNICHAR *)L"GUIDGenerator()";

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



// method to validate usage of GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS validateFunctionGUIDGenerator(IUNICHAR* ns, IUNICHAR* sFuncName, 
                                        IUINT32 numArgs, 
                                        INFA_EXPR_OPD_METADATA** inputArgList, 
                                        INFA_EXPR_OPD_METADATA* retValue)
{
    INFA_EXPR_STATUS exprStatus;
    //static IUNICHAR *errMsg = NULL;
    //int nArgsToCheck,i;


    // check of number of arguments
    if (numArgs > 0)  // should be 3 args
    {
        IUNICHAR *errMsg = (IUNICHAR *)L"GUIDGenerator function takes 0 arguments.";
        exprStatus.status = IFAILURE;
        exprStatus.errMsg = errMsg;
        return exprStatus;
    }



	

	    // this is an echo function
    // would return whatever is coming from 
    // input
    //retValue->datatype = inputArgList[0]->datatype;
    /*retValue->precision = inputArgList[0]->precision;*/
    //retValue->scale = inputArgList[0]->scale;

    retValue->datatype = eCTYPE_CHAR;
    retValue->precision = 40;
    //retValue->scale = 0;

    // if input value is constant
    // return value is also constant
    // if all the arguments are constant then return value is also constant
	//NOTE: *** if no input port is considered this must be set to false.
    retValue->isValueConstant = IFALSE;	//checkArgsConstness(inputArgList, 0, numArgs-1);

    exprStatus.status = ISUCCESS;
    return exprStatus;
}



// method to process row for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_ROWSTATUS processRowGUIDGenerator(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *fnInstance, IUNICHAR **errMsg)
{
    //INFA_EXPR_OPD_RUNTIME_HANDLE* arg1 = fnInstance->inputOPDHandles[0];
	//INFA_EXPR_OPD_RUNTIME_HANDLE* arg2 = fnInstance->inputOPDHandles[1];
	//INFA_EXPR_OPD_RUNTIME_HANDLE* arg3 = fnInstance->inputOPDHandles[2];
    INFA_EXPR_OPD_RUNTIME_HANDLE* retHandle = fnInstance->retHandle;

	// set initial valuses and defaults
	char vGUID[41];	// = "Oceans 11";
	


	GUIDGenerator(vGUID);


	INFA_EXPR_SetString(retHandle, vGUID);
	INFA_EXPR_SetLength(retHandle, strlen(vGUID));
	INFA_EXPR_SetIndicator(retHandle, INFA_EXPR_DATA_VALID);

	/*delete s;
	free(s);*/
	
    return INFA_EXPR_SUCCESS;
}




void GUIDGenerator(char *vGUID) {

	boost::uuids::random_generator gen;
	boost::uuids::uuid u = gen();

    string str = boost::lexical_cast<std::string>(u); 

	const char *getMe = str.c_str();
	int guidLen = strlen(getMe);

	int i = 0;
	for(i = 0; i < guidLen; i++)
	{
		vGUID[i] = getMe[i];
	}

    vGUID[guidLen] = 0;
}






// method to do module level initialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleInitGUIDGenerator(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;

    // initialize the GUIDGenerator_STR
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do module level deinitialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS moduleDeinitGUIDGenerator(INFA_EXPR_MODULE_HANDLE *modHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level initialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInitGUIDGenerator(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function level deinitialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionDeinitGUIDGenerator(INFA_EXPR_FUNCTION_HANDLE *funHandle)
{
    INFA_EXPR_STATUS exprStatus;
    exprStatus.status = ISUCCESS;
    return exprStatus;
}

// method to do function instance level initialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC 
    INFA_EXPR_STATUS functionInstInitGUIDGenerator(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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

// method to do function instance level deinitialization for GUIDGenerator function
extern "C" SAMPLE_EXPR_SPEC INFA_EXPR_STATUS functionInstDeinitGUIDGenerator(INFA_EXPR_FUNCTION_INSTANCE_HANDLE *funInstHandle)
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
