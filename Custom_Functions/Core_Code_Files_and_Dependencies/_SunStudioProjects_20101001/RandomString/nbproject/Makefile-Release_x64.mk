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
OBJECTDIR=build/Release_x64/${PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/RandomString.o

# C Compiler Flags
CFLAGS=

# CC Compiler Flags
CCFLAGS=
CXXFLAGS=

# Fortran Compiler Flags
FFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-Release_x64.mk dist/Release_x64/${PLATFORM}/libRandomString.so

dist/Release_x64/${PLATFORM}/libRandomString.so: ${OBJECTFILES}
	${MKDIR} -p dist/Release_x64/${PLATFORM}
	${LINK.cc} -G -o dist/Release_x64/${PLATFORM}/libRandomString.so -KPIC -norunpath -h libRandomString.so ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/RandomString.o: RandomString.cpp 
	${MKDIR} -p ${OBJECTDIR}
	$(COMPILE.cc) -fast -I../../DEV/_include -I/usr/include -KPIC  -o ${OBJECTDIR}/RandomString.o RandomString.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Release_x64
	${RM} dist/Release_x64/${PLATFORM}/libRandomString.so
	${CCADMIN} -clean

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
