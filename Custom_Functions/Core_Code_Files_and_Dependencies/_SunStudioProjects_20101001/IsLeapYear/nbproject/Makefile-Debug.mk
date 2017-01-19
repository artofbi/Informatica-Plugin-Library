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
OBJECTDIR=build/Debug/${PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/isLeapYear.o

# C Compiler Flags
CFLAGS=

# CC Compiler Flags
CCFLAGS=-mt
CXXFLAGS=-mt

# Fortran Compiler Flags
FFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-Debug.mk dist/Debug/${PLATFORM}/libIsLeapYear.so

dist/Debug/${PLATFORM}/libIsLeapYear.so: ${OBJECTFILES}
	${MKDIR} -p dist/Debug/${PLATFORM}
	${LINK.cc} -G -o dist/Debug/${PLATFORM}/libIsLeapYear.so -KPIC -norunpath -h libIsLeapYear.so ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/isLeapYear.o: isLeapYear.cpp 
	${MKDIR} -p ${OBJECTDIR}
	$(COMPILE.cc) -g -I../../DEV/_include -I/usr/include -KPIC  -o ${OBJECTDIR}/isLeapYear.o isLeapYear.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Debug
	${RM} dist/Debug/${PLATFORM}/libIsLeapYear.so
	${CCADMIN} -clean

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
