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
	${OBJECTDIR}/StringContains.o

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
	${MAKE}  -f nbproject/Makefile-Release.mk /export/home/infa/SunStudioWorkspace/Output_SOs/libStringContains.so

/export/home/infa/SunStudioWorkspace/Output_SOs/libStringContains.so: ${OBJECTFILES}
	${MKDIR} -p /export/home/infa/SunStudioWorkspace/Output_SOs
	${LINK.cc} -G -o /export/home/infa/SunStudioWorkspace/Output_SOs/libStringContains.so -KPIC -norunpath -h libStringContains.so ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/StringContains.o: StringContains.cpp 
	${MKDIR} -p ${OBJECTDIR}
	$(COMPILE.cc) -fast +w -I../../DEV/_include -I/usr/include -KPIC  -o ${OBJECTDIR}/StringContains.o StringContains.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Release
	${RM} /export/home/infa/SunStudioWorkspace/Output_SOs/libStringContains.so
	${CCADMIN} -clean

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
