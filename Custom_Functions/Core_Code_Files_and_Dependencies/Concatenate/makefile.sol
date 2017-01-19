# The environment variable PM_HOME should be set to point to the PowerCenter/PowerMart server install directory

CC=CC
FLAGS=-mt -KPIC -DUNIX -D_REENTRANT
DEFINES=-DSunOS
INCLUDEDIRS=-I../include
SYSLIBS=-lc
.SUFFIXES : .cpp
.cpp.o : ;$(CC) -c $(FLAGS) $(DEFINES) $(INCLUDEDIRS) $*.cpp -o $*.o

TARGET=libpmecho.so

OBJS=$(SRCS:%.cpp=%.o)

SRCS= \
SampleExprPlugin.cpp

all : $(TARGET)

$(TARGET) : $(OBJS)
	CC -o $(TARGET) -G -mt $(OBJS) $(SYSLIBS)