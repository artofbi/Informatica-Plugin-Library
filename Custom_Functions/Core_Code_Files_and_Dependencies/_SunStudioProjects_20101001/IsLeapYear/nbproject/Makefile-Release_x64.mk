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
OBJECTDIR=build/Release_x64/${PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/isLeapYear.o

# C Compiler Flags
CFLAGS=

# CC Compiler Flags
CCFLAGS=-m64
CXXFLAGS=-m64

# Fortran Compiler Flags
FFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-Release_x64.mk /export/home/infa/SunStudioWorkspace/Output_SOs/libIsLeapYear.so

/export/home/infa/SunStudioWorkspace/Output_SOs/libIsLeapYear.so: ${OBJECTFILES}
	${MKDIR} -p /export/home/infa/SunStudioWorkspace/Output_SOs
	${LINK.cc} -G -o /export/home/infa/SunStudioWorkspace/Output_SOs/libIsLeapYear.so -KPIC -norunpath -h libIsLeapYear.so ${OBJECTFILES} ${LDLIBSOPTIONS} 

${OBJECTDIR}/isLeapYear.o: isLeapYear.cpp 
	${MKDIR} -p ${OBJECTDIR}
	$(COMPILE.cc) -g -I../../DEV/_include -I/usr/include -KPIC  -o ${OBJECTDIR}/isLeapYear.o isLeapYear.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/Release_x64
	${RM} /export/home/infa/SunStudioWorkspace/Output_SOs/libIsLeapYear.so
	${CCADMIN} -clean

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
