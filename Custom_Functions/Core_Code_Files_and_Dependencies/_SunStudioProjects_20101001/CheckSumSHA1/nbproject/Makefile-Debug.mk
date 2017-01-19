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
OBJECTDIR=build/Debug/${PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/CheckSumSHA1/../CheckSumMD5/CheckSumMD5.o

# C Compiler Flags
CFLAGS=-mt

# CC Compiler Flags
CCFLAGS=-mt -m32
CXXFLAGS=-mt -m32

# Fortran Compiler Flags
FFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-Debug.mk /export/home/infa/SunStudioWorkspace/Output_SOs/libCheckSumSHA1.so

/export/home/infa/SunStudioWorkspace/Output_SOs/libCheckSumSHA1.so: ${OBJECTFILES}
	${MKDIR} -p /export/home/infa/SunStudioWorkspace/Output_SOs
	${LINK.cc} -G -o /export/home/infa/SunStudioWorkspace/Output_SOs/libCheckSumSHA1.so -KPIC -norunpath -h libCheckSumSHA1.so ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/CheckSumSHA1/../CheckSumMD5/CheckSumMD5.o: ../CheckSumMD5/CheckSumMD5.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/CheckSumSHA1/../CheckSumMD5
	$(COMPILE.cc) -g -I/usr/include -I../../SunStudioWorkspace/_Libraries -I../../SunStudioWorkspace/_Libraries/Encryption/hashlibpp_0_3_2/src -KPIC  -o ${OBJECTDIR}/_ext/export/home/infa/SunStudioProjects/CheckSumSHA1/../CheckSumMD5/CheckSumMD5.o ../CheckSumMD5/CheckSumMD5.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Debug
	${RM} /export/home/infa/SunStudioWorkspace/Output_SOs/libCheckSumSHA1.so
	${CCADMIN} -clean

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
