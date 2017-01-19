#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Environment
MKDIR=mkdir
CP=cp
CCADMIN=CCadmin
RANLIB=ranlib
CC=cc
CCC=CC
CXX=CC
FC=f95

# Macros
PLATFORM=SunStudio_12.1-Solaris-x86

# Include project Makefile
include Makefile

# Object Directory
OBJECTDIR=build/Release/${PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xml_util.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_processor.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_static.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_syntax.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_expression.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlerror.o \
	${OBJECTDIR}/XMLParse.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/lex_util.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tokenlist.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinystr.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlparser.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/action_store.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stream.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/node_set.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stack.o \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxml.o

# C Compiler Flags
CFLAGS=

# CC Compiler Flags
CCFLAGS=-mt -m32 -DUNIX -D_REENTRANT -DSunOS
CXXFLAGS=-mt -m32 -DUNIX -D_REENTRANT -DSunOS

# Fortran Compiler Flags
FFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-Release.mk /export/home/infa/SunStudioWorkspace/Output_SOs/libXMLParse.so

/export/home/infa/SunStudioWorkspace/Output_SOs/libXMLParse.so: ${OBJECTFILES}
	${MKDIR} -p /export/home/infa/SunStudioWorkspace/Output_SOs
	${LINK.cc} -G -o /export/home/infa/SunStudioWorkspace/Output_SOs/libXMLParse.so -KPIC -norunpath -h libXMLParse.so ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xml_util.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xml_util.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xml_util.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xml_util.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_processor.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_processor.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_processor.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_processor.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_static.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_static.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_static.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_static.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_syntax.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_syntax.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_syntax.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_syntax.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_expression.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_expression.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_expression.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_expression.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlerror.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlerror.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlerror.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlerror.cpp

${OBJECTDIR}/XMLParse.o: XMLParse.cpp 
	${MKDIR} -p ${OBJECTDIR}
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/XMLParse.o XMLParse.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/lex_util.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/lex_util.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/lex_util.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/lex_util.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tokenlist.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tokenlist.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tokenlist.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tokenlist.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinystr.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinystr.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinystr.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinystr.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlparser.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlparser.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlparser.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxmlparser.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/action_store.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/action_store.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/action_store.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/action_store.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stream.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stream.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stream.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stream.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/node_set.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/node_set.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/node_set.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/node_set.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stack.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stack.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stack.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/xpath_stack.cpp

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxml.o: ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxml.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath
	$(COMPILE.cc) -fast +w -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/XML/tinyxml -I../../SunStudioWorkspace/_Libraries/XML/tinyxpath -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/XMLParse/../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxml.o ../../SunStudioWorkspace/_Libraries/XML/tinyxpath/tinyxml.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Release
	${RM} /export/home/infa/SunStudioWorkspace/Output_SOs/libXMLParse.so
	${CCADMIN} -clean

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
