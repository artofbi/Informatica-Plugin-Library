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
PLATFORM=SunStudio-Solaris-x86

# Include project Makefile
include Makefile

# Object Directory
OBJECTDIR=build/Release/${PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/IsPalindrome.o

# C Compiler Flags
CFLAGS=

# CC Compiler Flags
CCFLAGS=-mt -DUNIX -D_REENTRANT -DSunOS
CXXFLAGS=-mt -DUNIX -D_REENTRANT -DSunOS

# Fortran Compiler Flags
FFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-Release.mk /export/home/infa/SunStudioWorkspace/Output_SOs/libIsPalindrome.so

/export/home/infa/SunStudioWorkspace/Output_SOs/libIsPalindrome.so: ${OBJECTFILES}
	${MKDIR} -p /export/home/infa/SunStudioWorkspace/Output_SOs
	${LINK.cc} -G -o /export/home/infa/SunStudioWorkspace/Output_SOs/libIsPalindrome.so -KPIC -norunpath -h libIsPalindrome.so ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/IsPalindrome.o: IsPalindrome.cpp 
	${MKDIR} -p ${OBJECTDIR}
	$(COMPILE.cc) -fast -I../../DEV/_include -I/usr/include -KPIC  -o ${OBJECTDIR}/IsPalindrome.o IsPalindrome.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Release
	${RM} /export/home/infa/SunStudioWorkspace/Output_SOs/libIsPalindrome.so
	${CCADMIN} -clean

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
